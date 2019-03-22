using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Models
{
    public interface IAgentPointRepository
    {
        List<bx_agentpoint> Find(int agentId);
        bx_agentpoint FindById(int agentpointId);
        bx_agentpoint FindWorkAgentpoint(int agentId);
    }
}
