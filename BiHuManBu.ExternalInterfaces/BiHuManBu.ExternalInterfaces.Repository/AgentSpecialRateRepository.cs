using System;
using System.Collections.Generic;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Models;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class AgentSpecialRateRepository : IAgentSpecialRateRepository
    {
        public bx_agent_special_rate GetAgentSpecialRate(int agentid, int source)
        {
            throw new NotImplementedException();
        }

        public List<bx_agent_special_rate> GeAgentSpecialRates(List<int> agents, int source)
        {
            if (agents.Count == 0)
            {
                return null;
            }

            var result =
                DataContextFactory.GetDataContext().bx_agent_special_rate.Where(x => agents.Contains(x.agent_id.Value) && x.company_id == source)
                    .ToList();
            return result;
        }

    }
}
