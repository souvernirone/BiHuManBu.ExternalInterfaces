using BiHuManBu.ExternalInterfaces.Services.Messages.Request;

namespace BiHuManBu.ExternalInterfaces.Services.Interfaces
{

    public interface ICacheManagerService
    {
        /// <summary>
        /// 清空渠道配置缓存
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        int DelAgentSourceCache(GetClearAgentSourceRequest request);

        /// <summary>
        /// 清空城市配置缓存
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        int DelAgentCityCache(GetClearAgentSourceRequest request);

        /// <summary>
        /// 清空经纪人属性缓存
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        int DelAgentPropertiyCache(DelAgentPropertiyCacheRequest request);


    }
}
