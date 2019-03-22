using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Models
{
     public interface IAgentRateRepository
    {
         List<bx_agent_rate> GetAgentRates(List<int> agents,int source);
         List<bx_agent_rate> GetAgentRates(int agent, int source); 
    }
}
