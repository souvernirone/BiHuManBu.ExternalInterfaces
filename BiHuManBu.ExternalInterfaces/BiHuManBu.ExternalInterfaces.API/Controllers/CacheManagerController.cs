using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using log4net;
using System.Web.Http;
using ServiceStack.Text;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;

namespace BiHuManBu.ExternalInterfaces.API.Controllers
{
    public class CacheManagerController : ApiController
    {
        private readonly ICacheManagerService _cacheManagerService;
        private readonly ILog _logInfo = LogManager.GetLogger("INFO");

        public CacheManagerController(ICacheManagerService cacheManagerService)
        {
            _cacheManagerService = cacheManagerService;
        }

        /// <summary>
        /// 清空渠道配置缓存
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public  int  DelAgentSourceCache([FromUri]GetClearAgentSourceRequest request)
        {
            _logInfo.Info("清空渠道配置缓存请求：" + request.ToJson());
            return _cacheManagerService.DelAgentSourceCache(request);
        }

        /// <summary>
        /// 清空城市配置缓存
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public int DelAgentCityCache([FromUri]GetClearAgentSourceRequest request)
        {
            _logInfo.Info("清空城市配置缓存请求：" + request.ToJson());
            return _cacheManagerService.DelAgentCityCache(request);
        }

        /// <summary>
        /// 清空经纪人属性缓存
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public int DelAgentPropertiyCache([FromUri]DelAgentPropertiyCacheRequest request)
        {
            _logInfo.Info("清空经纪人属性缓存请求：" + request.ToJson());
            return _cacheManagerService.DelAgentPropertiyCache(request);
        }


        /// <summary>
        /// 指定代理人Id删除
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        [HttpGet]
        public BaseViewModel ResetAgent(int agent)
        {
            string agentKey = "agent_key_cache_" + agent;
            CacheProvider.Remove(agentKey);
            return new BaseViewModel
            {
                BusinessStatus = 1,
                StatusMessage = "清理成功"
            };
        }
        /// <summary>
        /// 批量删除代理人缓存Id
        /// </summary>
        /// <param name="fromid"></param>
        /// <param name="toid"></param>
        /// <returns></returns>
        [HttpGet]
        public BaseViewModel ResetMoreAgent(int fromid, int toid)
        {
            string agentKey = string.Empty;
            for (int i = fromid; i <= toid; i++)
            {
                agentKey = "agent_key_cache_" + i;
                CacheProvider.Remove(agentKey);
            }
            return new BaseViewModel
            {
                BusinessStatus = 1,
                StatusMessage = "清理成功"
            };
        }
        /// <summary>
        /// 自定义删除缓存Key
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        [HttpGet]
        public BaseViewModel ResetCustom(string cacheKey)
        {
            if (!string.IsNullOrWhiteSpace(cacheKey))
            {
                CacheProvider.Remove(cacheKey);
            }
            return new BaseViewModel
            {
                BusinessStatus = 1,
                StatusMessage = "清理成功"
            };
        }

    }
}
