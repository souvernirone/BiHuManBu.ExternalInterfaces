using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;

namespace BiHuManBu.ExternalInterfaces.Services.AgentChannelService.Interfaces
{
    public interface IChannelStatusByUkeyIdService
    {
        GetSingelChannelStatusResponse GetChannelStatusByUkeyId(GetChannelStatusByUkeyIdRequest request, IEnumerable<KeyValuePair<string, string>> pairs);
    }
}
