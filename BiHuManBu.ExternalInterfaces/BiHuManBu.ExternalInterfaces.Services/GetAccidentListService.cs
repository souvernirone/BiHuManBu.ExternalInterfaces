using BiHuManBu.ExternalInterfaces.Infrastructure.CacheKeyFactory;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Infrastructure.MessageCenter;
using BiHuManBu.ExternalInterfaces.Models.PartialModels.bx_agent;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.RemoteMessage;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ValidateService.Interfaces;
using log4net;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services
{
    public class GetAccidentListService: IGetAccidentListService
    {
        private readonly IValidateService _validateService;
        private readonly IMessageCenter _messageCenter;
        private ILog logInfo = LogManager.GetLogger("INFO");
        private ILog logError = LogManager.GetLogger("ERROR");
        public GetAccidentListService(IValidateService validateService, IMessageCenter messageCenter)
        {
            _validateService = validateService;
            _messageCenter = messageCenter;
        }
        public async Task<WaBxSysJyxResponse> GetAccidentList(GetAccidentListRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            WaBxSysJyxResponse response = new WaBxSysJyxResponse();

            //参数校验
            BaseResponse baseResponse = _validateService.Validate(request, pairs);
            if (baseResponse.Status == HttpStatusCode.Forbidden)
            {
                //response.Status = HttpStatusCode.Forbidden;
                response.ErrCode = -1;
                return response;
            }

            string SpecicalCacheKey = CommonCacheKeyFactory.CreateKeyWithLicense(request.Agent.ToString() + request.CityCode.ToString() + request.Source.ToString());
            var key = string.Format("{0}-RiskOfdrive-key", SpecicalCacheKey);

            string cacheKey = CacheProvider.Get<string>(key);

            if (cacheKey != null)
            {
                if (cacheKey == "1")
                {
                    string listcachekey = string.Format("{0}-RiskOfdrive", SpecicalCacheKey);
                    response = CacheProvider.Get<WaBxSysJyxResponse>(listcachekey);
                    //response.Status = HttpStatusCode.OK;
                    response.ErrCode = 1;
                    response.ErrMsg = "获取成功";
                    //response.Key = listcachekey;
                    return response;
                }
                else
                {
                    CacheProvider.Remove(key);
                }
            }
            object msgBody;
            msgBody = new
            {
                Source = SourceGroupAlgorithm.GetOldSource(request.Source),
                CityId = request.CityCode,
                TopAgentId = request.Agent,
                NotifyCacheKey = SpecicalCacheKey
            };
            _messageCenter.SendToMessageCenter(msgBody.ToJson(), ConfigurationManager.AppSettings["MessageCenter"], ConfigurationManager.AppSettings["JYOption"]);
            for (int i = 0; i < 115; i++)
            {
                cacheKey = CacheProvider.Get<string>(key);
                //step1val = xuBaoKey;
                //step1va2 = cacheKey;
                if (!string.IsNullOrWhiteSpace(cacheKey))
                {
                    if (cacheKey == "0" || cacheKey == "1")
                        break;
                }
                else
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
            }
            if (cacheKey == null)
            {
                //response.Status = HttpStatusCode.Forbidden;
                response.ErrCode = -10003;//缓存异常
                response.ErrMsg = "请求超时或缓存异常,请重试";
                //response.Key = "";
                return response;
            }
            else if (cacheKey == "0")
            {
                //response.Status = HttpStatusCode.Forbidden;
                response.ErrCode = 0;
                response.ErrMsg = "获取特约检索失败";
                //response.Key = "";
                return response;
            }
            else
            {
                string listcachekey = string.Format("{0}-RiskOfdrive", SpecicalCacheKey);
                response = CacheProvider.Get<WaBxSysJyxResponse>(listcachekey);
                //response.Status = HttpStatusCode.OK;
                response.ErrCode = 1;
                response.ErrMsg = "获取特约成功";
                //response.Key = listcachekey;
                return response;
            }
        }
    }
}
