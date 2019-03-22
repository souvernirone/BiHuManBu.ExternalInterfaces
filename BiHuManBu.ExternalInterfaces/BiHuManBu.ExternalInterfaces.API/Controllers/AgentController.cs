using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BiHuManBu.ExternalInterfaces.Infrastructure;
using BiHuManBu.ExternalInterfaces.Models.ReportModel;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Mapper;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using log4net;
using ServiceStack.Text;

namespace BiHuManBu.ExternalInterfaces.API.Controllers
{
    public class AgentController : ApiController
    {
        private ILog _logInfo;
        private IAgentService _agentService;

        public AgentController(IAgentService agentService)
        {
            _agentService = agentService;
            _logInfo = LogManager.GetLogger("INFO");
        }

        /// <summary>
        /// 添加代理人信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddAgent([FromBody]PostAddAgentRequest request)
        {
            _logInfo.Info(string.Format("微信添加代理人接口请求串：{0}，参数：{1}", Request.RequestUri, request.ToJson()));
            var viewModel = new AgentViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }
            var response = _agentService.AddAgent(request, Request.GetQueryNameValuePairs());
            _logInfo.Info(string.Format("添加代理人接口返回值：{0}", response.ToJson()));
            if (response.Status == HttpStatusCode.BadRequest || response.Status == HttpStatusCode.Forbidden)
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
            if (response.ErrCode == -1)
            {
                viewModel.BusinessStatus = -1;
                viewModel.StatusMessage = "手机号已存在，请重新输入";
                return viewModel.ResponseToJson();
            }
            else
                if (response.ErrCode == -2)
            {
                viewModel.Agent = response.agent.ConverToViewModel();
                viewModel.BusinessStatus = -2;
                viewModel.StatusMessage = "OpenId已存在，请重新输入";
                return viewModel.ResponseToJson();
            }
            else if (response.ErrCode == -3)
            {
                viewModel.BusinessStatus = -3;
                viewModel.StatusMessage = "当前用户是三级代理，不允许新增下一级代理";
                return viewModel.ResponseToJson();
            }
            else if (response.ErrCode == -4)
            {
                viewModel.BusinessStatus = -4;
                viewModel.StatusMessage = "顶级代理参数错误，无法创建下级代理";
                return viewModel.ResponseToJson();
            }
            else if (response.ErrCode == -5)
            {
                viewModel.BusinessStatus = -5;
                viewModel.StatusMessage = "顶级代理参数错误，顶级代理下无此子集代理";
                return viewModel.ResponseToJson();
            }
            else
            {
                viewModel.Agent = response.agent.ConverToViewModel();
                viewModel.BusinessStatus = 1;
            }
            return viewModel.ResponseToJson();
        }

