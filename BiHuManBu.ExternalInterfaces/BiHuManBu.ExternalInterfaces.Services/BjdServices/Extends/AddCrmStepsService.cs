using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Infrastructure;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using log4net;
using ServiceStack.Text;

namespace BiHuManBu.ExternalInterfaces.Services.BjdServices.Extends
{
    public class AddCrmStepsService : IAddCrmStepsService
    {
        private ILog logErr;
        private ILog logInfo;

        public AddCrmStepsService()
        {
            logErr = LogManager.GetLogger("ERROR");
            logInfo = LogManager.GetLogger("INFO");
        }

        public void AddCrmSteps(int agentId, string agentName, string mobile, string licenseNo, long newSource, double bizRate,
            double forceRate, int activityId, long buid, long bxid, int cityCode)
        {
            try
            {

                var crmTimeLineForSmsViewModel = new CrmTimeLineForSmsViewModel
                {
                    agent_id = agentId,
                    agent_name = agentName,
                    content = "",
                    sent_mobile = mobile,
                    sent_type = 0,
                    license_no = licenseNo,
                    Source = newSource,
                    sourceName = GetSourceName(newSource),
                    BizRate = bizRate,
                    ForceRate = forceRate,
                    ActivityId = activityId,
                    Bxid = bxid,
                    CityId = cityCode
                };

                string host = ConfigurationManager.AppSettings["SystemCrmUrl"];
                string url = string.Format("{0}/api/ConsumerDetail/AddCrmSteps", host);
                string postDataNoSecCode =
                    string.Format(
                        "JsonContent={0}&AgentId={1}&Type=11&BUid={2}",
                        crmTimeLineForSmsViewModel.ToJson(), agentId, buid);
                string res = String.Empty;
                string secCode = postDataNoSecCode.GetMd5();
                string postData = postDataNoSecCode + "&SecCode=" + secCode;
                //记录请求
                logInfo.Info("续保调用分配接口：Url:" + url + "，请求：" + postData);
                using (var client = new HttpClient(new HttpClientHandler()))
                {
                    HttpContent content = new StringContent(postData);
                    var typeHeader = new MediaTypeHeaderValue("application/x-www-form-urlencoded") { CharSet = "UTF-8" };
                    content.Headers.ContentType = typeHeader;
                    var response = client.PostAsync(url, content).Result;
                }
            }
            catch (Exception ex)
            {
                logErr.Info("续保请求发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
        }

        private static string GetSourceName(long newSource)
        {
            if (newSource == 2147483648)
            {
                return "恒邦车险";
            }
            if (newSource == 4294967296)
            {
                return "中铁车险";
            }
            if (newSource == 8589934592)
            {
                return "美亚车险";
            }
            if (newSource == 17179869184)
            {
                return "富邦车险";
            }
            return ToEnumDescription(newSource, typeof(EnumSourceNew));
        }

        private static String ToEnumDescription(long value, Type enumType)
        {
            NameValueCollection nvc = GetNvcFromEnumValue(enumType);
            return nvc[value.ToString()];
        }

        private static NameValueCollection GetNvcFromEnumValue(Type enumType)
        {
            var nvc = new NameValueCollection();
            Type typeDescription = typeof(DescriptionAttribute);
            System.Reflection.FieldInfo[] fields = enumType.GetFields();
            string strText = string.Empty;
            string strValue = string.Empty;
            foreach (FieldInfo field in fields)
            {
                if (field.FieldType.IsEnum)
                {
                    strValue = ((int)enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null)).ToString();
                    object[] arr = field.GetCustomAttributes(typeDescription, true);
                    if (arr.Length > 0)
                    {
                        var aa = (DescriptionAttribute)arr[0];
                        strText = aa.Description;
                    }
                    else
                    {
                        strText = "";
                    }
                    nvc.Add(strValue, strText);
                }
            }
            return nvc;
        }
    }
}
