using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BiHuManBu.ExternalInterfaces.Infrastructure;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using BiHuManBu.MessageCenter.API.RealTimeConnection;
using log4net;
using Microsoft.AspNet.SignalR;

namespace BiHuManBu.MessageCenter.API.Controllers
{
    public class MessageController : ApiController
    {
        private IMessageService _messageService;
        private ILog _logInfo;
        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
            _logInfo = LogManager.GetLogger("INFO"); 
        }

        [HttpPost]
        public HttpResponseMessage AddMessage([FromBody]AddMessageRequest request)
        {
            _logInfo.Info(string.Format("添加消息接口请求串：{0}", Request.RequestUri));
            var response = _messageService.AddMessage(request, Request.GetQueryNameValuePairs());

            var ctx = GlobalHost.ConnectionManager.GetHubContext<DrawingBoard>();
            if (request.MsgType == 0)
            {
                //如果是1系统消息，推送全部人
               // ctx.Clients.All.sendmessage(request.Title);//_messageService.PushMessage(request)
            }
            else
            {//否则，推送个体
                //ctx.Clients.User(request.ToAgentId.ToString()).sendmessage(request.Title);//_messageService.PushMessage(request)
            }

            return response.ResponseToJson();
        }

        [HttpGet]
        public HttpResponseMessage ReadMessage([FromUri]ReadMessageRequest request)
        {
            _logInfo.Info(string.Format("修改消息状态接口请求串：{0}", Request.RequestUri));
            BaseViewModel viewModel = new BaseViewModel();
            var response = _messageService.ReadMessage(request, Request.GetQueryNameValuePairs());
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
        public async  Task<HttpResponseMessage> MessageList([FromUri]MessageListRequest request)
        {
            var t = 1;
            var ctx = GlobalHost.ConnectionManager.GetHubContext<DrawingBoard>();
            var s = await ctx.Clients.User("102").sendmessage("哈哈，我进来了");
            //var s = await ctx.Clients.All.sendmessage("哈哈，我进来了");
            //_logInfo.Info(string.Format("消息列表获取接口请求串：{0}", Request.RequestUri));
            //MessageListViewModel viewModel = new MessageListViewModel();
            //var response = _messageService.MessageList(request, Request.GetQueryNameValuePairs());
            //viewModel.MsgList = response.MsgList;
            //viewModel.TotalCount = response.TotalCount;
            //return viewModel.ResponseToJson();
            return ((new BaseViewModel()).BusinessStatus = 1).ResponseToJson();
        }


    }
}
