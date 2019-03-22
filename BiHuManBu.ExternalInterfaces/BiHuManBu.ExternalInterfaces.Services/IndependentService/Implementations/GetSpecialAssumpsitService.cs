using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Infrastructure.CacheKeyFactory;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Infrastructure.MessageCenter;
using BiHuManBu.ExternalInterfaces.Services.IndependentService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ValidateService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.ViewModels.IndependentViewModel;
using ServiceStack.Text;

namespace BiHuManBu.ExternalInterfaces.Services.IndependentService.Implementations
{
    public class GetSpecialAssumpsitService : IGetSpecialAssumpsitService
    {
        private readonly IValidateService _validateService;
        private readonly IUserInfoValidateService _userInfoValidateService;
        private readonly IMessageCenter _messageCenter;
        public GetSpecialAssumpsitService(IValidateService validateService, IUserInfoValidateService userInfoValidateService,
            IMessageCenter messageCenter)
        {
            _validateService = validateService;
            _userInfoValidateService = userInfoValidateService;
            _messageCenter = messageCenter;
        }
        public async Task<GetSpecialAssumpsitResponse> GetSpecialAssumpsit(GetSpecialAssumpsitRequest request, IEnumerable<KeyValuePair<string, string>> pair)
        {
            GetSpecialAssumpsitResponse response = new GetSpecialAssumpsitResponse();
            //校验：1基础校验
            BaseResponse baseResponse = _validateService.Validate(request, pair);
            if (baseResponse.Status == HttpStatusCode.Forbidden)
            {
                response.Status = HttpStatusCode.Forbidden;
                return response;
            }
            //校验：2报价基础信息
            UserInfoValidateRequest validateRequest = new UserInfoValidateRequest()
            {
                LicenseNo = request.LicenseNo,
                CustKey = request.CustKey,
                ChildAgent = request.ChildAgent == 0 ? request.Agent : request.ChildAgent,
                RenewalCarType = request.RenewalCarType
            };
            //校验2
            var validateResult = _userInfoValidateService.UserInfoValidate(validateRequest);
            if (validateResult.Item1.Status == HttpStatusCode.NotAcceptable)
            {
                response.Status = HttpStatusCode.NotAcceptable;
                return response;
            }
            //正式逻辑=============
            string baojiaCacheKey = CommonCacheKeyFactory.CreateKeyWithLicenseAndAgentAndCustKey(request.LicenseNo, request.Agent, request.CustKey + request.RenewalCarType);
            //通知中心
            var msgBody = new
            {
                B_Uid = validateResult.Item2.Id,
                Source = SourceGroupAlgorithm.GetOldSource(request.Source),
                NotifyCacheKey = baojiaCacheKey,
            };
            //发送安心核保消息
            try
            {
                var baojiaKey = string.Format("{0}-GenerateSpecial-key", baojiaCacheKey);
                CacheProvider.Remove(baojiaKey);

                var msgbody = _messageCenter.SendToMessageCenter(msgBody.ToJson(),
                    ConfigurationManager.AppSettings["MessageCenter"],
                    ConfigurationManager.AppSettings["bxAnXinTeYue"]);

                var cacheKey = CacheProvider.Get<string>(baojiaKey);
                if (cacheKey == null)
                {
                    for (int i = 0; i < 180; i++)
                    {
                        cacheKey = CacheProvider.Get<string>(baojiaKey);
                        if (!string.IsNullOrWhiteSpace(cacheKey))
                        {
                            break;
                        }
                        else
                        {
                            await Task.Delay(TimeSpan.FromSeconds(1));
                        }
                    }
                }
                if (cacheKey == "1")
                {
                    var msgModel = CacheProvider.Get<AXGainFixSpecInfoResponse>(string.Format("{0}-GenerateSpecial", baojiaCacheKey));
                    List<SpecialVO> SpecialContents = new List<SpecialVO>();
                    if (msgModel != null)
                    {
                        //取交强模型
                        if (msgModel.PackageJQVO != null)
                        {
                            if (msgModel.PackageJQVO.FixSpecList != null)
                            {
                                foreach (var item in msgModel.PackageJQVO.FixSpecList)
                                {
                                    SpecialContents.Add(new SpecialVO()
                                    {
                                        CSpecNo = item.cSpecNo,
                                        CSysSpecContent = item.cSysSpecContent,
                                        Type = 1
                                    });
                                }
                            }
                        }
                        //取商业模型
                        if (msgModel.PackageSYVO != null)
                        {
                            if (msgModel.PackageSYVO.FixSpecList != null)
                            {
                                foreach (var item in msgModel.PackageSYVO.FixSpecList)
                                {
                                    SpecialContents.Add(new SpecialVO()
                                    {
                                        CSpecNo = item.cSpecNo,
                                        CSysSpecContent = item.cSysSpecContent,
                                        Type = 2
                                    });
                                }
                            }
                        }
                    }
                    response.SpecialContents = SpecialContents;
                }
            }
            catch (MessageException exception)
            {
                response.Status = HttpStatusCode.ExpectationFailed;
                response.ErrMsg = exception.Message;
                return response;
            }
            return response;
        }
    }
}
