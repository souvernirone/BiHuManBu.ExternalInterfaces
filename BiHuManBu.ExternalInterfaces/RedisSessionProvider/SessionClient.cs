using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.Redis;
using ServiceStack.Redis;
using ServiceStack.Text;
using StackExchange.Redis;

namespace RedisSessionProvider
{
    public  class SessionClient<T> where T:class,new()
    {
        #region 设置key
        public void SessionSet(string key, T value, TimeSpan expireIn)
        {
            Func<IRedisClient, bool> func = (IRedisClient client) =>
            {
                if (expireIn == TimeSpan.MinValue)
                {
                    client.SetEntry(key, value.ToJson());
                }
                else
                {
                    client.SetEntry(key,value.ToJson(),expireIn);
                }
                return false;
            };

             TryRedisWrite(func);
        }


        public void SessionExpireIn(string key, TimeSpan expiresTime)
        {
            Func<IRedisClient, bool> func = (IRedisClient client) =>
            {
                client.ExpireEntryIn(key, expiresTime);
                return false;
            };

            TryRedisWrite(func);
        }

        public bool SessionRemove(string key)
        {
            Func<IRedisClient, bool> func = (IRedisClient client) =>
            {
                return client.Remove(key);
            };
            return TryRedisWrite(func);
        }

        #endregion

        #region 读取

        public bool SessionHasExists(string key)
        {
            Func<IRedisClient, bool> func = (IRedisClient client) =>
            {
                var val = client.GetValue(key);
                return !string.IsNullOrWhiteSpace(val);
            };

            return TryRedisRead(func);
        }

        public T SessionGet(string key)
        {
            Func<IRedisClient, T> func = (IRedisClient client) =>
            {
                var val = client.GetValue(key);
                if (!string.IsNullOrWhiteSpace(val))
                {
                    return val.FromJson<T>();
                }
                else
                {
                    return default(T);
                }
            };
            return TryRedisRead(func);
        }

        public IList<string> SessionSearch(string pattern)
        {
            Func<IRedisClient, IList<string>> func = (IRedisClient client) => client.SearchKeys(pattern);
            return TryRedisRead(func);
        }
        /// <summary>
        /// 返回根据条件查找到的value对象列表        
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public IList<T> SearchValues(string pattern)
        {
            Func<IRedisClient, IList<T>> fun = (IRedisClient client) =>
            {
                IList<string> keys = new List<string>();

                //先查找KEY
                keys = client.SearchKeys(pattern);

                if (keys != null && keys.Count > 0)
                {
                    //再直接根据key返回对象列表
                    Dictionary<string, T> dics = (Dictionary<string, T>)client.GetAll<T>(keys);

                    return dics.Values.ToList<T>();
                }
                else
                {
                    return new List<T>();
                }

            };

            return TryRedisRead(fun);
        }
        #endregion

        #region 通用
        public F TryRedisRead<F>(Func<IRedisClient, F> doRead)
        {
            IRedisClient client = null;
            try
            {
                //using (client = RedisManager.GetClient())
                //{
                    return doRead(client);
                //}
            }
            catch (RedisException ex)
            {
                return default(F);
            }
            finally
            {
                if (client != null)
                {
                    client.Dispose();
                }
            }
        }

        public F TryRedisWrite<F>(Func<IRedisClient, F> doWrite)
        {
            IRedisClient client = null;
            try
            {
                //using (client = RedisManager.GetClient())
                //{
                    return doWrite(client);
                //}
            }
            catch (RedisException exception)
            {
                throw new Exception("Redis写入异常.");
            }
            finally
            {
                if (client != null)
                {
                    client.Dispose();
                }
            }
        }
        #endregion
    }
}
