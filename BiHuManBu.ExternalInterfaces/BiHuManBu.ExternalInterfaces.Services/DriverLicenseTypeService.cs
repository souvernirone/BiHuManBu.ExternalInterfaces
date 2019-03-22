using System.Collections.Generic;
using System.Net;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.IRepository;
using BiHuManBu.ExternalInterfaces.Models.PartialModels.bx_agent;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;

namespace BiHuManBu.ExternalInterfaces.Services
{
    public class DriverLicenseTypeService : CommonBehaviorService, IDriverLicenseTypeService
    {
        private readonly IDriverLicenseTypeRepository _typeRepository;
        private IAgentRepository _agentRepository;
        private ICacheHelper _cacheHelper;
        public DriverLicenseTypeService(IDriverLicenseTypeRepository typeRepository,IAgentRepository agentRepository,ICacheHelper cacheHelper)
            : base(agentRepository, cacheHelper)
        {
            _typeRepository = typeRepository;
            _agentRepository = agentRepository;
            _cacheHelper = cacheHelper;
        }
        public DriverLicenseTypeResponse GetList(GetDriverLicenseCarTypeRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            DriverLicenseTypeResponse response = new DriverLicenseTypeResponse();

            //根据经纪人获取手机号 
            IBxAgent agentModel = GetAgentModelFactory(request.Agent);
            if (!agentModel.AgentCanUse())
            {
                response.Status = HttpStatusCode.Forbidden;
                return response;
            }
            //参数校验
            if (!ValidateReqest(pairs, agentModel.SecretKey, request.SecCode))
            {
                response.Status = HttpStatusCode.Forbidden;
                return response;
            }
            var listskey = string.Format("gsc_driverlicense_cartype_key");
            var list = CacheProvider.Get<List<bx_drivelicense_cartype>>(listskey);
            if (list == null)
            {
                list = _typeRepository.FindList();
                CacheProvider.Set(listskey, list, 86400);
            }
            response.List = list;

            return response;
        }
    }
}
