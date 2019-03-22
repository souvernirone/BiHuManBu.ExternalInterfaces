using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.GetAgentConfigByCityService.Interfaces
{
    public interface IGetAgentConfigByCitysService
    {
        ResponseMultiQuotedChannelsViewModel GetAgentConfigByCityResponse(int agentId, int cityCode);

        ResponseMultiQuotedChannelsViewModel GetLastQuotedResponse(GetLastQuotedRequest request);
    }
}
