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
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.PartialModels.bx_agent;
using BiHuManBu.ExternalInterfaces.Services.IndependentService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ValidateService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.ViewModels.IndependentViewModel;
using ServiceStack.Text;

namespace BiHuManBu.ExternalInterfaces.Services.IndependentService.Implementations
{
    public class GetFloatingInfoService : IGetFloatingInfoService
    {
        private readonly IValidateService _validateService;
        private readonly IUserInfoValidateService _userInfoValidateService;
        private readonly IMessageCenter _messageCenter;
        private readonly ISubmitInfoRepository _submitInfoRepository;
        public GetFloatingInfoService(IValidateService validateService, IUserInfoValidateService userInfoValidateService,
            IMessageCenter messageCenter, ISubmitInfoRepository submitInfoRepository)
        {
            _validateService = validateService;
            _userInfoValidateService = userInfoValidateService;
            _messageCenter = messageCenter;
            _submitInfoRepository = submitInfoRepository;
        }
        public async Task<GetFloatingInfoResponse> GetFloatingInfo(GetFloatingInfoRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            GetFloatingInfoResponse response = new GetFloatingInfoResponse();
            //校验：1基础校验
            BaseResponse baseResponse = _validateService.Validate(request, pairs);
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
            //校验：4是否存在核保记录
            bx_submit_info submitInfo = _submitInfoRepository.GetSubmitInfo(validateResult.Item2.Id,
                SourceGroupAlgorithm.GetOldSource(request.Source));

            string baojiaCacheKey = CommonCacheKeyFactory.CreateKeyWithLicenseAndAgentAndCustKey(request.LicenseNo, request.Agent, request.CustKey + request.RenewalCarType);
            string notifyCacheKey = string.Format("{0}-gzd", baojiaCacheKey);
            //通知中心
            var msgBody = new
            {
                B_Uid = validateResult.Item2.Id,
                Source = SourceGroupAlgorithm.GetOldSource(request.Source),
                BiztNo = submitInfo != null ? submitInfo.biz_tno : "",
                ForcetNo = submitInfo != null ? submitInfo.force_tno : "",
                NotifyCacheKey = notifyCacheKey
            };
            //发送安心核保消息
            try
            {
                string baojiaKey = string.Format("{0}-Informing", notifyCacheKey);
                CacheProvider.Remove(baojiaKey);

                var msgbody = _messageCenter.SendToMessageCenter(msgBody.ToJson(),
                    ConfigurationManager.AppSettings["MessageCenter"],
                    ConfigurationManager.AppSettings["bxAnXinGaoZhi"]);

                var cacheKey = CacheProvider.Get<string>(baojiaKey);
                if (cacheKey == null)
                {
                    for (int i = 0; i < 180; i++)
                    {
                        cacheKey = CacheProvider.Get<string>(baojiaKey);
                        if (cacheKey != null)
                        {
                            break;
                        }
                        else
                        {
                            await Task.Delay(TimeSpan.FromSeconds(1));
                        }
                    }
                }
                JSFloatingNotificationPrintListResponse jsonModel = new JSFloatingNotificationPrintListResponse();
                if (!string.IsNullOrEmpty(cacheKey))
                {
                    jsonModel = cacheKey.FromJson<JSFloatingNotificationPrintListResponse>();
                }
                response.JSFloatingNotificationPrintList = jsonModel.JSFloatingNotificationPrintListResponseMain;
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
