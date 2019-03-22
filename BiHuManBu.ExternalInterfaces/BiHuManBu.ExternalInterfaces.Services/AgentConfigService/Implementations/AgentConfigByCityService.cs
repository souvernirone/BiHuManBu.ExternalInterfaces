using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.IRepository;
using BiHuManBu.ExternalInterfaces.Services.AgentConfigService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using ServiceStack.Text;

namespace BiHuManBu.ExternalInterfaces.Services.AgentConfigService.Implementations
{
    public class AgentConfigByCityService : IAgentConfigByCityService
    {
        private readonly IAgentConfigRepository _agentConfigRepository;
        private readonly IUserInfoRepository _userInfoRepository;
        private readonly IAgentSelectRepository _agentSelectRepository;

        public AgentConfigByCityService(IAgentConfigRepository agentConfigRepository, IUserInfoRepository userInfoRepository, IAgentSelectRepository agentSelectRepository)
        {
            _agentConfigRepository = agentConfigRepository;
            _userInfoRepository = userInfoRepository;
            _agentSelectRepository = agentSelectRepository;
        }

        public List<bx_agent_config> GetAgentConfigByCity(int agentId, int cityCode)
        {
            List<bx_agent_config> configModel = new List<bx_agent_config>();
            var key = string.Format("agent_config_{0}_{1}_list", agentId, cityCode);
            var cachelst = CacheProvider.Get<List<bx_agent_config>>(key);
            if (cachelst == null)
            {
                var lst = _agentConfigRepository.FindBy(agentId, cityCode);
                if (lst.Any())
                {
                    CacheProvider.Set(key, lst, 10800);
                    configModel = lst;
                }
            }
            else
            {
                configModel = cachelst;
            }
            return configModel;
        }

        public int GetAgentCityCodeByChannelId(int agentId, long channelId)
        {
            var model = _agentConfigRepository.FindByChannelId(agentId, channelId);
            return model == null ? 0 : model.city_id.HasValue ? model.city_id.Value : 0;
        }

        public long GetUserId(long childAgent)
        {
            return _userInfoRepository.FindUserIdByAgentId(childAgent);
        }

        public List<long> GetAgentSelectChannelidByBuid(int agent, int topagent)
        {
            return _agentSelectRepository.GetMulti(agent, topagent);
        }

        public List<bx_agent_config> GetAgentConfigByChannelid(List<long> channeIds)
        {
            return _agentConfigRepository.FindByIds(channeIds);
        }

        public List<MultiChannels> GetAgentConfigGroupBySource(int agentId, int cityCode, List<int> quoteSourceGroup)
        {
            List<MultiChannels> configModel = new List<MultiChannels>();
            //取缓存
            var key = string.Format("multiChannels_{0}_{1}_list", agentId, cityCode);
            var cachelst = CacheProvider.Get<List<MultiChannels>>(key);
            if (cachelst == null)
            {
                //取库
                var lst = _agentConfigRepository.FindConfigSourceList(agentId, cityCode);
                if (lst.Any())
                {
                    foreach (var l in lst)
                    {
                        if (l.Source.HasValue && l.ChannelId != 0)
                        {
                            MultiChannels a = new MultiChannels();
                            a.ChannelId = l.ChannelId;
                            a.Source = l.Source.Value;
                            configModel.Add(a);
                        }
                    }
                    CacheProvider.Set(key, configModel, 3600);
                }
            }
            else
            {
                configModel = cachelst;
            }
            //从缓存里的值取报了哪些保司
            if (configModel.Any())
            {
                configModel = configModel.Where(l => quoteSourceGroup.Contains((Int32)l.Source)).ToList();
            }
            return configModel;
        }

        public List<int> GetConfigCityList(int agentId)
        {
            var agentCityKey = string.Format("agent_city_key_{0}", agentId);
            var listCity = CacheProvider.Get<List<int>>(agentCityKey);
            if (listCity == null)
            {
                listCity = _agentConfigRepository.GetConfigCityList(agentId).ToList();
                CacheProvider.Set(agentCityKey, listCity, 10800);
            }
            return listCity;
        }
    }
}
