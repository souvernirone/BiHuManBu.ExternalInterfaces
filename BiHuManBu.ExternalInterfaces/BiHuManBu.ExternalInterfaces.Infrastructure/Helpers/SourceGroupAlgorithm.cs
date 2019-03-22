using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using System.Text;

namespace BiHuManBu.ExternalInterfaces.Infrastructure.Helpers
{
    public class SourceGroupAlgorithm
    {
        public static List<int> ListOri = new List<int>();
        public static Dictionary<string, int> LocalDictionary = new Dictionary<string, int>();

        /// <summary>
        /// 保险公司的位标识
        /// 这里在录入时要按照升序录入
        /// </summary>
        private static int[] _sourceFlag = { 1, 2, 4, 8, 16, 4096 };

        /// <summary>
        /// 获取最终的对应sum结果的source集合
        /// </summary>
        /// <param name="oriSource">顶级下的代理资源</param>
        /// <param name="sumValue">计算的结果</param>
        /// <param name="topAgent">顶级代理</param>
        /// <returns></returns>
        public static List<int> Get(List<int> oriSource, int sumValue, int topAgent)
        {
            Dictionary<string, int> dictionary = GetCacheList(oriSource, topAgent);
            if (!dictionary.Any())
            {
                return new List<int>();
            }
            foreach (KeyValuePair<string, int> kv in dictionary)
            {
                if (kv.Value == sumValue)
                {
                    if (string.IsNullOrEmpty(kv.Key))
                    {
                        return new List<int>();
                    }
                    //返回最终的list结果
                    string[] pair = kv.Key.Split(',');
                    List<int> list = new List<int>();
                    for (int i = 0; i < pair.Length; i++)
                    {
                        list.Add(int.Parse(pair[i]));
                    }
                    return list;
                }
            }
            return new List<int>();
        }

        /// <summary>
        /// source加入缓存
        /// </summary>
        /// <param name="oriSource"></param>
        /// <param name="topAgent"></param>
        /// <returns></returns>
        public static Dictionary<string, int> GetCacheList(List<int> oriSource, int topAgent)
        {
            //string sourceCacheKey = string.Format("source_cache_key-{0}", topAgent);
            //Dictionary<string, int> listSource = HttpRuntime.Cache.Get(sourceCacheKey) as Dictionary<string, int>;
            //if (listSource != null)
            //{
            //    return listSource;
            //}
            //else
            //{
            //    var sourceList = Get(oriSource);
            //    if (sourceList.Any())
            //    {
            //        HttpRuntime.Cache.Insert(sourceCacheKey, sourceList, null, DateTime.Now.AddHours(6), TimeSpan.Zero, CacheItemPriority.Normal, null);
            //        return sourceList;
            //    }
            //    return new Dictionary<string, int>();
            //}

            string sourceCacheKey = string.Format("source_cache_key_{0}", topAgent);
            string sourceCacheKeyWithNull = string.Format("source_cache_key_null_{0}", topAgent);

            var cacheValueWithNull = CacheProvider.Get<string>(sourceCacheKeyWithNull);
            if (cacheValueWithNull == "1")
            {
                return null;
            }
            var cacheValue = CacheProvider.Get<Dictionary<string, int>>(sourceCacheKey);
            if (cacheValue == null)
            {
                cacheValue = Get(oriSource);

                if (cacheValue == null)
                {
                    CacheProvider.Set(sourceCacheKeyWithNull, 1, 600);//防止缓存渗透
                }
                else
                {
                    CacheProvider.Set(sourceCacheKey, cacheValue, 12000);
                }

            }
            return cacheValue;

        }

        /// <summary>
        /// 获取source里面所有的排列组合情况 及 结果
        /// </summary>
        /// <param name="oriSource"></param>
        /// <returns></returns>
        public static Dictionary<string, int> Get(List<int> oriSource)
        {
            LocalDictionary.Clear();

            ListOri = oriSource;
            Dictionary<string, int> dic = new Dictionary<string, int>();
            for (int i = 0; i < ListOri.Count; i++)
            {
                LocalDictionary.Add(ListOri[i].ToString(), GetSum(ListOri[i].ToString()));
                dic.Add(ListOri[i].ToString(), i);
            }
            GetString(dic);
            return LocalDictionary;
        }

        static void GetString(Dictionary<string, int> dd)
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            foreach (KeyValuePair<string, int> kv in dd)
            {
                for (int i = kv.Value + 1; i < ListOri.Count; i++)
                {
                    LocalDictionary.Add(kv.Key + "," + ListOri[i], GetSum(kv.Key + "," + ListOri[i]));
                    dic.Add(kv.Key + "," + ListOri[i], i);
                }
            }
            if (dic.Count > 0) GetString(dic);
        }

