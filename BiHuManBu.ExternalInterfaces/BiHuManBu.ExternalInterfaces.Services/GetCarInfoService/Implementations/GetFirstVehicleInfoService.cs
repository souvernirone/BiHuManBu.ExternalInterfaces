using System.Configuration;
using BiHuManBu.ExternalInterfaces.Infrastructure.CacheKeyFactory;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Infrastructure.MessageCenter;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.GetCarInfoService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.RemoteMessage;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ValidateService.Interfaces;
using log4net;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.GetCarInfoService.Implementations
{
    public class GetFirstVehicleInfoService:IGetFirstVehicleInfoService
    {
        private readonly IValidateService _validateService;
        private readonly IGetAgentInfoService _getAgentInfoService;
        private readonly IUserInfoRepository _userInfoRepository;
        private readonly IQuoteReqCarinfoRepository _quoteReqCarinfoRepository;
        private readonly IGetMoldNameFromCenter _getMoldNameFromCenter;
        private readonly IMessageCenter _messageCenter;
        private ILog logInfo = LogManager.GetLogger("INFO");
        private ILog logError = LogManager.GetLogger("ERROR");
        public GetFirstVehicleInfoService(IValidateService validateService, IGetAgentInfoService getAgentInfoService, IUserInfoRepository userInfoRepository,
            IQuoteReqCarinfoRepository quoteReqCarinfoRepository, IGetMoldNameFromCenter getMoldNameFromCenter, IMessageCenter messageCenter) {
            _validateService = validateService;
            _getAgentInfoService = getAgentInfoService;
            _userInfoRepository = userInfoRepository;
            _quoteReqCarinfoRepository = quoteReqCarinfoRepository;
            _getMoldNameFromCenter = getMoldNameFromCenter;
            _messageCenter = messageCenter;
        }
        /// <summary>
        /// 新车查询车型接口第一步
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        public async Task<GetCarVehicleInfoResponse> GetNewCarVehicle(GetNewCarVehicleRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            GetCarVehicleInfoResponse response = new GetCarVehicleInfoResponse();

            //校验：1基础校验
            BaseResponse baseResponse = _validateService.Validate(request, pairs);
            if (baseResponse.Status == HttpStatusCode.Forbidden)
            {
                response.Status = HttpStatusCode.Forbidden;
                return response;
            }

            //微信端逻辑 次级代理
            if (request.ChildAgent > 0)
            {
                request.Agent = request.ChildAgent;
            }
            //if (request.MoldName.Trim().Length == 0)
            //{
            //    response.Status = HttpStatusCode.Forbidden;
            //    return response;
            //}
            string xuBaoCacheKey = CommonCacheKeyFactory.CreateKeyWithLicense(string.Format("{0}{1}{2}", request.CarVin, request.EngineNo, request.MoldName));
            var xuBaoKey = string.Format("{0}-{1}-carmodel-key", xuBaoCacheKey, request.IsNeedCarVin);
            CacheProvider.Remove(xuBaoKey);
            var msgBody = new
            {
                Agent = request.Agent,
                CarVin = request.CarVin,
                VehicleName = request.MoldName,
                cityId = request.CityCode,
                NotifyCacheKey = xuBaoCacheKey,
                RequestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            //发送续保信息
            //var msgbody = _messageCenter.SendToMessageCenter(msgBody.ToJson(), ConfigurationManager.AppSettings["MessageCenter"], ConfigurationManager.AppSettings["BxVechileQueue"]);
            try
            {
                string strurl = string.Format("{0}/service/carmodelsrecord", ConfigurationManager.AppSettings["CenterNewUrl"]);
                string returnServerIpPort = string.Empty;
                string result = ProxyCenterHttpClient.Post(strurl, msgBody.ToJson(), 60, ref returnServerIpPort);

                var cacheKey = CacheProvider.Get<string>(xuBaoKey);
                if (cacheKey == null)
                {
                    for (int i = 0; i < 60; i++)
                    {
                        cacheKey = CacheProvider.Get<string>(xuBaoKey);
                        if (!string.IsNullOrWhiteSpace(cacheKey))
                        {
                            break;
                        }
                        else
                        {
                            await Task.Delay(TimeSpan.FromSeconds(0.5));
                        }
                    }
                }
                response.Vehicles = new List<BxCarVehicleInfo>();
                if (cacheKey == null)
                {
                    response.BusinessStatus = -1;//超时
                    response.BusinessMessage = "请求超时";
                }
                else
                {
                    string itemsCache = string.Format("{0}-{1}-carmodel", xuBaoCacheKey, request.IsNeedCarVin);
                    response.BusinessStatus = 1;
                    if (cacheKey == "1")
                    {
                        var temp = CacheProvider.Get<string>(itemsCache);
                        response.Vehicles = temp.FromJson<List<BxCarVehicleInfo>>();
                    }
                    else if (cacheKey == "-10002")
                    {
                        response.BusinessStatus = -10002;
                    }
                    else if (cacheKey == "-10003")
                    {
                        response.BusinessStatus = -10003;
                    }
                    else if (cacheKey == "-10004")
                    {
                        response.BusinessStatus = -10004;
                    }
                }
            }
            catch (Exception ex)
            {
                response = new GetCarVehicleInfoResponse();
                response.Status = HttpStatusCode.ExpectationFailed;
                logError.Info("获取车型请求发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return response;
        }
    }
}
