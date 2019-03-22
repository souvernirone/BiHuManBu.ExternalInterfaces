using System.Collections.Generic;
using System.Net;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.PartialModels.bx_agent;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using log4net;

namespace BiHuManBu.ExternalInterfaces.Services
{
    public class EnumService : CommonBehaviorService,IEnumService
    {
        private IParaBhTypeRepository _paraBhTypeRepository;

        public EnumService(IParaBhTypeRepository paraBhTypeRepository, IAgentRepository agentRepository, ICacheHelper cacheHelper)
            : base(agentRepository, cacheHelper)
        {
            _paraBhTypeRepository = paraBhTypeRepository;
        }

        public GetParaBhTypeResponse GetParaBhType(GetParaBhTypeRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            var response = new GetParaBhTypeResponse();

            //参数校验
            IBxAgent agentModel = GetAgentModelFactory(request.Agent);
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
            var list = _paraBhTypeRepository.GetListByTypeId(request.TypeId, request.IsAll);
            if (list.Count > 0)
            {
                response.Total = list.Count;
                response.ParaBhTypeList = list;
            }
            else
            {
                response.ErrCode = -1;
            }
            return response;
        }
    }
}