        /// <summary>
        /// 根据OpenId获取代理人信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetAgentByOpenId([FromUri]GetByOpenIdRequest request)
        {
            _logInfo.Info(string.Format("获取代理人接口请求串：{0}", Request.RequestUri));
            var viewModel = new AgentViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }
            var response = _agentService.GetAgentByOpenId(request, Request.GetQueryNameValuePairs());
            if (response.Status == HttpStatusCode.BadRequest || response.Status == HttpStatusCode.Forbidden)
            {
                viewModel.BusinessStatus = -10001;
                viewModel.StatusMessage = "参数校验错误，请检查您的校验码";
                return viewModel.ResponseToJson();
            }
            if (response.Status == HttpStatusCode.ExpectationFailed)
            {
                viewModel.BusinessStatus = -10003;
                viewModel.StatusMessage = "服务发生异常";
            }
            if (response.ErrCode == -1)
            {
                viewModel.BusinessStatus = -1;
                viewModel.StatusMessage = "无代理人信息";
            }
            else
            {
                viewModel.Agent = response.agent.ConverToViewModel();
                viewModel.BusinessStatus = 1;
            }
            return viewModel.ResponseToJson();
        }

        [HttpGet]
        public HttpResponseMessage GetAgentByOpenIdOld([FromUri]GetByOpenIdRequest request)
        {
            _logInfo.Info(string.Format("获取代理人接口（老）请求串：{0}", Request.RequestUri));
            var viewModel = new AgentViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }
            var response = _agentService.GetAgentByOpenIdOld(request, Request.GetQueryNameValuePairs());
            if (response.Status == HttpStatusCode.BadRequest || response.Status == HttpStatusCode.Forbidden)
            {
                viewModel.BusinessStatus = -10001;
                viewModel.StatusMessage = "参数校验错误，请检查您的校验码";
                return viewModel.ResponseToJson();
            }
            if (response.Status == HttpStatusCode.ExpectationFailed)
            {
                viewModel.BusinessStatus = -10003;
                viewModel.StatusMessage = "服务发生异常";
            }
            if (response.ErrCode == -1)
            {
                viewModel.BusinessStatus = -1;
                viewModel.StatusMessage = "无代理人信息";
            }
            else
            {
                viewModel.Agent = response.agent.ConverToViewModel();
                viewModel.BusinessStatus = 1;
            }
            return viewModel.ResponseToJson();
        }

        [HttpGet]
        public HttpResponseMessage GetAgentIdentity([FromUri]GetAgentIdentityAndRateRequest request)
        {
            _logInfo.Info(string.Format("获取代理人身份接口请求串：{0}", Request.RequestUri));
            var viewModel = new AgentIdentityAndRateViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }
            var response = _agentService.GetAgent(request, Request.GetQueryNameValuePairs());
            if (response.Status == HttpStatusCode.BadRequest || response.Status == HttpStatusCode.Forbidden)
            {
                viewModel.BusinessStatus = -10001;
                viewModel.StatusMessage = "参数校验错误，请检查您的校验码";
                return viewModel.ResponseToJson();
            }
            if (response.Status == HttpStatusCode.ExpectationFailed)
            {
                viewModel.BusinessStatus = -10003;
                viewModel.StatusMessage = "服务发生异常";
            }
            else
            {
                viewModel = response.ConvertToViewModel();
                viewModel.BusinessStatus = 1;
            }
            return viewModel.ResponseToJson();
        }

        [HttpGet]
        public HttpResponseMessage GetAgentSource([FromUri] GetAgentResourceRequest request)
        {
            _logInfo.Info(string.Format("微信获取代理人资源接口请求串：{0}", Request.RequestUri));
            var viewModel = new AgentSourceViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }

            var response = _agentService.GetAgentSource(request, Request.GetQueryNameValuePairs());
            if (response.Status == HttpStatusCode.BadRequest || response.Status == HttpStatusCode.Forbidden)
            {
                viewModel.BusinessStatus = -10001;
                viewModel.StatusMessage = "参数校验错误，请检查您的校验码";
                return viewModel.ResponseToJson();
            }
            if (response.Status == HttpStatusCode.ExpectationFailed)
            {
                viewModel.BusinessStatus = -10003;
                viewModel.StatusMessage = "服务发生异常";
            }
            else
            {
                viewModel.ComList = response.ComList;
                viewModel.BusinessStatus = 1;
            }

            return viewModel.ResponseToJson();
        }
        [HttpGet]
        public HttpResponseMessage GetAgentNewSource([FromUri] GetAgentResourceRequest request)
        {
            _logInfo.Info(string.Format("微信获取代理人资源New接口请求串：{0}", Request.RequestUri));
            var viewModel = new AgentSourceViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }

            var response = _agentService.GetAgentNewSource(request, Request.GetQueryNameValuePairs());
            if (response.Status == HttpStatusCode.BadRequest || response.Status == HttpStatusCode.Forbidden)
            {
                viewModel.BusinessStatus = -10001;
                viewModel.StatusMessage = "参数校验错误，请检查您的校验码";
                return viewModel.ResponseToJson();
            }
            if (response.Status == HttpStatusCode.ExpectationFailed)
            {
                viewModel.BusinessStatus = -10003;
                viewModel.StatusMessage = "服务发生异常";
            }
            else
            {
                viewModel.ComList = response.ComList;
                viewModel.BusinessStatus = 1;
            }

            return viewModel.ResponseToJson();
        }

        [HttpGet]
        public HttpResponseMessage GetAgentSourceName([FromUri]GetAgentResourceRequest request)
        {
            _logInfo.Info(string.Format("微信获取代理人资源+名称接口请求串：{0}", Request.RequestUri));
            var viewModel = new AppAgentSourceViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }

            var response = _agentService.GetAgentSource(request, Request.GetQueryNameValuePairs());
            if (response.Status == HttpStatusCode.BadRequest || response.Status == HttpStatusCode.Forbidden)
            {
                viewModel.BusinessStatus = -10001;
                viewModel.StatusMessage = "参数校验错误，请检查您的校验码";
                return viewModel.ResponseToJson();
            }
            if (response.Status == HttpStatusCode.ExpectationFailed)
            {
                viewModel.BusinessStatus = -10003;
                viewModel.StatusMessage = "服务发生异常";
            }
            else
            {
                viewModel.SourceList = response.ComList.ConverToViewModel();
                viewModel.BusinessStatus = 1;
            }

            return viewModel.ResponseToJson();
        }

        [HttpGet]
        public HttpResponseMessage GetAgentList([FromUri]GetAgentRequest request)
        {
            _logInfo.Info(string.Format("微信获取代理人列表接口请求串：{0}", Request.RequestUri));
            var viewModel = new AgentProtoViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }
            var response = _agentService.GetAgentList(request, Request.GetQueryNameValuePairs());
            if (response.Status == HttpStatusCode.BadRequest || response.Status == HttpStatusCode.Forbidden)
            {
                viewModel.BusinessStatus = -10001;
                viewModel.StatusMessage = "参数校验错误，请检查您的校验码";
                return viewModel.ResponseToJson();
            }
            if (response.Status == HttpStatusCode.InternalServerError)
            {
                viewModel.BusinessStatus = -10003;
                viewModel.StatusMessage = "服务发生异常";
            }
            if (response.Status == HttpStatusCode.NoContent)
            {
                viewModel.BusinessStatus = 0;
                viewModel.StatusMessage = "无代理人信息";
            }
            else
            {
                viewModel.BusinessStatus = 1;
                viewModel.TotalCount = response.totalCount;
                viewModel.AgentList = response.AgentList.ConverToViewModel();
            }
            return viewModel.ResponseToJson();
        }

        [HttpPost]
        public HttpResponseMessage ApproveAgent([FromBody]ApproveAgentRequest request)
        {
            _logInfo.Info(string.Format("微信审批代理人接口请求串：{0}，参数：{1}", Request.RequestUri, request.ToJson()));
            var viewModel = new BaseViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }
            var response = _agentService.ApproveAgent(request, Request.GetQueryNameValuePairs());
            if (response.Status == HttpStatusCode.BadRequest || response.Status == HttpStatusCode.Forbidden)
            {
                viewModel.BusinessStatus = -10001;
                viewModel.StatusMessage = "参数校验错误，请检查您的校验码";
                return viewModel.ResponseToJson();
            }
            if (response.Status == HttpStatusCode.InternalServerError)
            {
                viewModel.BusinessStatus = -10003;
                viewModel.StatusMessage = "服务发生异常";
            }
            if (response.Status == HttpStatusCode.OK)
            {
                viewModel.BusinessStatus = 1;
                viewModel.StatusMessage = "修改成功";
            }
            else
            {
                viewModel.BusinessStatus = 0;
                viewModel.StatusMessage = "修改失败";
            }
            return viewModel.ResponseToJson();
        }

        /// <summary>
        /// 单独开放给运营看的，获取到期的代理人
        /// </summary>
        /// <param name="Agent">要查询的代理</param>
        /// <param name="Status">查询的状态 1可用 2不可用</param>
        /// <param name="SecCode"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetAgentExpireList(string Agent, int Status)
        {
            _logInfo.Info(string.Format("获取快到期的代理人列表接口请求串：{0}", Request.RequestUri));
            var viewModel = new ExpireAgentViewModel();
            if (!Agent.Equals("yunying"))
            {
                return "非法账号".ResponseToJson();
            }
            var response = _agentService.GetAgentExpireList(Status);
            viewModel.List = response.ConverToExpireAgentViewModel();
            return viewModel.ResponseToJson();
        }
    }
}