using System.Collections.Generic;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Models;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class AgentRateRepository:IAgentRateRepository
    {
        public List<bx_agent_rate> GetAgentRates(List<int> agents, int source)
        {
            if (agents.Count == 0) return null;
            var result =
               DataContextFactory.GetDataContext().bx_agent_rate.Where(x=>agents.Contains(x.agent_id)&&x.company_id==source)
                   .ToList();
            return result;
        }

        public List<bx_agent_rate> GetAgentRates(int agent, int source)
        {
            return DataContextFactory.GetDataContext().bx_agent_rate.Where(x => x.agent_id == agent && x.company_id == source).ToList();
        }

        public List<int> GetAgentOfZhiKeRate(int topAgent)
        {
            return new List<int>();
        } 

    }
}
