using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using log4net;
using System;

namespace BiHuManBu.ExternalInterfaces.Services
{
    public class CacheManagerService : ICacheManagerService
    {
        private ILog _logInfo;
        private ILog _logError;

        public CacheManagerService()
        {
            _logInfo = LogManager.GetLogger("INFO");
            _logError = LogManager.GetLogger("ERROR");
        }

        public int DelAgentCityCache(GetClearAgentSourceRequest request)
        {
            try
            {
                _logInfo.Info(string.Format("经纪人agent：{0}清空了城市配置缓存", request.Agent));
                var key = string.Format("ExternalApi_{0}_ConfigCity_Find", request.Agent);
                CacheProvider.Remove(key);
                var keyAllCity = "ExternalApi_City_Find";//获取所有城市配置表bx_city
                CacheProvider.Remove(keyAllCity);
                return 1;
            }
            catch 
            {
                _logError.Info(string.Format("经纪人agent：{0}清空了城市配置缓存，发生异常", request.Agent));
                return 0;
            }
        }

        public int DelAgentPropertiyCache(DelAgentPropertiyCacheRequest request)
        {
            try
            {
                _logInfo.Info(string.Format("经纪人agent：{0}清空了经纪人属性缓存", request.Agent));
                var key = string.Format("agent_key_cache_{0}", request.Agent);
                CacheProvider.Remove(key);
                return 1;
            }
            catch (Exception ex)
            {
                _logError.Info(string.Format("经纪人agent：{0}清空经纪人属性缓存，发生异常", request.Agent));
                return 0;
            }
        }

        public int DelAgentSourceCache(GetClearAgentSourceRequest request)
        {
            try
            {
                _logInfo.Info(string.Format("经纪人agent：{0}清空了资源配置缓存", request.Agent));
                var key = string.Format("agent_source_key_{0}_{1}", request.Agent, request.City);
                CacheProvider.Remove(key);
                var keyAllCity = string.Format("agent_source_key_{0}", request.Agent);//App和Crm用到了此缓存，获取某代理下所有城市的渠道
                CacheProvider.Remove(keyAllCity);
                //报价渠道状态用到此缓存,获取某代理下某个城市的渠道
                var keyAgentCity = string.Format("agent_config_{0}_{1}_list", request.Agent, request.City);
                CacheProvider.Remove(keyAgentCity);
                var keyGroupbyList = string.Format("multiChannels_{0}_{1}_list", request.Agent, request.City);
                CacheProvider.Remove(keyGroupbyList);
                var keyByAgent = string.Format("ExternalApi_ConfigCity_FindByAgent_{0}", request.Agent);
                CacheProvider.Remove(keyByAgent);
                return 1;
            }
            catch
            {
                _logError.Info(string.Format("经纪人agent：{0},城市{1}清空了资源配置缓存，发生异常", request.Agent, request.City));
                return 0;
            }
        }
    }
}
