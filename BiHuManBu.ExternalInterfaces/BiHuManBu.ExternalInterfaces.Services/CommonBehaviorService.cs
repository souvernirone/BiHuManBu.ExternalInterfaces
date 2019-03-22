using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using BiHuManBu.ExternalInterfaces.Infrastructure;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.PartialModels.bx_agent;
using log4net;
using ServiceStack.Text;

namespace BiHuManBu.ExternalInterfaces.Services
{
    public class CommonBehaviorService
    {
        private ILog logInfo;
        private ILog logError;
        private IAgentRepository _agentRepository;
        private ICacheHelper _cacheHelper;

        public CommonBehaviorService(IAgentRepository agentRepository, ICacheHelper cacheHelper)
        {
            _agentRepository = agentRepository;
            logInfo = LogManager.GetLogger("INFO");
            logError = LogManager.GetLogger("ERROR");
            _cacheHelper = cacheHelper;
        }
        public bx_agent GetAgent(int agentid)
        {
            string CacheKey = string.Format("agent_key_cache_{0}", agentid);
            string CacheKeyWithNull = string.Format("agent_key_cache_null_{0}", agentid);

            var cacheValueWithNull = CacheProvider.Get<string>(CacheKeyWithNull);
            if (cacheValueWithNull == "1")
            {
                return null;
            }
            var cacheValue = CacheProvider.Get<bx_agent>(CacheKey);
            if (cacheValue == null)
            {
                cacheValue = _agentRepository.GetAgent(agentid);

                if (cacheValue == null)
                {

                    CacheProvider.Set(CacheKeyWithNull, 1, 600);//防止缓存渗透
                }
                else
                {
                    CacheProvider.Set(CacheKey, cacheValue, 12000);
                }

            }
            return cacheValue;
        }

        public IBxAgent GetAgentModelFactory(int agentid)
        {
            string cacheKey = string.Format("agent_key_cache_{0}", agentid);
            IBxAgent cacheValue = CacheProvider.Get<bx_agent>(cacheKey);
            if (cacheValue == null)
            {
                cacheValue = _agentRepository.GetAgent(agentid);
                if (cacheValue == null)
                {
                    cacheValue = new NullBxAgent();
                }
                CacheProvider.Set(cacheKey, cacheValue, 120);//5分钟
            }
            return cacheValue;
        }

        public IBxAgent GetCommonAgentModelFactory(int agentid, IAgentRepository agentRepository)
        {
            string cacheKey = string.Format("agent_key_cache_{0}", agentid);
            IBxAgent cacheValue = CacheProvider.Get<bx_agent>(cacheKey);
            if (cacheValue == null)
            {
                cacheValue = agentRepository.GetAgent(agentid);
                if (cacheValue == null)
                {
                    cacheValue = new NullBxAgent();
                }
                CacheProvider.Set(cacheKey, cacheValue, 120);//5分钟
            }
            return cacheValue;
        }

        /// <summary>
        /// 参数校验
        /// </summary>
        /// <param name="list">参数列表</param>
        /// <param name="configKey">配置密钥</param>
        /// <param name="checkCode">输入的校验串</param>
        /// <returns></returns>
        public bool ValidateReqest(IEnumerable<KeyValuePair<string, string>> list, string configKey, string checkCode)
        {
            var checkApi = string.IsNullOrWhiteSpace(System.Configuration.ConfigurationManager.AppSettings["CheckApi"]) ? 0 : int.Parse(System.Configuration.ConfigurationManager.AppSettings["CheckApi"]);
            if (checkApi == 0)
            {
                return true;
            }
            if (!list.Any()) return false;
            if (string.IsNullOrEmpty(configKey)) return false;

            StringBuilder inputParamsString = new StringBuilder();
            foreach (KeyValuePair<string, string> item in list)
            {
                if (item.Key.ToLower() != "seccode")
                {

                    inputParamsString.Append(string.Format("{0}={1}&", item.Key, item.Value));
                }
            }

            var content = inputParamsString.ToString();
            var securityString = (content.Substring(0, content.Length - 1) + configKey).GetMd5();

            return securityString == checkCode;
        }