        public static int GetSum(string str)
        {
            int sum = 0;
            string[] pair = str.Split(',');
            for (int i = 0; i < pair.Length; i++)
            {
                sum += int.Parse(pair[i]);
            }
            return sum;
        }

        /// <summary>
        /// 老Source渠道 换新
        /// </summary>
        /// <param name="oldSource">老的Source</param>
        /// <returns>新的Source</returns>
        public static long GetNewSource(int oldSource)
        {
            switch (oldSource)
            {
                case 0:
                    return 2;
                case 1:
                    return 1;
                case 9999:
                    return oldSource;
            }
            return (long)Math.Pow(2, oldSource);
        }

        /// <summary>
        /// 新Source渠道 换新
        /// </summary>
        /// <param name="newSource">新的Source</param>
        /// <returns>老的Source</returns>
        public static int GetOldSource(long newSource)
        {
            switch (newSource)
            {
                case 1:
                    return 1;
                case 2:
                    return 0;
                case 9999:
                    return (int)newSource;
            }
            return (int)Math.Log(newSource, 2);
        }
        public static long[] GetOldSources(long[] newSources)
        {
            List<long> oldSources = new List<long>();
            foreach (var newSource in newSources)
            {
                switch (newSource)
                {
                    case 1:
                        oldSources.Add(1); break;
                    case 2:
                        oldSources.Add(0); break;
                    case 9999:
                        oldSources.Add((int)newSource); break;
                    default: oldSources.Add((int)Math.Log(newSource, 2)); break;
                }
            }
            return oldSources.ToArray();
        }

        /// <summary>
        /// 获取最终的对应sum结果的source集合
        /// </summary>
        /// <param name="oriSource">顶级下的代理资源</param>
        /// <param name="sumValue">计算的结果</param>
        /// <returns></returns>
        public static List<long> GetSource(List<long> oriSource, int sumValue)
        {
            var sumSource = ParseSource(sumValue);
            if (sumSource.Count == 0)
                return sumSource;

            return sumSource.Where(o => oriSource.Contains(o)).ToList();

        }

        /// <summary>
        /// 找出传入的数是由那几个数相加得到的
        /// 例如：传入6，返回4,2。传入9，返回8,1
        /// 注意：这里传入的值必须是1，2，4，8...等相加的和
        /// </summary>
        /// <param name="sumValue">source的和</param>
        /// <returns></returns>
        public static List<long> ParseSource(int sumValue)
        {
            var result = new List<long>();
            if (sumValue == 0)
                return result;

            foreach (var item in _sourceFlag)
            {
                if (item > sumValue)
                    break;
                if ((sumValue & item) == item)
                {
                    result.Add(item);
                }
            }

            return result;
        }
        /// <summary>
        /// 找出传入的数是由那几个数相加得到的
        /// 例如：传入6，返回4,2对应的老source：0,2。传入9，返回8,1对应的老source：3,1
        /// 注意：这里传入的值必须是1，2，4，8...等相加的和
        /// </summary>
        /// <param name="sumValue">source的和</param>
        /// <returns></returns>
        public static List<int> ParseOldSource(int sumValue)
        {
            var result = new List<int>();
            if (sumValue == 0)
                return result;
            foreach (var item in _sourceFlag)
            {
                if (item > sumValue)
                    break;
                if ((sumValue & item) == item)
                {
                    result.Add(GetOldSource(item));
                }
            }
            return result;
        }

        /// <summary>
        /// 找出传入的数是由那几个数相加得到的
        /// 例如：传入6，返回4,2对应的老source：0,2。传入9，返回8,1对应的老source：3,1
        /// 注意：这里传入的值必须是1，2，4，8...等相加的和
        /// </summary>
        /// <param name="sumValue">source的和</param>
        /// <returns></returns>
        public static List<int> ParseOldSource(long sumValue)
        {
            var result = new List<int>();
            if (sumValue == 0)
                return result;
            foreach (var item in _sourceFlag)
            {
                if (item > sumValue)
                    break;
                if ((sumValue & item) == item)
                {
                    result.Add(GetOldSource(item));
                }
            }
            return result;
        }

        /// <summary>
        /// 找出传入的数是由那几个数相加得到的
        /// 例如：传入6，返回4,2。传入9，返回8,1
        /// 注意：这里传入的值必须是1，2，4，8...等相加的和
        /// </summary>
        /// <param name="sumValue">source的和</param>
        /// <returns></returns>
        public static List<long> ParseNewSource(long sumValue)
        {
            var result = new List<long>();
            if (sumValue == 0)
                return result;

            foreach (var item in _sourceFlag)
            {
                if (item > sumValue)
                    break;
                if ((sumValue & item) == item)
                {
                    result.Add(item);
                }
            }

            return result;
        }
    }
}
