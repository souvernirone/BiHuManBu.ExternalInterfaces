using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.IRepository;
using BiHuManBu.ExternalInterfaces.Models.PartialModels.bx_agent;
using BiHuManBu.ExternalInterfaces.Models.ReportModel;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using log4net;

namespace BiHuManBu.ExternalInterfaces.Services
{
    public class CityService : CommonBehaviorService, ICityService
    {
        private ICityRepository _cityRepository;
        private IAgentRepository _agentRepository;
        private ICacheHelper _cacheHelper;
        private ILoginService _loginService;
        private ILog logError = LogManager.GetLogger("ERROR");
        private IAgentConfigRepository _agentConfigRepository;
        private ICityQuoteDayRepository _dayRepository;
        public CityService(ICityRepository cityRepository, ILoginService loginService, ICacheHelper cacheHelper, IAgentRepository agentRepository, IAgentConfigRepository agentConfigRepository
            , ICityQuoteDayRepository dayRepository)
            : base(agentRepository, cacheHelper)
        {
            _cityRepository = cityRepository;
            _agentRepository = agentRepository;
            _cacheHelper = cacheHelper;
            _loginService = loginService;
            _agentConfigRepository = agentConfigRepository;
            _dayRepository = dayRepository;
        }
        public async Task<GetCityListResponse> GetCityList(GetCityListRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            var response = new GetCityListResponse();
            //根据经纪人获取手机号 
            IBxAgent agentModel = GetAgentModelFactory(request.Agent);
            //logInfo.Info("获取到的经纪人信息:"+agentModel.ToJson());
            //参数校验
            if (!agentModel.AgentCanUse())
            {
                response.Status = HttpStatusCode.Forbidden;
                return response;
            }

            if (!ValidateReqest(pairs, agentModel.SecretKey, request.SecCode))
            {
                response.Status = HttpStatusCode.Forbidden;
                return response;
            }
            try
            {
                var cities = FindList();
                var configedCities = new List<bx_city>();
                var configCities = FindConfigCityList(int.Parse(_agentRepository.GetTopAgentId(request.Agent)));
                var configedCityIds = configCities.Select(x => x.city_id).Distinct();
                if (configedCityIds.Any())
                {
                    foreach (var i in configedCityIds)
                    {
                        var config = (int)i;
                        var tmpcity = cities.FirstOrDefault(x => x.id == config);
                        if (tmpcity != null)
                        {
                            configedCities.Add(tmpcity);
                        }
                    }
                    response.Cities = configedCities;
                }

                response.Status = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {

                response.Status = HttpStatusCode.ExpectationFailed;
                logError.Info("续保请求发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }

            return response;
        }

        public List<bx_city> FindList()
        {
            var key = "ExternalApi_City_Find";
            var cachelst = CacheProvider.Get<List<bx_city>>(key);
            if (cachelst == null)
            {
                var lst = _cityRepository.FindList();
                CacheProvider.Set(key, lst, 86400);
                return lst;
            }
            return cachelst;
        }

        public async Task<GetContinedPeriodResponse> GetContinuedPeriod(GetCityContinuedPeriodRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {

            GetContinedPeriodResponse response = new GetContinedPeriodResponse();
            //根据经纪人获取手机号 
            IBxAgent agentModel = GetAgentModelFactory(request.Agent);
            //logInfo.Info("获取到的经纪人信息:"+agentModel.ToJson());
            //参数校验
            if (!agentModel.AgentCanUse())
            {
                response.Status = HttpStatusCode.Forbidden;
                return response;
            }
            if (!agentModel.AgentCanQuote())
            {
                response.Status = HttpStatusCode.Forbidden;
                return response;
            }

            if (!ValidateReqest(pairs, agentModel.SecretKey, request.SecCode))
            {
                response.Status = HttpStatusCode.Forbidden;
                return response;
            }
            try
            {
                var cacheKey = string.Format("continuedperiod_{0}", request.Agent);
                var cache = CacheProvider.Get<List<bx_cityquoteday>>(cacheKey);
                if (cache == null)
                {
                    cache = _dayRepository.GetList(request.Agent);
                    CacheProvider.Set(cacheKey, cache, 600);
                    response.Items = cache;
                }
                else
                {
                    response.Items = cache;
                }
                response.Status = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                response.Status = HttpStatusCode.ExpectationFailed;
                logError.Info("获取续保期列表请求发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return response;

        }

        public List<bx_agent_config> FindConfigCityList(int agent)
        {
            var key = string.Format("ExternalApi_{0}_ConfigCity_Find", agent);
            var cachelst = CacheProvider.Get<List<bx_agent_config>>(key);
            if (cachelst == null)
            {
                var lst = _agentConfigRepository.FindNewCity(agent);
                CacheProvider.Set(key, lst, 10800);
                return lst;
            }
            return cachelst;

        }
    }
}
