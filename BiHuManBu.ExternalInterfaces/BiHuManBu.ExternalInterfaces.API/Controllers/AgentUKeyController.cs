using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BiHuManBu.ExternalInterfaces.Infrastructure;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.ReportModel;
using BiHuManBu.ExternalInterfaces.Services.AgentChannelService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Mapper;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using log4net;
using ServiceStack.Text;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;

namespace BiHuManBu.ExternalInterfaces.API.Controllers
{
    public class AgentUKeyController : ApiController
    {
        //
        // GET: /AgentUKey/

        private IAgentUKeyService _agentUKeyService;
        private IChannelService _channelService;
        private readonly IChannelStatusByUkeyIdService _channelStatusByUkeyIdService;
        private ILog _logInfo;

        public AgentUKeyController(IAgentUKeyService agentUKeyService, IChannelService channelService, IChannelStatusByUkeyIdService channelStatusByUkeyIdService)
        {
            _agentUKeyService = agentUKeyService;
            _channelService = channelService;
            _channelStatusByUkeyIdService = channelStatusByUkeyIdService;
            _logInfo = LogManager.GetLogger("INFO");
        }

        /// <summary>
        /// 代理人可修改的Ukey列表，目前仅对外使用
        /// 博福易商
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetUKeyList([FromUri]GetUKeyListRequest request)
        {
            _logInfo.Info(string.Format("获取代理人UKey列表接口请求串：{0}", Request.RequestUri));
            var viewModel = new UKeyListViewModel();
            viewModel.CityUKeyList = new List<CityUKeyModel>();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(a => a.Errors.Count == 1).Aggregate(string.Empty, (current, a) => current + (a.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }
            var response = _agentUKeyService.GetUKeyList(request, Request.GetQueryNameValuePairs());
            if (response.Status == HttpStatusCode.Forbidden)
            {
                viewModel.BusinessStatus = -10001;
                viewModel.StatusMessage = "参数校验错误，请检查您的校验码";
                return viewModel.ResponseToJson();
            }
            if (response.Status == HttpStatusCode.ExpectationFailed)
            {
                viewModel.BusinessStatus = -10003;
                viewModel.StatusMessage = "服务发生异常";
                return viewModel.ResponseToJson();
            }
            if (response.Status == HttpStatusCode.OK)
            {
                viewModel.BusinessStatus = 1;
                viewModel.StatusMessage = "获取信息成功";
                viewModel.CityUKeyList = response.CityUKeyList;
            }
            else
            {
                viewModel.BusinessStatus = 0;
                viewModel.StatusMessage = "未获取到数据";
            }
            return viewModel.ResponseToJson();
        }

        /// <summary>
        /// 代理人修改Ukey密码，对外+crm+运营后台
        /// 博福易商
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage EditAgentUKey([FromBody]EditAgentUKeyRequest request)
        {
            _logInfo.Info(string.Format("修改代理人UKey接口请求串：{0}", request.ToJson()));
            var viewModel = new BaseViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(a => a.Errors.Count == 1).Aggregate(string.Empty, (current, a) => current + (a.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }
            //对外接口的OldPassWord不能为空
            if (request.ReqSource == 1 && string.IsNullOrWhiteSpace(request.OldPassWord))
            {
                viewModel.BusinessStatus = -10000;
                viewModel.StatusMessage = "输入参数错误";
                return viewModel.ResponseToJson();
            }

            //请求ukey修改密码接口
            var response = _agentUKeyService.EditAgentUKey(request, Request.GetQueryNameValuePairs());
            if (response.Status == HttpStatusCode.Forbidden)
            {
                viewModel.BusinessStatus = -10001;
                viewModel.StatusMessage = "参数校验错误，请检查您的校验码";
                return viewModel.ResponseToJson();
            }
            if (response.Status == HttpStatusCode.ExpectationFailed)
            {
                viewModel.BusinessStatus = -10003;
                viewModel.StatusMessage = "服务发生异常";
                return viewModel.ResponseToJson();
            }
            if (response.Status == HttpStatusCode.OK)
            {
                viewModel.BusinessStatus = 1;
                viewModel.StatusMessage = "修改信息成功";
            }
            else
            {
                viewModel.BusinessStatus = 0;
                viewModel.StatusMessage = response.ErrMsg;
            }
            return viewModel.ResponseToJson();
        }

        /// <summary>
        /// 修改代理人备份密码 2017-10-20 zky/运营后台
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage EditBackupPwd([FromBody]EditBackupPwdRequest request)
        {
            _logInfo.Info(string.Format("修改代理人UKey备用密码接口请求串：{0}", request.ToJson()));
            var viewModel = new BaseViewModel();

            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(a => a.Errors.Count == 1).Aggregate(string.Empty, (current, a) => current + (a.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }

            string param = string.Format("Agent={0}&PwdOne={1}&PwdThree={2}&PwdTwo={3}&ReqSource={4}&UkeyId={5}", request.Agent, request.PwdOne, request.PwdThree, request.PwdTwo, request.ReqSource, request.UkeyId, request.UkeyId);
            if (request.SecCode != param.GetMd5())
            {
                viewModel.BusinessStatus = -10001;
                viewModel.StatusMessage = "参数校验错误，请检查您的校验码";
                return viewModel.ResponseToJson();
            }

            var response = _agentUKeyService.EditBackupPwd(request);
            if (response.ErrCode == -1)
            {
                viewModel.BusinessStatus = 0;
                viewModel.StatusMessage = response.ErrMsg;

            }
            else if (response.ErrCode == 0)
            {
                viewModel.BusinessStatus = 1;
                viewModel.StatusMessage = "修改成功";
            }
            return viewModel.ResponseToJson();
        }

        /// <summary>
        /// 获取渠道及状态
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetChannelStatus([FromUri]GetChannelStatusRequest request)
        {
            _logInfo.Info(string.Format("获取渠道及状态接口请求串：{0}", Request.RequestUri));
            var viewModel = new GetChannelStatusViewModel();
            viewModel.ChannelList = new List<ChannelStatusModel>();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(a => a.Errors.Count == 1).Aggregate(string.Empty, (current, a) => current + (a.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }
            if (request.CityCode == 0 && request.ChannelId == 0)
            {
                viewModel.BusinessStatus = -10001;
                viewModel.StatusMessage = "参数校验错误，请检查您的校验码";
                return viewModel.ResponseToJson();
            }
            //请求获取渠道及状态接口
            var response = _channelService.GetChannelStatus(request, Request.GetQueryNameValuePairs());
            if (response.Status == HttpStatusCode.Forbidden)
            {
                viewModel.BusinessStatus = -10001;
                viewModel.StatusMessage = "参数校验错误，请检查您的校验码";
                return viewModel.ResponseToJson();
            }
            if (response.Status == HttpStatusCode.ExpectationFailed)
            {
                viewModel.BusinessStatus = -10003;
                viewModel.StatusMessage = "服务发生异常";
                return viewModel.ResponseToJson();
            }
            if (response.Status == HttpStatusCode.OK)
            {
                viewModel.BusinessStatus = 1;
                viewModel.StatusMessage = "获取信息成功";
                viewModel.ChannelList = response.CacheChannelList.ConverToChannelViewModel(request.ShowIsPaicApi,request.Agent);
            }
            else
            {
                viewModel.BusinessStatus = 0;
                viewModel.StatusMessage = response.ErrMsg;
            }
            return viewModel.ResponseToJson();
        }

        /// <summary>
        /// 根据UkeyId获取渠道及状态
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetChannelStatusByUkeyId([FromUri]GetChannelStatusByUkeyIdRequest request)
        {
            _logInfo.Info(string.Format("根据UkeyId获取渠道及状态接口请求串：{0}", Request.RequestUri));
            var viewModel = new ChannelStatusModel2();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(a => a.Errors.Count == 1).Aggregate(string.Empty, (current, a) => current + (a.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }
            //请求获取渠道及状态接口
            var response = _channelStatusByUkeyIdService.GetChannelStatusByUkeyId(request, Request.GetQueryNameValuePairs());
            if (response.Status == HttpStatusCode.Forbidden)
            {
                viewModel.BusinessStatus = -10001;
                viewModel.StatusMessage = "参数校验错误，请检查您的校验码";
                return viewModel.ResponseToJson();
            }
            if (response.Status == HttpStatusCode.ExpectationFailed)
            {
                viewModel.BusinessStatus = -10003;
                viewModel.StatusMessage = "服务发生异常";
                return viewModel.ResponseToJson();
            }
            if (response.Status == HttpStatusCode.OK)
            {
                viewModel.StatusModel = response.CacheChannel.ConverToChannelViewModel(request.Agent);
                viewModel.BusinessStatus = 1;
                viewModel.StatusMessage = "获取信息成功";
            }
            else
            {
                viewModel.BusinessStatus = 0;
                viewModel.StatusMessage = response.ErrMsg;
                viewModel.StatusModel = new ChannelStatusBaseModel();
            }
            return viewModel.ResponseToJson();
        }
    }
}
