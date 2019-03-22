using BiHuManBu.ExternalInterfaces.Infrastructure;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.IRepository;
using BiHuManBu.ExternalInterfaces.Models.PartialModels.bx_agent;
using BiHuManBu.ExternalInterfaces.Services.GetReInfoService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.ValidateService.Interfaces;
using log4net;
using ServiceStack.Text;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.GetReInfoService.Implementations
{
    public class PostThirdPartService: IPostThirdPartService
    {
        private readonly IGetAgentInfoService _getAgentInfoService;
        private IConfigRepository _configRepository;
        private ILog logInfo = LogManager.GetLogger("INFO");
        private ILog logError = LogManager.GetLogger("ERROR");
        public PostThirdPartService(IGetAgentInfoService getAgentInfoService,IConfigRepository configRepository) {
            _getAgentInfoService = getAgentInfoService;
            _configRepository = configRepository;
        }

        /// <summary>
        /// 续保调用第三方接口传数据
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="viewModel"></param>
        public void PostThirdPart(int agent, ViewModels.GetReInfoViewModel viewModel)
        {
            //请求第三方Url
            string strUrl = string.Empty;
            //取缓存值
            var cacheKey = string.Format("camera_url_{0}", agent);
            strUrl = CacheProvider.Get<string>(cacheKey);
            if (string.IsNullOrEmpty(strUrl))
            {
                bx_config config = _configRepository.Find(agent.ToString(), 4);
                strUrl = config != null ? config.config_value : "";
                CacheProvider.Set(cacheKey, strUrl, 10800);
            }
            //判断url是否为空
            if (string.IsNullOrEmpty(strUrl))
            {
                return;
            }
            IBxAgent agentModel = _getAgentInfoService.GetAgentModelFactory(agent);
            string secretKey = agentModel == null ? "" : agentModel.SecretKey;
            //执行post请求方法
            Task.Factory.StartNew(() => SendPost(agent, secretKey, strUrl, viewModel));
        }

        private void SendPost(int agent, string secretKey, string strUrl, ViewModels.GetReInfoViewModel viewModel)
        {
            try
            {
                var webClientObj = new WebClient();
                var postVars = new System.Collections.Specialized.NameValueCollection();
                //返回状态内容
                postVars.Add("BusinessStatus", viewModel.BusinessStatus.ToString());
                postVars.Add("StatusMessage", viewModel.StatusMessage);
                //返回UserInfo
                postVars.Add("UserInfo", viewModel.UserInfo.ToJson());
                //返回SaveQuote
                postVars.Add("SaveQuote", viewModel.SaveQuote.ToJson());
                postVars.Add("CustKey", viewModel.CustKey);
                postVars.Add("Agent", agent.ToString());
                string secCode =
                    string.Format("Agent={0}&BusinessStatus={1}&CustKey={2}&SaveQuote={3}&StatusMessage={4}&UserInfo={5}{6}",
                        agent, viewModel.BusinessStatus, viewModel.CustKey, viewModel.SaveQuote.ToJson(), viewModel.StatusMessage, viewModel.UserInfo.ToJson(), secretKey);
                postVars.Add("SecCode", secCode.GetMd5());

                byte[] byRemoteInfo = webClientObj.UploadValues(strUrl, "POST", postVars);
                //返回值
                string remoteInfo = Encoding.UTF8.GetString(byRemoteInfo);
                logInfo.Info(string.Format("请求第三方{0}接口返回消息：{1}", agent, remoteInfo));

                ////post请求
                //if (agent == 79055 || agent == 73943 || agent == 95554)
                //{
                //    byte[] byRemoteInfo = webClientObj.UploadValues(strUrl, "POST", postVars);
                //    //返回值
                //    string remoteInfo = Encoding.UTF8.GetString(byRemoteInfo);
                //    logInfo.Info(string.Format("请求第三方{0}接口返回消息：{1}", agent, remoteInfo));
                //}
                //else
                //{
                //    var postVarsNew = new System.Collections.Specialized.NameValueCollection();
                //    postVarsNew.Add("values", postVarsNew.ToJson());
                //    byte[] byRemoteInfo = webClientObj.UploadValues(strUrl, "POST", postVarsNew);
                //    //返回值
                //    string remoteInfo = Encoding.UTF8.GetString(byRemoteInfo);
                //    logInfo.Info(string.Format("请求第三方{0}接口返回消息：{1}", agent, remoteInfo));
                //}
            }
            catch (Exception ex)
            {
                logError.Error("调用" + agent + "接口传摄像头续保信息接口异常，Url为：" + strUrl + "；\n 异常信息:" + ex.StackTrace + " \n " + ex.Message);
            }
        }
    }
}
