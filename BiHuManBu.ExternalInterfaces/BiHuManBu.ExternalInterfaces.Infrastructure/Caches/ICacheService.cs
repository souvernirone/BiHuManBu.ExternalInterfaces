using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Infrastructure.Caches
{
    public  interface ICacheService
    {
        bool Add(string strKey, object objValue, bool isSetExpireTime = true, int cacheType = 1);
        bool Add(string strKey, object objValue, long lNumofSeconds, int cacheType = 1);
        bool Set<T>(string strKey, T objValue, bool isSetExpireTime = true, int cacheType = 1);
        bool Set(string strKey, object objValue, long lNumofSeconds, int cacheType = 1);
        T Get<T>(string strKey, int cacheType = 1);

        object Remove(string strKey, int cacheType = 1);
    }
}
