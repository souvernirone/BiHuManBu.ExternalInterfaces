using BiHuManBu.ExternalInterfaces.Infrastructure;
using BiHuManBu.ExternalInterfaces.Services.ValidateService.Interfaces;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace BiHuManBu.ExternalInterfaces.Services.ValidateService.Achieves
{
    public class CheckGetSecCode : ICheckGetSecCode
    {
        private readonly string _checkApi = ConfigurationManager.AppSettings["CheckApi"];
        /// <summary>
        /// 参数校验
        /// </summary>
        /// <param name="list">参数列表</param>
        /// <param name="configKey">配置密钥</param>
        /// <param name="checkCode">输入的校验串</param>
        /// <returns></returns>
        public bool ValidateReqest(IEnumerable<KeyValuePair<string, string>> list, string configKey, string checkCode)
        {
            var checkApi = string.IsNullOrWhiteSpace(_checkApi) ? 0 : int.Parse(_checkApi);
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
    }
}
