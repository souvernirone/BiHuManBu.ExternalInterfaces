using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Models;

namespace BiHuManBu.ExternalInterfaces.Services.Interfaces
{
    public interface IAgentPointService
    {
        List<bx_agentpoint> GetAgentpoints(int agentId);
        bx_agentpoint GetAgentpoint(int agentpointId);
        bx_agentpoint GetWorkAgentpoint(int agentId);
    }
}
