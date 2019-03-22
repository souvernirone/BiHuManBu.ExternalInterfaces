using BiHuManBu.ExternalInterfaces.Services.GetReInfoService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Mapper;
using BiHuManBu.ExternalInterfaces.Services.Messages.RemoteMessage;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using log4net;
using ServiceStack.Text;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.GetReInfoService.Implementations
{
    public class GetIntelligentInsurance: IGetIntelligentInsurance
    {
        private ILog logError = LogManager.GetLogger("ERROR");
        private ILog logInfo = LogManager.GetLogger("INFO");
        private string centerUrl = System.Configuration.ConfigurationManager.AppSettings["baoxianCenterApi"];
        public GetIntelligentInsurance() { }

        public async Task<Tuple<SaveQuoteViewModel, bool>> GetCenterInsurance(GetIntelligentReInfoRequest request)
        {
            Tuple<RecommendModel, bool> model;
            /* demo如下：
             *http://192.168.1.19:5790/api/InsuranceRecommend/Get?moldName=别克SGM6531UAAF&debutDate=2017-04-01&licenseNo=京J87653
             * 如果模型返回的覆盖范围total值小于100，则将licenseno去掉，乔培培会根据licenseno的城市返回拿取值范围
             */
            //get请求
            bool isGet = false;
            string strUrl = string.Format("{0}/api/InsuranceRecommend/Get?moldName={1}&debutDate={2}&licenseNo={3}", centerUrl, request.MoldName, request.RegisterDate, request.LicenseNo);
            model = await SimulateGet(strUrl);
            if (model.Item1.Total < 100)
            {
                strUrl = string.Format("{0}/api/InsuranceRecommend/Get?moldName={1}&debutDate={2}", centerUrl, request.MoldName, request.RegisterDate);
                model = await SimulateGet(strUrl);
            }
            //模型转换
            SaveQuoteViewModel newmodel = model.Item1.ConverToViewModel();
            return new Tuple<SaveQuoteViewModel, bool>(newmodel, model.Item2);
        }

        private async Task<Tuple<RecommendModel, bool>> SimulateGet(string strUrl)
        {
            RecommendModel model = new RecommendModel();
            bool isGet = false;
            try
            {
                string responsemsg = string.Empty;
                using (var client = new HttpClient())
                {
                    //设置超时时间 180秒
                    client.Timeout = TimeSpan.FromMilliseconds(1000 * 30);
                    var clientResult = await client.GetAsync(strUrl);
                    if (clientResult.IsSuccessStatusCode)
                    {
                        responsemsg = await clientResult.Content.ReadAsStringAsync();
                        //loginfo记录返回
                        logInfo.Info(string.Format("中心智能获取险种请求串:{0}，返回：{1}", strUrl, responsemsg));
                    }
                    if (!string.IsNullOrWhiteSpace(responsemsg) && responsemsg.Length > 20)
                    {
                        //解析对象，中心返回的对象不规则，这里得注意try，catch
                        model = responsemsg.FromJson<RecommendModel>();
                        isGet = true;
                        return new Tuple<RecommendModel, bool>(model, isGet);
                    }
                }
                return new Tuple<RecommendModel, bool>(model, isGet);
            }
            catch (Exception ex)
            {
                logError.Info("请求中心获取智能推荐险种异常，请求串为：" + strUrl + "\n 异常信息:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return new Tuple<RecommendModel, bool>(new RecommendModel(), isGet);
        }
    }
}
