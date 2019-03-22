using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.AgentChannelService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;

namespace BiHuManBu.ExternalInterfaces.Services.AgentChannelService.Implementations
{
    public class ChannelModelMapRedisService : IChannelModelMapRedisService
    {
        private IHashOperator _hashOperator;

        /// <summary>
        /// 总的渠道状态哈希Key
        /// </summary>
        private readonly string _hashKey = "BiHu.BaoXian.MappingTableManage";
        public ChannelModelMapRedisService(IHashOperator hashOperator)
        {
            _hashOperator = hashOperator;
        }

        public List<CacheChannelModel> GetCacheChannelList(List<bx_agent_config> configs)
        {
            var list = new List<CacheChannelModel>();
            CacheChannelModel model;
            //取哈希值的单条key，是配置表里的url作为key来取值的
            string redisUrlKey = string.Empty;
            foreach (var item in configs)
            {
                //获取渠道配置url
                redisUrlKey = item.isurl == 1 ? item.bx_url : item.macurl;
                //获取单个渠道状态的模型
                model = new CacheChannelModel();
                if (!string.IsNullOrEmpty(redisUrlKey))
                {
                    model = _hashOperator.Get<CacheChannelModel>(_hashKey, redisUrlKey);
                    if (model != null)
                    {
                        list.Add(model);
                    }
                }
            }
            return list;
        }

        public List<AgentCacheChannelModel> GetAgentCacheChannelList(List<bx_agent_config> configs)
        {
            var list = new List<AgentCacheChannelModel>();
            AgentCacheChannelModel model;
            //取哈希值的单条key，是配置表里的url作为key来取值的
            string redisUrlKey = string.Empty;
            foreach (var item in configs)
            {
                //获取渠道配置url
                redisUrlKey = item.isurl == 1 ? item.bx_url : item.macurl;
                //获取单个渠道状态的模型
                model = new AgentCacheChannelModel();
                if (!string.IsNullOrEmpty(redisUrlKey))
                {
                    model = _hashOperator.Get<AgentCacheChannelModel>(_hashKey, redisUrlKey);
                    if (model != null)
                    {
                        model.ChannelId = item.id;
                        model.ChannelName = item.config_name;
                        model.IsPaicApi = item.is_paic_api;
                        list.Add(model);
                    }
                }

            }
            return list;
        }

        /// <summary>
        /// 获取渠道配置url作为缓存key来查询。判断ukey表的isurl==1?bx_url:macurl
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public AgentCacheChannelModel GetAgentCacheChannel(string url)
        {
            AgentCacheChannelModel model = new AgentCacheChannelModel(); ;
            //取哈希值的单条key，是配置表里的url作为key来取值的
            if (!string.IsNullOrEmpty(url))
            {
                model = _hashOperator.Get<AgentCacheChannelModel>(_hashKey, url);
                return model;
            }
            return null;
        }
    }
}
