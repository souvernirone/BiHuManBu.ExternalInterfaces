using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.PartialModels.bx_agent;
using BiHuManBu.ExternalInterfaces.Services.ValidateService.Interfaces;

namespace BiHuManBu.ExternalInterfaces.Services.ValidateService.Achieves
{
    public class GetAgentInfoService : IGetAgentInfoService
    {
        private IAgentRepository _agentRepository;

        public GetAgentInfoService(IAgentRepository agentRepository)
        {
            _agentRepository = agentRepository;
        }

        public IBxAgent GetAgentModelFactory(int agentid)
        {
            string cacheKey = string.Format("agent_key_cache_{0}", agentid);
            IBxAgent cacheValue = CacheProvider.Get<bx_agent>(cacheKey);
            if (cacheValue == null)
            {
                cacheValue = _agentRepository.GetAgent(agentid);
                if (cacheValue == null)
                {
                    cacheValue = new NullBxAgent();
                }
                CacheProvider.Set(cacheKey, cacheValue, 120);//5分钟
            }
            return cacheValue;
        }
    }
}
