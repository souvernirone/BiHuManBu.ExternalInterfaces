using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Models.ReportModel;

namespace BiHuManBu.ExternalInterfaces.Models
{
    public interface IAgentConfigRepository
    {
        List<bx_agent_config> Find(int agentid);
        List<bx_agent_config> FindNewCity(int agentid);
        List<long> FindSource(int agentid);
        List<bx_agent_config> FindBy(int agentid, int citycode);
        List<bx_agent_config> FindCities(int agentid, int cityId);

        /// <summary>
        /// 获取城市集合
        /// </summary>
        /// <param name="agentId">顶级代理人Id</param>
        /// <returns></returns>
        List<int> FindCity(int agentId);

        bx_agent_config FindById(long id);

        List<AgentConfigNameModel> FindListById(string ids);

        List<bx_agent_config> FindByIds(List<long> ids);

        bx_agent_config FindByChannelId(int agentid, long channelId);

        List<MultiChannelsModel> FindConfigSourceList(int agentid, int citycode);

        /// <summary>
        /// 根据代理人获取配置的城市
        /// </summary>
        /// <param name="agentid"></param>
        /// <returns></returns>
        List<int> GetConfigCityList(int agentid);
    }
}
