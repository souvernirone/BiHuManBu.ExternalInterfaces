using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.AgentConfigService.Interfaces
{
    public interface IAgentConfigByCityService
    {
        List<bx_agent_config> GetAgentConfigByCity(int agentId, int cityCode);

        long GetUserId(long childAgent);

        List<long> GetAgentSelectChannelidByBuid(int agent,int topagent);

        List<bx_agent_config> GetAgentConfigByChannelid(List<long> channeIds);

        int GetAgentCityCodeByChannelId(int agentId, long channelId);
        List<MultiChannels> GetAgentConfigGroupBySource(int agentId, int cityCode, List<int> quoteSourceGroup);

        /// <summary>
        /// 根据代理人获取城市列表
        /// </summary>
        /// <param name="agentId"></param>
        /// <returns></returns>
        List<int> GetConfigCityList(int agentId);
    }
}
