using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using BiHuManBu.ExternalInterfaces.Infrastructure;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.PartialModels.bx_agent;
using BiHuManBu.ExternalInterfaces.Repository;
using BiHuManBu.ExternalInterfaces.Services;
using BiHuManBu.ExternalInterfaces.Services.CacheServices;
using log4net;

namespace BiHuManBu.ExternalInterfaces.API.Controllers
{
    public class CallBackController:ApiController
    {
        private ILog logInfo = LogManager.GetLogger("INFO");
        private CommonBehaviorService _commonBehaviorService;

        public CallBackController(ICarInsuranceCache carInsuranceCache)
        {
            _commonBehaviorService = new CommonBehaviorService(new AgentRepository(),new CacheHelper());

        }
        [HttpGet]
        public async Task UpdateBaoJiaPrice(string licenseno,string intentioncompany,int agent,string custKey)
        {
            logInfo.Info("报价回调接口"+Request.RequestUri);
            //if (agent != 4405) return;
            string str1 = string.Empty;
            IBxAgent agentModel = _commonBehaviorService.GetAgentModelFactory(agent);
            //custKey = custKey.Substring(custKey.IndexOf("-", System.StringComparison.Ordinal) + 1);

            var pairs = Request.GetQueryNameValuePairs();
            var newPairs = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<string, string> pair in pairs)
            {
                if (pair.Key == "custkey")
                {
                    newPairs.Add(new KeyValuePair<string, string>(pair.Key, custKey));
                }
                else
                {
                    newPairs.Add(new KeyValuePair<string, string>(pair.Key, pair.Value));
                }
              
            }
             
           
            string  secCode = _commonBehaviorService.GetSecCode(newPairs, agentModel.SecretKey);
           
            

            using (HttpClient client = new HttpClient())
            {
                //var url = System.Configuration.ConfigurationManager.AppSettings["CarInsuranceGetPriceInfo"];
               var geturl =string.Format("http://it.91bihu.com/api/CarInsurance/GetPrecisePrice?licenseno={0}&intentioncompany={1}&agent={2}&seccode={3}&custkey={4}", licenseno, intentioncompany, agent, secCode, custKey);
                //var geturl =
                // string.Format("http://192.168.3.12:8099/api/CarInsurance/GetPrecisePrice?licenseno={0}&intentioncompany={1}&agent={2}&seccode={3}&custkey={4}", licenseno, intentioncompany, agent, secCode, custKey);
                
                var clientResult =
                    await client.GetAsync(geturl);
                if (clientResult.IsSuccessStatusCode)
                {
                    str1 = await clientResult.Content.ReadAsStringAsync();
                }
            }
           
            //根据agent确定回调地址
            string str = str1;
            //string str =
            //    "\"UserInfo\":{\"LicenseNo\":\"京NR4923\",\"ForceExpireDate\":\"2017-03-24\",\"BusinessExpireDate\":\"2016-03-24\"}";
            using (HttpClient client = new HttpClient())
            {
                DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                DateTime dtNow = DateTime.Parse(DateTime.Now.ToString());
                TimeSpan toNow = dtNow.Subtract(dtStart);
                string timeStamp = toNow.Ticks.ToString();
                timeStamp = timeStamp.Substring(0, timeStamp.Length - 7);
                var _from = "bihu";
                var _nonce = "abcdefgh";
                var _timestamp = timeStamp;
               // var queryString = "data=" + @"{UserInfo:{LicenseNo:京NR4923,ForceExpireDate:2017-03-24,BusinessExpireDate:2016-03-24},Item:{BizRate:0,ForceRate:0,BizTotal:2396.07,ForceTotal:0,TaxTotal:0,Source:2,QuoteStatus:-1,QuoteResult:成功,CheSun:{BaoE:95900,BaoFei:984.92},SanZhe:{BaoE:300000,BaoFei:717.49},DaoQiang:{BaoE:0,BaoFei:0},SiJi:{BaoE:10000,BaoFei:23.99},ChengKe:{BaoE:10000,BaoFei:60.86},BoLi:{BaoE:1,BaoFei:106.63},HuaHen:{BaoE:2000,BaoFei:234.09},SheShui:{BaoE:0,BaoFei:0},CheDeng:{BaoE:1,BaoFei:0},ZiRan:{BaoE:0,BaoFei:0},BuJiMianCheSun:{BaoE:1,BaoFei:147.74},BuJiMianSanZhe:{BaoE:1,BaoFei:107.62},BuJiMianDaoQiang:{BaoE:0,BaoFei:0},BuJiMianRenYuan:{BaoE:1,BaoFei:12.73},BuJiMianFuJia:{BaoE:0,BaoFei:0}},BusinessStatus:1}";
               // var transData = str.Replace("\"", "");
                var transData = str;
                var queryString = "data=" + transData;
                var request = new
                {
                    _from = "bihu",
                    _nonce = "abcdefgh",
                    _timestamp = timeStamp,
                    _sign = (queryString.GetMd5() + _nonce + _timestamp + _from +
                            "NmQ0YzhmNzhlZmM1OWNk").GetMd5().ToUpper(),
                    data = transData
                };
                var datas = CommonHelper.ReverseEachProperties(request);

                var postData = new System.Net.Http.FormUrlEncodedContent(datas);
                var clientResult =
                    await client.PostAsync("http://210.13.242.24:7001/api/insurance/updatePrice", postData);
                //var rr2 = response2.Content.ReadAsStringAsync().Result;
                if (clientResult.IsSuccessStatusCode)
                {
                    var raw_response = await clientResult.Content.ReadAsByteArrayAsync();
                   var  result = Encoding.Default.GetString(await clientResult.Content.ReadAsByteArrayAsync(), 0, raw_response.Length);
                   if (result.IndexOf("10000", System.StringComparison.Ordinal) > 0)
                    {
                        logInfo.Info("获取报价回调成功" + str);
                    }
                   else
                   {
                       logInfo.Info("获取报价回调失败" + str);
                   }

                }

            }
           
        }


