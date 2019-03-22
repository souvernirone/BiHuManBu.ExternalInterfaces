using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using BiHuManBu.ExternalInterfaces.Infrastructure;
using BiHuManBu.ExternalInterfaces.Services.CommonService.interfaces;

namespace BiHuManBu.ExternalInterfaces.Services.CommonService.implementations
{
    public class HttpGetRequestValidService : IRequestValidService
    {
        public bool ValidateReqest(IEnumerable<KeyValuePair<string, string>> list, string configKey, string checkCode)
        {
            var checkApi = string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["CheckApi"])
                ? 0
                : int.Parse(ConfigurationManager.AppSettings["CheckApi"]);
            if (checkApi == 0)
            {
                return true;
            }
            if (!list.Any()) return false;
            if (string.IsNullOrEmpty(configKey)) return false;

            var inputParamsString = new StringBuilder();
            foreach (var item in list)
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