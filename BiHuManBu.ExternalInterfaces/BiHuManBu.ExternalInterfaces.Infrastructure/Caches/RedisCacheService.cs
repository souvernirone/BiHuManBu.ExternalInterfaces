using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.Redis;

namespace BiHuManBu.ExternalInterfaces.Infrastructure.Caches
{
    public class RedisCacheService:ICacheService
    {
        public bool Add(string strKey, object objValue, bool isSetExpireTime = true, int cacheType = 1)
        {
            return RedisManager.Add<object>(strKey, objValue, isSetExpireTime);
        }

        public bool Add(string strKey, object objValue, long lNumofSeconds, int cacheType = 1)
        {
            return RedisManager.Add<object>(strKey, objValue, lNumofSeconds);
        }

        public bool Set<T>(string strKey, T objValue, bool isSetExpireTime = true, int cacheType = 1)
        {
            return RedisManager.Set<T>(strKey, objValue, isSetExpireTime);
        }

        public bool Set(string strKey, object objValue, long lNumofSeconds, int cacheType = 1)
        {
            return RedisManager.Set<object>(strKey, objValue, lNumofSeconds);
        }

        public T Get<T>(string strKey, int cacheType = 1)
        {
            return RedisManager.Get<T>(strKey);
        }

        public object Remove(string strKey, int cacheType = 1)
        {
            return RedisManager.Remove(strKey);
        }
    }
}
