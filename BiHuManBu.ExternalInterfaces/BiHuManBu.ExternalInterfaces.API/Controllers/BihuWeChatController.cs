using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BiHuManBu.ExternalInterfaces.Infrastructure;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Mapper;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using log4net;

namespace BiHuManBu.ExternalInterfaces.API.Controllers
{
    public class BihuWeChatController : ApiController
    {
        private IBjdService _bjdService;
        private ILog logError;
        private ILog logInfo;
        //
        // GET: /BihuWeChat/
        public BihuWeChatController(IBjdService bjdService)
        {
            _bjdService = bjdService;
            logError = LogManager.GetLogger("ERROR");
            logInfo = LogManager.GetLogger("INFO");
        }

        [HttpGet]
        public HttpResponseMessage GetReInfoDetail([FromUri]GetReInfoDetailRequest request)
        {
            logInfo.Info(string.Format("获取续保详情接口请求串：{0}", Request.RequestUri));
            var model = new AppReInfoViewModel();
            if (!ModelState.IsValid)
            {
                model.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(a => a.Errors.Count == 1).Aggregate(string.Empty, (current, a) => current + (a.Errors[0].ErrorMessage + ";   "));
                model.StatusMessage = "输入参数错误，" + msg;
                return model.ResponseToJson();
            }
            var response = _bjdService.GetReInfoDetail(request, Request.GetQueryNameValuePairs());
            //logInfo.Info(string.Format("获取续保详情接口返回结果：{0}", response.ToJson()));
            if (response.Status == HttpStatusCode.BadRequest || response.Status == HttpStatusCode.Forbidden)
            {
                model.BusinessStatus = -10001;
                model.StatusMessage = "参数校验错误，请检查您的校验码";
                return model.ResponseToJson();
            }
            if (response.Status == HttpStatusCode.ExpectationFailed)
            {
                model.BusinessStatus = -10003;
                model.StatusMessage = "服务发生异常";
                return model.ResponseToJson();
            }

            if (response.Status == HttpStatusCode.OK)
            {
                model.BusinessStatus = response.BusinessStatus == 8 ? 1 : response.BusinessStatus;
                model.StatusMessage = response.BusinessMessage;
                model.UserInfo = response.UserInfo.ConvertToViewModel(response.SaveQuote, response.CarInfo,
                    response.LastInfo);
                model.SaveQuote = response.SaveQuote.ConvetToViewModel();
                model.WorkOrder = response.WorkOrder.ConverToViewModel();
                model.DetailList = response.DetailList.ConverToViewModel();
                model.IsDistrib = response.IsDistrib;
                model.Buid = response.Buid;
                model.Agent = response.Agent;
                model.AgentName = response.AgentName;
                model.SaAgent = response.SaAgent;
                model.SaAgentName = response.SaAgentName;

                //当获取续保信息失败（包括状态是3），将去年source制为-1，以免与 平安的0值混淆
                if (response.BusinessStatus != 1)
                {
                    model.SaveQuote.Source = -1;
                }
            }
            else
            {
                model.BusinessStatus = -10002;
                model.StatusMessage = "没有续保信息";
            }
            return model.ResponseToJson();
        }

    }
}
