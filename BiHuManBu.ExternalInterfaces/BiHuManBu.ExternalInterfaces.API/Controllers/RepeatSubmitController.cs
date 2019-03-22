using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BiHuManBu.ExternalInterfaces.Infrastructure;
using BiHuManBu.ExternalInterfaces.Services.Mapper;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.RepeatSubmitService.interfaces;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using log4net;
using WebApiThrottle;

namespace BiHuManBu.ExternalInterfaces.API.Controllers
{
    public class RepeatSubmitController : ApiController
    {
        private readonly ILog _logInfo;
        private readonly IRepeatSubmitService _repeatSubmitService;

        public RepeatSubmitController(IRepeatSubmitService repeatSubmitService)
        {
            _repeatSubmitService = repeatSubmitService;
            _logInfo = LogManager.GetLogger("INFO");
        }

        /// <summary>
        ///     获取核保信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet, ActionName("GetRepeatSubmitDetail")]
        [EnableCommonThrottling(PerSecond = 5, PerMinute = 20, PerHour = 100)]
        public async Task<HttpResponseMessage> GetRepeatSubmitDetail([FromUri] GetRepeatSubmitRequest request)
        {
            _logInfo.Info(string.Format("获取重复投保请求串：{0}", Request.RequestUri));
            var viewModel = new GetRepeatSubmitViewModel();
            viewModel.RepeatInfo = new GetRepeatSubmitInfo
            {
                BusinessExpireDate = string.Empty,
                ForceExpireDate = string.Empty,
                RepeatSubmitResult = -1,
                RepeatSubmitMessage = string.Empty
            };

            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                var msg = ModelState.Values.Where(item => item.Errors.Count == 1)
                    .Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }
            var response = await _repeatSubmitService.GetRepeatSubmitInfo(request, Request.GetQueryNameValuePairs());
            if (response.Status == HttpStatusCode.BadRequest || response.Status == HttpStatusCode.Forbidden)
            {
                viewModel.BusinessStatus = -10001;
                viewModel.StatusMessage = "参数校验错误，请检查您的校验码";
                return viewModel.ResponseToJson();
            }
            if (response.Status == HttpStatusCode.ExpectationFailed)
            {
                viewModel.BusinessStatus = -10003;
                viewModel.StatusMessage = "服务器发生异常";
                return viewModel.ResponseToJson();
            }
            if (response.Status == HttpStatusCode.NoContent)
            {
                viewModel.BusinessStatus = -10004;
                viewModel.StatusMessage = "您还未报价";
                return viewModel.ResponseToJson();
            }
            if (response.Status == HttpStatusCode.Conflict)
            {
                viewModel.BusinessStatus = -10005;
                viewModel.StatusMessage = "您的报价请求还在处理阶段,请结束后再调用此接口（每次报价结束后，请求一次该接口即可，不需要重复调用）";
                return viewModel.ResponseToJson();
            }

            viewModel = response.RepeatSubmitConvertToViewModel();
            viewModel.BusinessStatus = 1;
            viewModel.StatusMessage = "请求成功";

            return viewModel.ResponseToJson();
        }
    }
}