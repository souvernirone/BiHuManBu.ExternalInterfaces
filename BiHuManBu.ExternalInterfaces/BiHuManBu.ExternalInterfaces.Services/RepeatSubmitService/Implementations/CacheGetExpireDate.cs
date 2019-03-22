using System;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Infrastructure.CacheKeyFactory;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.RepeatSubmitService.interfaces;

namespace BiHuManBu.ExternalInterfaces.Services.RepeatSubmitService.Implementations
{
    public class CacheGetExpireDate : IGetExpireDate
    {
        private readonly ICacheService _cacheService;

        public CacheGetExpireDate(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public async Task<bx_lastinfo> GetDate(string identity)
        {
            var expireDataCacheKey = string.Format("{0}-lastinfo-repeat-key", identity);
            var expireDataCacheValue = string.Format("{0}-lastinfo-repeat", identity);
            var cacheKey = _cacheService.Get<string>(expireDataCacheKey);
            if (cacheKey == null)
            {
                for (var i = 0; i < 250; i++)
                {
                    cacheKey = _cacheService.Get<string>(expireDataCacheKey);
                    if (!string.IsNullOrWhiteSpace(cacheKey))
                    {
                        if (cacheKey == "0" || cacheKey == "1") //0:没有发生重复投保  1： 发生重复投保 
                            break;
                    }
                    else
                    {
                        await Task.Delay(TimeSpan.FromSeconds(1));
                    }
                }
            }
            if (cacheKey == null)
            {
                throw new Exception("超时");
            }
            if (cacheKey == "1")
            {
                var lastinfoRecord = CacheProvider.Get<bx_lastinfo>(expireDataCacheValue);
                return lastinfoRecord;
            }
            if (cacheKey == "0")
            {
                return null;
            }
            return null;
        }
    }
}