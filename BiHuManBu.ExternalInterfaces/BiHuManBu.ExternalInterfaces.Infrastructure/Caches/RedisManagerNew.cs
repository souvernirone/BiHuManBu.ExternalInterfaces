using System.Linq;
using BiHuManBu.Redis;
using ServiceStack.Redis;

namespace BiHuManBu.ExternalInterfaces.Infrastructure.Caches
{
    public class RedisManagerNew
    {
        ///// <summary>
        ///// redis配置文件信息
        ///// </summary>
        //private static RedisConfig RedisConfig = RedisConfig.GetConfig();

        //private static PooledRedisClientManager prcm;

        ///// <summary>
        ///// 静态构造方法，初始化链接池管理对象
        ///// </summary>
        //static RedisManagerNew()
        //{
        //    CreateManager();
        //}

        ///// <summary>
        ///// 创建链接池管理对象
        ///// </summary>
        //private static void CreateManager(long?db=0)
        //{
        //    string[] WriteServerConStr = SplitString(RedisConfig.WriteServerList, ",");
        //    string[] ReadServerConStr = SplitString(RedisConfig.ReadServerList, ",");
        //    prcm = new PooledRedisClientManager(ReadServerConStr, WriteServerConStr,
        //                     new RedisClientManagerConfig
        //                     {
        //                         MaxWritePoolSize = RedisConfig.MaxWritePoolSize,
        //                         MaxReadPoolSize = RedisConfig.MaxReadPoolSize,
        //                         AutoStart = RedisConfig.AutoStart,
        //                         DefaultDb = db
        //                     });
        //}

        //private static string[] SplitString(string strSource, string split)
        //{
        //    return strSource.Split(split.ToArray());
        //}
        ///// <summary>
        ///// 客户端缓存操作对象
        ///// </summary>
        //public static IRedisClient GetClient(long? db = 0)
        //{
        //    if (prcm == null)
        //        CreateManager(db);
        //    return prcm.GetClient();
        //}
    }
}