        public bool AppTokenValidateReqest(string bhToken, int childAgent)
        {
            var checkApp = string.IsNullOrWhiteSpace(System.Configuration.ConfigurationManager.AppSettings["CheckApp"]) ? 0 : int.Parse(System.Configuration.ConfigurationManager.AppSettings["CheckApp"]);
            if (checkApp == 0)
            {
                return true;
            }
            try
            {
                string tokenkey = RsaCryptionHelper.RSADecrypt(bhToken);
                if (string.IsNullOrEmpty(tokenkey))
                {
                    return false;
                }
                if (!tokenkey.Contains("&"))
                {
                    return false;
                }
                string[] strItem = tokenkey.Split('&');
                string itemAgentId = strItem[0].Substring(strItem[0].IndexOf("=") + 1,
                    strItem[0].Length - strItem[0].IndexOf("=") - 1);
                if (!itemAgentId.Equals(childAgent.ToString()))
                {
                    return false;
                }
                string itemDatetime = strItem[1].Substring(strItem[1].IndexOf("=") + 1,
                    strItem[1].Length - strItem[1].IndexOf("=") - 1);
                if (!(DateTime.Parse(itemDatetime) > DateTime.Now) &&
                    DateTime.Parse(itemDatetime).AddDays(30) < DateTime.Now)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                logError.Info("接口BhToken校验异常：BhToken为：" + bhToken + "\n 当前代理人为：" + childAgent + "\n 异常信息:" + ex.StackTrace + " \n " + ex.Message);
                return false;
            }
            return true;
        }

        private static readonly string _mobileSecKey = System.Configuration.ConfigurationManager.AppSettings["MobileSecKey"];
        /// <summary>
        /// 此处校验复制ValidateReqest的方法，App用来校验，代码逻辑一样
        /// app端校验，微信端未做校验
        /// </summary>
        /// <param name="list"></param>
        /// <param name="configKey"></param>
        /// <param name="checkCode"></param>
        /// <returns></returns>
        public bool AppValidateReqest(IEnumerable<KeyValuePair<string, string>> list, string checkCode)
        {
            var checkApp = string.IsNullOrWhiteSpace(System.Configuration.ConfigurationManager.AppSettings["CheckApp"]) ? 0 : int.Parse(System.Configuration.ConfigurationManager.AppSettings["CheckApp"]);
            if (checkApp == 0)
            {
                return true;
            }
            if (!list.Any()) return true;
            if (string.IsNullOrEmpty(_mobileSecKey)) return false;

            var inputParamsString = new StringBuilder();
            foreach (KeyValuePair<string, string> item in list)
            {
                //排除 安卓端无效参数

                if (item.Key.ToLower().Equals("callbackparamskey2") || item.Key.ToLower().Equals("callbackparamskey1") || item.Key.ToLower().Equals("sign") || item.Key.ToLower().Equals("nonce") || item.Key.ToLower().Equals("token") || item.Key.ToLower().Equals("timestamp"))
                {
                }
                else if (item.Key.ToLower() != "seccode")
                {
                    inputParamsString.Append(string.Format("{0}={1}&", item.Key, item.Value));
                }
            }

            var content = inputParamsString.ToString();
            string strsec = content.Substring(0, content.Length - 1);// + _mobileSecKey;
            var securityString = strsec.GetMd5().ToUpper();

            return securityString == checkCode.ToUpper();
        }
        public string GetSecCode(IEnumerable<KeyValuePair<string, string>> list, string configKey)
        {
            StringBuilder inputParamsString = new StringBuilder();
            foreach (KeyValuePair<string, string> item in list)
            {
                if (item.Key.ToLower() != "seccode")
                {

                    inputParamsString.Append(string.Format("{0}={1}&", item.Key, item.Value));
                }
            }

            var content = inputParamsString.ToString();
            var securityString = (content.Substring(0, content.Length - 1) + configKey).GetMd5();

            return securityString;
        }

        public string GetAppSettingCache(string appSettingKey, int duration)
        {
            return "";
        }

        /// <summary>
        /// 参数校验
        /// </summary>
        /// <param name="list">参数列表</param>
        /// <param name="configKey">配置密钥</param>
        /// <param name="checkCode">输入的校验串</param>
        /// <returns></returns>
        public bool ValidatePostReqest(string strUrl, string configKey, string checkCode)
        {
            var checkApi = string.IsNullOrWhiteSpace(System.Configuration.ConfigurationManager.AppSettings["CheckApi"]) ? 0 : int.Parse(System.Configuration.ConfigurationManager.AppSettings["CheckApi"]);
            if (checkApi == 0)
            {
                return true;
            }
            if (string.IsNullOrEmpty(configKey)) return false;
            var content = GetValidateUrl(strUrl);
            var securityString = (content + configKey).GetMd5();
            return securityString == checkCode;
        }
        /// <summary>
        /// 获取要加密的对象模型
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Dictionary<String, Object> GetValidateRequest(object request)
        {
            Dictionary<String, Object> map = new Dictionary<string, object>();
            Type t = request.GetType();
            PropertyInfo[] pi = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo p in pi)
            {
                MethodInfo mi = p.GetGetMethod();
                if (mi != null && mi.IsPublic)
                {
                    map.Add(p.Name, mi.Invoke(request, new Object[] { }));
                }
            }
            return map;
        }

        public static string GetValidateUrl(string url)
        {
            string[] arr = url.Split(new string[] { "&" }, StringSplitOptions.RemoveEmptyEntries);
            Array.Sort(arr);
            return string.Join("&", arr);
        }

    }
}
