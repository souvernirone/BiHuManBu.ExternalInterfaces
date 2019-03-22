using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.IRepository;
using BiHuManBu.ExternalInterfaces.Services.ConfigService.Interfaces;

namespace BiHuManBu.ExternalInterfaces.Services.ConfigService.Implementations
{
    public class GetConfigValueService : IGetConfigValueService
    {
        private readonly IConfigRepository _configRepository;
        public GetConfigValueService(IConfigRepository configRepository)
        {
            _configRepository = configRepository;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheKey">缓存key</param>
        /// <param name="configType">bx_config对应类型</param>
        /// <returns></returns>
        public string GetConfigValue(string configKey, int configType, string cacheKey)
        {
            string strValue = string.Empty;
            //取缓存值
            strValue = CacheProvider.Get<string>(cacheKey);
            if (string.IsNullOrEmpty(strValue))
            {
                bx_config config = _configRepository.Find(configKey, configType);
                if (config != null)
                {
                    strValue = config.config_value ?? "";
                    CacheProvider.Set(cacheKey, strValue, 21600);
                }
            }
            return strValue;
        }

    }
}
