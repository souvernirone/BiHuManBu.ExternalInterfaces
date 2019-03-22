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
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.GetCarInfoService.Implementations
{
    public class GetNewVehicleInfoService : IGetNewVehicleInfoService
    {
        private readonly IValidateService _validateService;
        private readonly IGetAgentInfoService _getAgentInfoService;
        private readonly IUserInfoRepository _userInfoRepository;
        private readonly IQuoteReqCarinfoRepository _quoteReqCarinfoRepository;
        private readonly IGetMoldNameFromCenter _getMoldNameFromCenter;
        private readonly IMessageCenter _messageCenter;
        private ILog logInfo = LogManager.GetLogger("INFO");
        private ILog logError = LogManager.GetLogger("ERROR");
        public GetNewVehicleInfoService(IValidateService validateService, IGetAgentInfoService getAgentInfoService, IUserInfoRepository userInfoRepository,
            IQuoteReqCarinfoRepository quoteReqCarinfoRepository, IGetMoldNameFromCenter getMoldNameFromCenter, IMessageCenter messageCenter) {
            _validateService = validateService;
            _getAgentInfoService = getAgentInfoService;
            _userInfoRepository = userInfoRepository;
            _quoteReqCarinfoRepository = quoteReqCarinfoRepository;
            _getMoldNameFromCenter = getMoldNameFromCenter;
            _messageCenter = messageCenter;
        }

        /// <summary>
        /// 普通查询车型接口,第二版
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        public async Task<GetNewCarVehicleInfoResponse> GetCarVehicle(GetVehicleRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            GetNewCarVehicleInfoResponse response = new GetNewCarVehicleInfoResponse();
            //校验：1基础校验
            BaseResponse baseResponse = _validateService.Validate(request, pairs);
            if (baseResponse.Status == HttpStatusCode.Forbidden)
            {
                response.Status = HttpStatusCode.Forbidden;
                return response;
            }

            var topagent = request.Agent;
            //微信端逻辑 次级代理
            if (request.ChildAgent > 0)
            {
                request.Agent = request.ChildAgent;
            }
            //针对车架号+发动机号的逻辑
            bx_userinfo userinfo = null;
            string xuBaoCacheKey;
            if (!string.IsNullOrWhiteSpace(request.EngineNo) && !string.IsNullOrWhiteSpace(request.CarVin))
            {
                userinfo = _userInfoRepository.FindByCarvin(request.CarVin, request.EngineNo, request.CustKey,
                     request.Agent.ToString(), request.CarType);
                xuBaoCacheKey = CommonCacheKeyFactory.CreateKeyWithLicense(string.Format("{0}-{1}-{2}-{3}", topagent, request.CarVin, request.EngineNo, "moldname" + request.CarType));
            }
            else
            {
                userinfo = _userInfoRepository.FindByOpenIdAndLicense(request.CustKey, request.LicenseNo,
                   request.Agent.ToString(), request.CarType);
                xuBaoCacheKey = CommonCacheKeyFactory.CreateKeyWithLicense(string.Format("{0}-{1}-{2}", topagent, request.LicenseNo, "moldname" + request.CarType));
            }
            #region 新增逻辑 平安报价需要区分类型
            if (userinfo != null)
            {
                var reqItem = _quoteReqCarinfoRepository.Find(userinfo.Id);
                if (reqItem != null)
                {
                    reqItem.pingan_quote_type = request.IsNeedCarVin;
                    _quoteReqCarinfoRepository.Update(reqItem);
                }
                else
                {
                    reqItem = new bx_quotereq_carinfo
                    {
                        pingan_quote_type = request.IsNeedCarVin
                    };
                    _quoteReqCarinfoRepository.Add(reqItem);
                }
            }
            #endregion
            if (request.IsNeedCarVin == 0 && request.MoldName.Trim().Length == 0)
            {
                response.Status = HttpStatusCode.Forbidden;
                return response;
            }
            var xuBaoKey = string.Format("{0}-{1}-carmodelListV2-key", xuBaoCacheKey, request.IsNeedCarVin);
            CacheProvider.Remove(xuBaoKey);
            //获取品牌型号
            var moldNameViewModle = await _getMoldNameFromCenter.GetMoldNameService(request.CarVin, request.MoldName, topagent, request.CityCode);
            if (moldNameViewModle != null && !string.IsNullOrEmpty(moldNameViewModle.MoldName))
                request.MoldName = moldNameViewModle.MoldName;
            var msgBody = new
            {
                Agent = topagent,
                B_Uid = 0,
                VehicleName = request.MoldName,
                cityId = request.CityCode.ToString(),
                NotifyCacheKey = xuBaoCacheKey,
                IsNeedVin = request.IsNeedCarVin,
                CarVin = request.CarVin,
                //安心保险需要的2个参数
                LicenseNo = request.LicenseNo,
                RegisterDate = request.RegisterDate
            };            
            //发送信息
            //var msgbody = _messageCenter.SendToMessageCenter(msgBody.ToJson(),
            //    ConfigurationManager.AppSettings["MessageCenter"],
            //    ConfigurationManager.AppSettings["BxVechileQueue2"]);
            try
            {
                string strurl = string.Format("{0}/service/carModelsv2queue", ConfigurationManager.AppSettings["CenterNewUrl"]);
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
                            await Task.Delay(TimeSpan.FromSeconds(1));
                        }
                    }
                }
                response.Vehicles = new List<NewBxCarVehicleInfo>();
                if (cacheKey == null)
                {
                    response.BusinessStatus = -1;//超时
                    response.BusinessMessage = "请求超时";
                }
                else
                {
                    string itemsCache = string.Format("{0}-{1}-carmodelListV2", xuBaoCacheKey, request.IsNeedCarVin);
                    response.BusinessStatus = 1;
                    if (cacheKey == "1")
                    {
                        var temp = CacheProvider.Get<string>(itemsCache);
                        response.Vehicles = temp.FromJson<List<NewBxCarVehicleInfo>>();
                        if (response.Vehicles != null && response.Vehicles.Count > 1)
                        {
                            response.Vehicles = response.Vehicles.OrderBy(x => x.PriceT).ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response = new GetNewCarVehicleInfoResponse();
                response.Status = HttpStatusCode.ExpectationFailed;
                logError.Info("获取车型请求发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return response;
        }
    }
}
