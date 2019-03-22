using BiHuManBu.ExternalInterfaces.Infrastructure.CacheKeyFactory;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Infrastructure.MessageCenter;
using BiHuManBu.ExternalInterfaces.Services.GetCarInfoService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using log4net;
using ServiceStack.Text;
using System;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.GetCarInfoService.Implementations
{
    public class GetMoldNameFromCenter : IGetMoldNameFromCenter
    {
        private static readonly string _url = ConfigurationManager.AppSettings["BaoxianCenter"];
        private ILog logError = LogManager.GetLogger("ERROR");
        private IMessageCenter _messageCenter;
        public GetMoldNameFromCenter(IMessageCenter messageCenter)
        {
            _messageCenter = messageCenter;
        }

        /// <summary>
        /// 从中心获取品牌型号
        /// </summary>
        /// <param name="carVin"></param>
        /// <param name="moldName"></param>
        /// <param name="agent">顶级代理Id</param>
        /// <param name="cityCode"></param>
        /// <returns></returns>
        public async Task<GetMoldNameResponse> GetMoldNameService(string carVin, string moldName, int agent, int cityCode)
        {
            GetMoldNameResponse response = new GetMoldNameResponse();
            if (string.IsNullOrWhiteSpace(carVin) || carVin.Length <= 5 || string.IsNullOrWhiteSpace(moldName))
            {
                response.MoldName = string.Empty;
                response.BusinessMessage = "不符合查询条件，不返回对应的品牌型号";
                response.BusinessStatus = -10000;
                return response;
            }
            var frontCarVin = carVin.ToUpper().Substring(0, 5);
            if (carVin.StartsWith("L") || moldName.ToUpper().IndexOf(frontCarVin, System.StringComparison.Ordinal) < 0)
            {
                response.MoldName = string.Empty;
                response.BusinessMessage = "不符合查询条件，不返回对应的品牌型号";
                response.BusinessStatus = -10000;
                return response;

            }
            string xuBaoCacheKey = CommonCacheKeyFactory.CreateKeyWithLicense(carVin);
            var xuBaoKey = string.Format("{0}-ModelName-key", xuBaoCacheKey);
            CacheProvider.Remove(xuBaoKey);
            var msgBody = new
            {
                Agent = agent,//顶级
                CarVin = carVin,
                cityId = cityCode,
                NotifyCacheKey = xuBaoCacheKey,
                RequestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };
            //发送续保信息
            var msgbody = _messageCenter.SendToMessageCenter(msgBody.ToJson(), ConfigurationManager.AppSettings["MessageCenter"], ConfigurationManager.AppSettings["FindMoldName"]);
            try
            {
                var cacheKey = CacheProvider.Get<string>(xuBaoKey);
                if (cacheKey == null)
                {
                    for (int i = 0; i < 60; i++)
                    {
                        cacheKey = CacheProvider.Get<string>(xuBaoKey);
                        if (!string.IsNullOrWhiteSpace(cacheKey))
                        {
                            break;
                        }
                        else
                        {
                            await Task.Delay(TimeSpan.FromSeconds(0.5));
                        }
                    }
                }
                if (cacheKey == null)
                {
                    response.BusinessStatus = -1;//超时
                    response.BusinessMessage = "请求超时";
                    response.MoldName = string.Empty;
                }
                else
                {
                    WaPaAutoModelResponse model = new WaPaAutoModelResponse();
                    string itemsCache = string.Format("{0}-ModelName", xuBaoCacheKey);
                    if (cacheKey == "1")
                    {
                        var temp = CacheProvider.Get<string>(itemsCache);
                        model = temp.FromJson<WaPaAutoModelResponse>();
                        if (model.ErrCode == 0)
                        {
                            response.MoldName = model.AutoModeType != null ? model.AutoModeType.autoModelName : string.Empty;
                            if (model.AutoModeType == null)
                            {
                                response.BusinessMessage = "没有取到对应的品牌型号";
                            }
                            else
                            {
                                response.BusinessMessage = "获取成功";
                            }
                        }
                        else
                        {
                            response.MoldName = string.Empty;
                            response.BusinessMessage = "没有取到对应的品牌型号";
                            response.BusinessStatus = -10002;
                        }
                    }
                    else
                    {
                        response.MoldName = string.Empty;
                        response.BusinessMessage = "没有取到对应的品牌型号";
                        response.BusinessStatus = -10002;
                    }
                }
            }
            catch (Exception ex)
            {
                response = new GetMoldNameResponse();
                response.Status = HttpStatusCode.ExpectationFailed;
                logError.Info("获取车型请求发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return response;
            #region 以前的逻辑
            //if (!string.IsNullOrWhiteSpace(carVin) && carVin.Length > 5)
            //{
            //    var frontCarVin = carVin.Substring(0, 5);
            //    if (!carVin.StartsWith("L") && moldName.ToUpper().IndexOf(frontCarVin, System.StringComparison.Ordinal) >= 0)
            //    {
            //        using (HttpClient client = new HttpClient())
            //        {
            //            client.BaseAddress = new Uri(_url);
            //            var getUrl = string.Format("api/taipingyang/gettaipycarinfoby?carvin={0}", carVin);
            //            HttpResponseMessage responseVin = client.GetAsync(getUrl).Result;
            //            var resultVin = responseVin.Content.ReadAsStringAsync().Result;
            //            var carinfo = resultVin.FromJson<WaGetTaiPyCarInfoResponse>();
            //            if (carinfo != null && carinfo.CarInfo != null)
            //            {
            //                return carinfo.CarInfo.moldName;
            //            }
            //        }
            //    }
            //}
            #endregion
        }
    }
}
