using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Models.PartialModels.bx_agent;

namespace BiHuManBu.ExternalInterfaces.Services.ValidateService.Interfaces
{
    public interface IGetAgentInfoService
    {
        IBxAgent GetAgentModelFactory(int agentid);
    }
}
