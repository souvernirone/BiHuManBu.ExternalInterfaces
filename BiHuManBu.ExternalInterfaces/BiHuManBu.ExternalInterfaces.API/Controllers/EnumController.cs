
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
using ServiceStack.Text;

namespace BiHuManBu.ExternalInterfaces.API.Controllers
{
    public class EnumController : ApiController
    {
        //
        // GET: /Enum/
        private IEnumService _enumService;
        private ILog _logInfo;
        public EnumController(IEnumService enumService)
        {
            _enumService = enumService;
            _logInfo = LogManager.GetLogger("INFO");
        }

        [HttpGet]
        public HttpResponseMessage GetParaBhType([FromUri]GetParaBhTypeRequest request)
        {
            _logInfo.Info(string.Format("获取枚举类型接口请求串：{0}", Request.RequestUri));
            var viewModel = new GetParaBhTypeViewModel();

            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }
            var response = _enumService.GetParaBhType(request, Request.GetQueryNameValuePairs());
            _logInfo.Info("获取枚举类型接口返回值" + response.ToJson());
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
                viewModel.StatusMessage = "无枚举类型";
            }
            else
            {
                viewModel.BusinessStatus = 1;
                viewModel.StatusMessage = "获取成功";
                viewModel.ParaBhType = response.ParaBhTypeList.ConvertToViewModel();
            }

            return viewModel.ResponseToJson();
        }

    }
}
