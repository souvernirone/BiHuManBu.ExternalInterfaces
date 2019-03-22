using BiHuManBu.ExternalInterfaces.Services.DistributeService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.GetReInfoService.Interfaces;
using log4net;
using System;

namespace BiHuManBu.ExternalInterfaces.Services.GetReInfoService.Implementations
{
    public class SentDistributedService: ISentDistributedService
    {
        private ILog logError = LogManager.GetLogger("ERROR");
        private IFilterMoldNameService _filterMoldNameService;
        public SentDistributedService(IFilterMoldNameService filterMoldNameService) {
            _filterMoldNameService = filterMoldNameService;
        }

        /// <summary>
        /// 发送分配请求
        /// 20180820.by.gpj修改.调用刘松年新版分配，不用userinfo的moldname了
        /// </summary>
        public void SentDistributed(int businessStatus, string moldName, long buid, int reqAgent, int reqChildAgent, string uiAgent, int reqCityCode, string reqLicenseNo, int reqRenewalType, int uiRenewalType, string businessExpireDate, string forceExpireDate, bool needCarMoldFilter, int cameraAgent, string cameraId, bool isAdd, int repeatStatus, int roleType, string custKey, int cityCode)
        {
            try
            {
                if (businessStatus == 1)
                {
                    if (!string.IsNullOrWhiteSpace(businessExpireDate))
                    {
                        var fd = DateTime.Parse(businessExpireDate);
                        if (fd.Date == DateTime.MinValue.Date)
                        {
                            businessExpireDate = string.Empty;
                        }
                        else
                        {
                            businessExpireDate = DateTime.Parse(businessExpireDate).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(forceExpireDate))
                    {
                        var fd = DateTime.Parse(forceExpireDate);
                        if (fd.Date == DateTime.MinValue.Date)
                        {
                            forceExpireDate = string.Empty;
                        }
                        else
                        {
                            forceExpireDate = DateTime.Parse(forceExpireDate).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                    }
                }

                _filterMoldNameService.FilterMoldName(moldName, reqChildAgent, reqAgent, buid, reqCityCode, businessExpireDate, forceExpireDate, cameraId, isAdd, repeatStatus, roleType, custKey, cityCode);
                //int childagent = reqChildAgent == 0 ? int.Parse(uiAgent) : reqChildAgent;
                //string _url = string.Format("{0}/api/Camera/FilterAndDistribute", _host);
                //string postDataNoSecCode =
                //    string.Format(
                //        "buId={0}&Agent={1}&CityCode={2}&LicenseNo={3}&businessExpireDate={4}&forceExpireDate={5}&childAgent={6}&CameraId{7}&carModelKey={8}&CameraAgent={9}",
                //        buid, reqAgent, reqCityCode, reqLicenseNo, businessExpireDate, forceExpireDate, childagent,cameraId, moldName, cameraAgent);
                //string res = String.Empty;
                //string secCode = postDataNoSecCode.GetMd5();
                //string postData = postDataNoSecCode + "&SecCode=" + secCode;
                ////记录请求
                //logInfo.Info(string.Format("续保调用{0}接口：Url:{1}，请求：{2}", "分配", _url, postData));
                //using (HttpClient client = new HttpClient(new HttpClientHandler()))
                //{
                //    HttpContent content = new StringContent(postData);
                //    MediaTypeHeaderValue typeHeader = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                //    typeHeader.CharSet = "UTF-8";
                //    content.Headers.ContentType = typeHeader;
                //    var response = client.PostAsync(_url, content).Result;
                //    //logInfo.Info(response.ToJson());
                //}
            }
            catch (Exception ex)
            {
                logError.Info("续保请求发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
        }
    }
}
