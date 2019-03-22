using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace BiHuManBu.ExternalInterfaces.Infrastructure.Helpers
{
    /// <summary>
    /// 中心提供的Get、Post请求
    /// </summary>
    public static class ProxyCenterHttpClient
    {
        /// <summary>
        /// GET请求类型
        /// </summary>
        /// <param name="url">完整请求地址</param>
        /// <param name="timeout">超时时间,单位秒</param>
        /// <param name="returnServerIpPort">如果访问成功,返回访问的真实服务地址</param>
        /// <param name="serverIpPort">指定服务地址,如果须要</param>
        /// <returns>返回结果,或抛出异常</returns>
        public static string Get(string url, int timeout, ref string returnServerIpPort, string serverIpPort = "")
        {
            using (var client = new HttpClient { Timeout = new TimeSpan(0, 0, timeout) })
            {
                client.DefaultRequestHeaders.Add("Timeout", timeout.ToString());
                client.DefaultRequestHeaders.Add("ServerIpPort", serverIpPort);

                using (var response = client.GetAsync(url).Result)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    SetServerIpPort(response.Headers, ref returnServerIpPort);
                    return result;
                }
            }
        }

        /// <summary>
        /// POST请求类型
        /// </summary>
        /// <param name="url">完整请求地址</param>
        /// <param name="postData">Post参数,默认Json格式</param>
        /// <param name="timeout">超时时间,单位秒</param>
        /// <param name="returnServerIpPort">如果访问成功,返回访问的真实服务地址</param>
        /// <param name="serverIpPort">指定服务地址,如果须要</param>
        /// <returns>返回结果,或抛出异常</returns>
        public static string Post(string url, string postData, int timeout, ref string returnServerIpPort, string serverIpPort = "")
        {
            using (var client = new HttpClient { Timeout = new TimeSpan(0, 0, timeout) })
            {
                client.DefaultRequestHeaders.Add("Timeout", timeout.ToString());
                client.DefaultRequestHeaders.Add("ServerIpPort", serverIpPort);

                var content = new StringContent(postData);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                using (var response = client.PostAsync(url, content).Result)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    SetServerIpPort(response.Headers, ref returnServerIpPort);
                    return result;
                }
            }
        }

        private static void SetServerIpPort(HttpResponseHeaders headers, ref string returnServerIpPort)
        {
            IEnumerable<string> headersList;
            headers.TryGetValues("ServerIpPort", out headersList);
            if (headersList != null)
                returnServerIpPort = headersList.First();
        }
    }
}
