using System;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.PartialModels.bx_agent;
using BiHuManBu.ExternalInterfaces.Services.CommonService.interfaces;

namespace BiHuManBu.ExternalInterfaces.Services.CommonService.implementations
{
    public class AgentPrivilegeService : IAgentPrivilegeService
    {
        private ICacheService _cacheService;
        private IAgentRepository _agentRepository;

        public AgentPrivilegeService(ICacheService cacheService, IAgentRepository agentRepository)
        {
            _cacheService = cacheService;
            _agentRepository = agentRepository;
        }
        public AgentValidResult CanUse(int agentId)
        {
            AgentValidResult result = new AgentValidResult();
            var agent = GetAgentModel(agentId);
            result.Model = agent;
            if (!agent.AgentCanUse())
            {
                result.CheckResult = false;
            }
            else
            {
                result.CheckResult = true; 
            }
            
            return result;
        }

        private IBxAgent GetAgentModel(int agentid)
        {
            string cacheKey = string.Format("agent_key_cache_{0}", agentid);
            IBxAgent cacheValue = _cacheService.Get<bx_agent>(cacheKey);
            if (cacheValue == null)
            {
                cacheValue = _agentRepository.GetAgent(agentid);
                if (cacheValue == null)
                {
                    cacheValue = new NullBxAgent();
                }
                _cacheService.Set(cacheKey, cacheValue, 120);//5分钟
            }
            return cacheValue;
        }
    }

    public class AgentValidResult
    {
        public IBxAgent Model { get; set; }
        public bool CheckResult { get; set; }
    }
}