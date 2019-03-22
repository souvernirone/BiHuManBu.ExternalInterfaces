using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Infrastructure.CacheKeyFactory;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Repository.DbOper;
using BiHuManBu.ExternalInterfaces.Services.CommonService.interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.RepeatSubmitService.interfaces;
using log4net;
using ServiceStack.Text;

namespace BiHuManBu.ExternalInterfaces.Services.RepeatSubmitService.Implementations
{
    public class RepeatSubmitService : IRepeatSubmitService
    {
        private readonly IAgentPrivilegeService _agentPrivilege;
        private readonly IRequestValidService _validService;
        private readonly IRepository<bx_userinfo> _userInfoRepository;
        private readonly IGetExpireDate _getExpireDate;
        private readonly IRepeatInfoFormat _repeatInfoFormat;
        private ILog _logError;


        public RepeatSubmitService(IAgentPrivilegeService agentPrivilege, IRequestValidService validService, IRepository<bx_userinfo> userInfoRepository, IGetExpireDate getExpireDate, IRepeatInfoFormat repeatInfoFormat)
        {
            _agentPrivilege = agentPrivilege;
            _validService = validService;
            _userInfoRepository = userInfoRepository;
            _getExpireDate = getExpireDate;
            _repeatInfoFormat = repeatInfoFormat;
            _logError = LogManager.GetLogger("ERROR");
        }
        public async Task<GetRepeatSubmitResponse> GetRepeatSubmitInfo(GetRepeatSubmitRequest request,
            IEnumerable<KeyValuePair<string, string>> pairs)
        {
            var response = new GetRepeatSubmitResponse();
            try
            {
                var privilegeResulst = _agentPrivilege.CanUse(request.Agent);
                if (!privilegeResulst.CheckResult)
                {
                    response.Status = HttpStatusCode.Forbidden;
                    return response;
                }
                if (!_validService.ValidateReqest(pairs, privilegeResulst.Model.SecretKey, request.SecCode))
                {
                    response.Status = HttpStatusCode.Forbidden;
                    return response;
                }

                //微信端逻辑 次级代理
                if (request.ChildAgent > 0)
                {
                    request.Agent = request.ChildAgent;
                }

                var queryAgent = request.Agent.ToString();
                var userinfo =
                    _userInfoRepository.Search(
                        x =>
                            x.Agent == queryAgent && x.OpenId == request.CustKey &&
                            x.RenewalCarType == request.RenewalCarType && x.IsTest == 0&&x.LicenseNo==request.LicenseNo).FirstOrDefault();
                if (userinfo == null)
                {
                    response.Status = HttpStatusCode.NoContent;
                    return response;
                }
                if (userinfo.IsSingleSubmit > 0 && userinfo.QuoteStatus == -1 && userinfo.UpdateTime.GetValueOrDefault().AddMinutes(5) > DateTime.Now)
                {
                    response.Status = HttpStatusCode.Conflict;
                    return response;
                }

                string baojiaCacheKey =
                CommonCacheKeyFactory.CreateKeyWithLicenseAndAgentAndCustKey(request.LicenseNo, request.Agent, request.CustKey + request.RenewalCarType);
                var lastinfo = await _getExpireDate.GetDate(baojiaCacheKey);

                var formatInfo = _repeatInfoFormat.FormatRepeatInfo(userinfo.IsSingleSubmit.GetValueOrDefault(), baojiaCacheKey);

                response = RepeatSubmitResponseFactory(lastinfo, formatInfo);
            }
            catch (Exception ex)
            {

                response.Status = HttpStatusCode.ExpectationFailed;
                _logError.Info("获取重复投保请求发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException + ",请求对象信息：" + request.ToJson() + ";返回对象信息" + response.ToJson());
            }
            

            return response;

        }

        private GetRepeatSubmitResponse RepeatSubmitResponseFactory(bx_lastinfo lastinfo,RepeatInfoFormatModel formatInfo)
        {
            var response = new GetRepeatSubmitResponse();
            if (lastinfo != null)
            {
                response.RepeatSubmitMessage = formatInfo.RepeatMsg;
                response.ForceExpireDate = string.IsNullOrWhiteSpace(lastinfo.last_end_date)
                    ? string.Empty
                    : DateTime.Parse(lastinfo.last_end_date).ToString("yyyy-MM-dd HH:mm:ss");
                response.BusinessExpireDate = string.IsNullOrWhiteSpace(lastinfo.last_business_end_date)
                    ? string.Empty
                    : DateTime.Parse(lastinfo.last_business_end_date).ToString("yyyy-MM-dd HH:mm:ss");
                response.RepeatSubmitResult = formatInfo.RepeatType;
                response.CompositeRepeatType = formatInfo.CompositeRepeatType;
                response.RepeatSubmitPerComp = formatInfo.RepeatPerCompany;
            }
            else
            {
                response.RepeatSubmitMessage = "没有重复投保";
                response.RepeatSubmitResult = 0;
                response.ForceExpireDate = string.Empty;
                response.BusinessExpireDate = string.Empty;
                response.CompositeRepeatType = 0;
                response.RepeatSubmitPerComp = new Dictionary<int, int>();
            }

            return response;
        }
    }
}