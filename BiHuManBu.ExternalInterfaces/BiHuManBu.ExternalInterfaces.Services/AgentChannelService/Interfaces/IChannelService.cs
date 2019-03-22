using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;

namespace BiHuManBu.ExternalInterfaces.Services.AgentChannelService.Interfaces
{
    public interface IChannelService
    {
        /// <summary>
        /// 获取渠道状态
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        GetChannelStatusResponse GetChannelStatus(GetChannelStatusRequest request, IEnumerable<KeyValuePair<string, string>> pairs);
    }
}
