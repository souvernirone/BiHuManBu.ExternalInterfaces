using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;

namespace BiHuManBu.ExternalInterfaces.Services.AgentChannelService.Interfaces
{
    public interface IChannelModelMapRedisService
    {
        List<CacheChannelModel> GetCacheChannelList(List<bx_agent_config> configs);
        List<AgentCacheChannelModel> GetAgentCacheChannelList(List<bx_agent_config> configs);
        /// <summary>
        /// 取单独一条渠道记录的状态
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        AgentCacheChannelModel GetAgentCacheChannel(string url);
    }
}
