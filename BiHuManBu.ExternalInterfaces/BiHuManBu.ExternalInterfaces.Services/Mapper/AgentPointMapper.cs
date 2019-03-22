using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.Mapper
{
    public static class AgentPointMapper
    {
        public static AgentPoint ConverToViewModel(this bx_agentpoint agentpoint)
        {
            AgentPoint vm = new AgentPoint();
            if (agentpoint != null)
                vm = new AgentPoint
                {
                    id = agentpoint.id,
                    AgentId = agentpoint.AgentId,
                    Address = agentpoint.Address,
                    Phone = agentpoint.Phone,
                    create_time = agentpoint.create_time
                };
            return vm;
        }
    }
}
