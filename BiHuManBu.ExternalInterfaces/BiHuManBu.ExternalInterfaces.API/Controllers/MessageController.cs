using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BiHuManBu.ExternalInterfaces.Infrastructure;
using BiHuManBu.ExternalInterfaces.Services;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Mapper;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using log4net;
using ServiceStack.Text;

namespace BiHuManBu.ExternalInterfaces.API.Controllers
{
    [AuthorizeFilter]
    public class MessageController : ApiController
    {
        private IMessageService _messageService;
        private ILog _logAppInfo;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
            _logAppInfo = LogManager.GetLogger("APP");
        }

        [HttpPost]
        public HttpResponseMessage AddMessage([FromBody]AddMessageRequest request)
        {
            _logAppInfo.Info(string.Format("添加消息接口请求串：{0}，参数：{1}", Request.RequestUri, request.ToJson()));
            var response = _messageService.AddMessage(request, Request.GetQueryNameValuePairs());
            _logAppInfo.Info(string.Format("添加消息接口返回值：{0}", response.ToJson()));
            return response.ResponseToJson();
        }

        [HttpGet]
        public HttpResponseMessage ReadMessage([FromUri]ReadMessageRequest request)
        {
            _logAppInfo.Info(string.Format("修改消息状态接口请求串：{0}", Request.RequestUri));
            BaseViewModel viewModel = new BaseViewModel();
            var response = _messageService.ReadMessage(request, Request.GetQueryNameValuePairs());
            _logAppInfo.Info(string.Format("修改消息状态接口返回值：{0}", response.ToJson()));
            if (response.Status == HttpStatusCode.OK)
            {
                viewModel.BusinessStatus = 1;
            }
            else
            {
                viewModel.BusinessStatus = 0;
            }
            return viewModel.ResponseToJson();
        }

        [HttpGet]
        public HttpResponseMessage MessageList([FromUri]MessageListRequest request)
        {
            _logAppInfo.Info(string.Format("消息列表获取接口请求串：{0}", Request.RequestUri));
            MessageListViewModel viewModel = new MessageListViewModel();
            var response = _messageService.MessageListNew(request, Request.GetQueryNameValuePairs());
            viewModel.BusinessStatus = response.ErrCode;
            viewModel.StatusMessage = response.ErrMsg;
            viewModel.MsgList = response.MsgList;
            viewModel.TotalCount = response.TotalCount;
            return viewModel.ResponseToJson();
        }

        [HttpGet]
        public HttpResponseMessage MsgClosedOrder([FromUri]MsgClosedOrderRequest request)
        {
            _logAppInfo.Info(string.Format("已出单详情接口请求串：{0}", Request.RequestUri));
            MsgClosedOrderViewModel viewModel = new MsgClosedOrderViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }
            var response = _messageService.MsgClosedOrder(request, Request.GetQueryNameValuePairs());
            _logAppInfo.Info(string.Format("已出单详情接口返回值：{0}", response.ToJson()));
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
            viewModel.BusinessStatus = response.ErrCode;
            viewModel.StatusMessage = response.ErrMsg;
            if (response.ErrCode == 1)
            {
                viewModel.LicenseNo = response.LicenseNo;
                viewModel.MoldName = response.MoldName;
                viewModel.ReviewContent = response.ReviewContent;
                viewModel.SaAgent = response.SaAgent;
                viewModel.SaAgentName = response.SaAgentName;
                viewModel.AdvAgent = response.AdvAgent;
                viewModel.AdvAgentName = response.AdvAgentName;
                viewModel.SourceName = response.SourceName;
            }
            return viewModel.ResponseToJson();
        }

        [HttpGet]
        public HttpResponseMessage LastDayReInfoTotal([FromUri]LastDayReInfoTotalRequest request)
        {
            _logAppInfo.Info(string.Format("续保统计接口请求串：{0}", Request.RequestUri));
            LastDayReInfoTotalViewModel viewModel=new LastDayReInfoTotalViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }
            var response = _messageService.LastDayReInfoTotal(request, Request.GetQueryNameValuePairs());
            _logAppInfo.Info(string.Format("续保统计接口返回值：{0}", response.ToJson()));
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
            viewModel.BusinessStatus = response.ErrCode;
            viewModel.StatusMessage = response.ErrMsg;
            if (response.ErrCode == 1)
            {
                viewModel.InStoreNum = response.InStoreNum;
                viewModel.ExpireNum = response.ExpireNum;
                viewModel.IntentionNum = response.IntentionNum;
                viewModel.OrderNum = response.OrderNum;
                viewModel.ReInfo = response.ReInfo.ConvertViewModel();
            }
            return viewModel.ResponseToJson();
        }

    }
}
