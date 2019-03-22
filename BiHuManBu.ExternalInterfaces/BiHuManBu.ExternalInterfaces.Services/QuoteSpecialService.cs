using BiHuManBu.ExternalInterfaces.Infrastructure.CacheKeyFactory;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Infrastructure.MessageCenter;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.IRepository;
using BiHuManBu.ExternalInterfaces.Services.CacheServices;
using BiHuManBu.ExternalInterfaces.Services.GetPrecisePriceService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.GetSubmitInfoService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.PrecisePriceService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.ValidateService.Interfaces;
using log4net;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services
{
    public class QuoteSpecialService : CommonBehaviorService, IQuoteSpecialService
    {
        private static readonly string _url = ConfigurationManager.AppSettings["BaoxianCenter"];
        private static readonly string _apiurl = ConfigurationManager.AppSettings["RateCentertest"];
        private const string PreXubaoCacheKey = "xubao_cach_key_";
        private ISaveQuoteRepository _saveQuoteRepository;
        private IUserInfoRepository _userInfoRepository;
        private ILoginService _loginService;
        private ILastInfoRepository _infoRepository;
        private ISubmitInfoRepository _submitInfoRepository;
        private IQuoteResultRepository _quoteResultRepository;
        private ILog logInfo = LogManager.GetLogger("INFO");
        private ILog logError = LogManager.GetLogger("ERROR");
        private IAgentRepository _agentRepository;
        private IMessageCenter _messageCenter;
        private ICarInfoRepository _carInfoRepository;
        private ICacheHelper _cacheHelper;
        private ICarInsuranceCache _carInsuranceCache;
        private ICarRenewalRepository _carRenewalRepository;
        private IDeviceDetailRepository _detailRepository;
        private IQuoteReqCarinfoRepository _quoteReqCarinfoRepository;
        private IQuoteResultCarinfoRepository _quoteResultCarinfoRepository;
        private IAgentConfigRepository _agentConfig;
        private IConfigRepository _configRepository;
        private INoticexbService _noticexbService;
        private ICarModelRepository _carModelRepository;
        private readonly IMultiChannelsService _multiChannelsService;
        private readonly ICheckRequestGetPrecisePrice _checkRequestGetPrecisePrice;
        private readonly ICheckRequestGetSubmitInfo _checkRequestGetSubmitInfo;
        private readonly IGetAgentInfoService _getAgentInfoService;
        private readonly ISpecialOptionService _specialOptionService;

        public QuoteSpecialService(ISaveQuoteRepository saveQuoteRepository, IUserInfoRepository userInfoRepository, ILoginService loginService, ISubmitInfoRepository submitInfoRepository,
            IQuoteResultRepository quoteResultRepository, ILastInfoRepository lastInfoRepository, IAgentRepository agentRepository, IMessageCenter messageCenter,
            ICarInfoRepository carInfoRepository, IRenewalQuoteRepository renewalQuoteRepository, IQuoteReqCarinfoRepository quoteReqCarinfoRepository, IQuoteResultCarinfoRepository quoteResultCarinfoRepository,
            ICacheHelper cacheHelper, ICarInsuranceCache carInsuranceCache, ICarRenewalRepository carRenewalRepository, IDeviceDetailRepository detailRepository, IAgentConfigRepository agentConfig, INoticexbService noticexbService, IConfigRepository configRepository,
            ICarModelRepository carModelRepository, IMultiChannelsService multiChannelsService, ICheckRequestGetPrecisePrice checkRequestGetPrecisePrice, ICheckRequestGetSubmitInfo checkRequestGetSubmitInfo, IGetAgentInfoService getAgentInfoService, ISpecialOptionService specialOptionService)
            : base(agentRepository, cacheHelper)
        {
            _saveQuoteRepository = saveQuoteRepository;
            _userInfoRepository = userInfoRepository;
            _loginService = loginService;
            _infoRepository = lastInfoRepository;
            _submitInfoRepository = submitInfoRepository;
            _quoteResultRepository = quoteResultRepository;
            _agentRepository = agentRepository;
            _messageCenter = messageCenter;
            _carInfoRepository = carInfoRepository;
            _carInsuranceCache = carInsuranceCache;
            _carRenewalRepository = carRenewalRepository;
            _detailRepository = detailRepository;
            _quoteReqCarinfoRepository = quoteReqCarinfoRepository;
            _quoteResultCarinfoRepository = quoteResultCarinfoRepository;
            _agentConfig = agentConfig;
            _noticexbService = noticexbService;
            _configRepository = configRepository;
            _carModelRepository = carModelRepository;
            _multiChannelsService = multiChannelsService;
            _checkRequestGetPrecisePrice = checkRequestGetPrecisePrice;
            _checkRequestGetSubmitInfo = checkRequestGetSubmitInfo;
            _getAgentInfoService = getAgentInfoService;
            _specialOptionService = specialOptionService;
        }
        public async Task<SpecialListResponse> GetSpeciaList(GetSpecialListRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {

            SpecialListResponse response = new SpecialListResponse();
            string ukeyId;
            var agentConfig = _agentConfig.FindBy(request.AgentId, request.CityId).FirstOrDefault(conf => conf.source == SourceGroupAlgorithm.GetOldSource(request.Source));
            if (agentConfig == null)
            {
                response.Status = HttpStatusCode.Forbidden;
                response.BusinessStatus = -10001;
                response.BusinessMessage = "没有找到代理人配置";
                return response;
            }
            else
            {
                ukeyId = agentConfig.ukey_id.ToString();
            }
            bx_agent agentModel = GetAgent(request.AgentId);
            //参数校验
            if (agentModel == null)
            {
                response.Status = HttpStatusCode.Forbidden;
                response.BusinessStatus = -10001;
                response.BusinessMessage = "没有找到代理人";
                return response;
            }

            if (!ValidateReqest(pairs, agentModel.SecretKey, request.SecCode))
            {
                response.Status = HttpStatusCode.Forbidden;
                response.BusinessStatus = -10001;
                response.BusinessMessage = "参数校验失败";
                return response;
            }

            string SpecicalCacheKey = CommonCacheKeyFactory.CreateKeyWithLicense(request.AgentId.ToString() + request.CityId.ToString() + request.Source.ToString());
            var key = string.Format("{0}-SpecialOptions-key", SpecicalCacheKey);

            string cacheKey = CacheProvider.Get<string>(key);

            if (cacheKey != null)
            {
                if (cacheKey == "1")
                {
                    string listcachekey = string.Format("{0}-SpecialOptions", SpecicalCacheKey);
                    response = CacheProvider.Get<SpecialListResponse>(listcachekey);
                    response.Status = HttpStatusCode.OK;
                    response.BusinessStatus = 1;
                    response.BusinessMessage = "获取特约成功";
                    response.Key = listcachekey;
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
                CityId = request.CityId,
                TopAgentId = request.AgentId,
                ukeyId = ukeyId,
                NotifyCacheKey = SpecicalCacheKey
            };
            _messageCenter.SendToMessageCenter(msgBody.ToJson(), ConfigurationManager.AppSettings["MessageCenter"], ConfigurationManager.AppSettings["SpecialOption"]);
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
                response.Status = HttpStatusCode.Forbidden;
                response.BusinessStatus = -10003;//缓存异常
                response.BusinessMessage = "请求超时或缓存异常,请重试";
                response.Key = "";
                return response;
            }
            else if (cacheKey == "0")
            {
                response.Status = HttpStatusCode.Forbidden;
                response.BusinessStatus = 0;
                response.BusinessMessage = "获取特约检索失败";
                response.Key = "";
                return response;
            }
            else
            {
                string listcachekey = string.Format("{0}-SpecialOptions", SpecicalCacheKey);
                response = CacheProvider.Get<SpecialListResponse>(listcachekey);
                response.Status = HttpStatusCode.OK;
                response.BusinessStatus = 1;
                response.BusinessMessage = "获取特约成功";
                response.Key = listcachekey;
                return response;
            }
        }
    }
}