        [HttpGet]
        public async Task UpdateHeBaoInfo(string licenseno, string intentioncompany, int agent, string custKey)
        {
            logInfo.Info("核保回调接口" + Request.RequestUri);
            string str1 = string.Empty;
            IBxAgent agentModel = _commonBehaviorService.GetAgentModelFactory(agent);

            //custKey = custKey.Substring(custKey.IndexOf("-", System.StringComparison.Ordinal) + 1);

            var pairs = Request.GetQueryNameValuePairs();
            var newPairs = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<string, string> pair in pairs)
            {
                if (pair.Key == "custkey")
                {
                    newPairs.Add(new KeyValuePair<string, string>(pair.Key, custKey));
                }
                else
                {
                    newPairs.Add(new KeyValuePair<string, string>(pair.Key, pair.Value));
                }

            }


            string secCode = _commonBehaviorService.GetSecCode(newPairs, agentModel.SecretKey);

           
            using (HttpClient client = new HttpClient())
            {
               // var url = System.Configuration.ConfigurationManager.AppSettings["CarInsuranceGetSubmitInfo"];

                var geturl =
                    string.Format("http://it.91bihu.com/api/CarInsurance/GetSubmitInfo?licenseno={0}&intentioncompany={1}&agent={2}&seccode={3}&custkey={4}", licenseno, intentioncompany, agent, secCode, custKey);
                var clientResult =await client.GetAsync(geturl);
                if (clientResult.IsSuccessStatusCode)
                {
                    str1 = await clientResult.Content.ReadAsStringAsync();
                }
            }

            //根据agent确定回调地址
            string str = str1;
            //string str =
            //    "\"UserInfo\":{\"LicenseNo\":\"京NR4923\",\"ForceExpireDate\":\"2017-03-24\",\"BusinessExpireDate\":\"2016-03-24\"}";
            using (HttpClient client = new HttpClient())
            {
                DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                DateTime dtNow = DateTime.Parse(DateTime.Now.ToString());
                TimeSpan toNow = dtNow.Subtract(dtStart);
                string timeStamp = toNow.Ticks.ToString();
                timeStamp = timeStamp.Substring(0, timeStamp.Length - 7);
                var _from = "bihu";
                var _nonce = "abcdefgh";
                var _timestamp = timeStamp;
                // var queryString = "data=" + @"{UserInfo:{LicenseNo:京NR4923,ForceExpireDate:2017-03-24,BusinessExpireDate:2016-03-24},Item:{BizRate:0,ForceRate:0,BizTotal:2396.07,ForceTotal:0,TaxTotal:0,Source:2,QuoteStatus:-1,QuoteResult:成功,CheSun:{BaoE:95900,BaoFei:984.92},SanZhe:{BaoE:300000,BaoFei:717.49},DaoQiang:{BaoE:0,BaoFei:0},SiJi:{BaoE:10000,BaoFei:23.99},ChengKe:{BaoE:10000,BaoFei:60.86},BoLi:{BaoE:1,BaoFei:106.63},HuaHen:{BaoE:2000,BaoFei:234.09},SheShui:{BaoE:0,BaoFei:0},CheDeng:{BaoE:1,BaoFei:0},ZiRan:{BaoE:0,BaoFei:0},BuJiMianCheSun:{BaoE:1,BaoFei:147.74},BuJiMianSanZhe:{BaoE:1,BaoFei:107.62},BuJiMianDaoQiang:{BaoE:0,BaoFei:0},BuJiMianRenYuan:{BaoE:1,BaoFei:12.73},BuJiMianFuJia:{BaoE:0,BaoFei:0}},BusinessStatus:1}";
                //var transData = str.Replace("\"", "");
                var transData = str;
                var queryString = "data=" + transData;
                var request = new
                {
                    _from = "bihu",
                    _nonce = "abcdefgh",
                    _timestamp = timeStamp,
                    _sign = (queryString.GetMd5() + _nonce + _timestamp + _from +
                            "NmQ0YzhmNzhlZmM1OWNk").GetMd5().ToUpper(),
                    data = transData
                };
                var datas = CommonHelper.ReverseEachProperties(request);

                var postData = new FormUrlEncodedContent(datas);
                var clientResult =
                    await client.PostAsync("http://210.13.242.24:7001/api/insurance/updateOrder", postData);
                //var rr2 = response2.Content.ReadAsStringAsync().Result;
                if (clientResult.IsSuccessStatusCode)
                {
                    var raw_response = await clientResult.Content.ReadAsByteArrayAsync();
                    var result = Encoding.Default.GetString(await clientResult.Content.ReadAsByteArrayAsync(), 0, raw_response.Length);
                    if (result.IndexOf("10000", System.StringComparison.Ordinal) > 0)
                    {
                        logInfo.Info("获取核保回调成功" + str);
                    }
                    else
                    {
                        logInfo.Info("获取核保回调失败" + str);
                    }

                }

            }

        }
    }
}