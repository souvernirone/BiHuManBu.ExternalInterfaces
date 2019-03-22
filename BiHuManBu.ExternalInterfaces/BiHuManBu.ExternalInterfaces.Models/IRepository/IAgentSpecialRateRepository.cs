using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Models
{
    public interface IAgentSpecialRateRepository
    {
        bx_agent_special_rate GetAgentSpecialRate(int agentid, int source);

        List<bx_agent_special_rate> GeAgentSpecialRates(List<int> agents, int source);
    }
}
