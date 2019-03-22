using System.Net;
using System.Net.Http;
using System.Web.Http;
using BiHuManBu.ExternalInterfaces.Infrastructure;
using BiHuManBu.ExternalInterfaces.Services.AgentConfigService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.GetAgentConfigByCityService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using log4net;
using ServiceStack.Text;

namespace BiHuManBu.ExternalInterfaces.API.Controllers
{
    public class QuotedChannelsController : ApiController
    {
        private readonly IGetAgentConfigByCitysService _agentConfigByCitysService;
        private readonly ILog _logInfo;
        private ILog _logError;

        public QuotedChannelsController(IGetAgentConfigByCitysService agentConfigByCitysService)
        {
            _agentConfigByCitysService = agentConfigByCitysService;
             _logInfo = LogManager.GetLogger("INFO");
            _logError = LogManager.GetLogger("ERROR");
        }

        #region 获取报价渠道 李金友 2017-12-12 /PC

        /// <summary>
        ///  获取报价渠道 李金友 2017-12-12 /PC
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetMultiQuotedChannels([FromUri]MultiQuotedChannelsRequest request)
        {
            _logInfo.Info("获取报价渠道请求" + Request.RequestUri);
            var viewModel = new ResponseMultiQuotedChannelsViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                viewModel.StatusMessage = "输入参数错误，请检查您输入的参数是否有空或者长度不符合要求等";
                return viewModel.ResponseToJson();
            }

            return
                _agentConfigByCitysService.GetAgentConfigByCityResponse(request.Agent, request.CityCode).ResponseToJson();
        }

        #endregion



        #region MyRegion

        /// <summary>
        /// 上次取渠道状态的接口  李金友 2017-12-19 /PC
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetLastQuotedChannel([FromUri]GetLastQuotedRequest request)
        {

            _logInfo.Info("获取上次报价渠道请求" + Request.RequestUri);
            var viewModel = new ResponseMultiQuotedChannelsViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                viewModel.StatusMessage = "输入参数错误，请检查您输入的参数是否有空或者长度不符合要求等";
                return viewModel.ResponseToJson();
            }

            return
                _agentConfigByCitysService.GetLastQuotedResponse(request).ResponseToJson();
        }

        #endregion


    }
}
