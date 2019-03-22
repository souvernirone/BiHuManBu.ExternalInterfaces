
using System.Collections.Generic;
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
    public class ChargingSystemController : ApiController
    {
        //
        // GET: /ChargingSystem/
        private ILog _logInfo;
        private ILog _logError;

        private IChargingSystemService _chargingSystemService;

        public ChargingSystemController(IChargingSystemService chargingSystemService)
        {
            _logInfo = LogManager.GetLogger("INFO");
            _logError = LogManager.GetLogger("ERROR");
            _chargingSystemService = chargingSystemService;
        }

        [HttpGet]
        public HttpResponseMessage GetReInfoList([FromUri]GetReInfoListRequest request)
        {
            _logInfo.Info(string.Format("获取续保列表接口请求串：{0}", Request.RequestUri));
            var model = new ReListViewModel();
            if (!ModelState.IsValid)
            {
                model.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(a => a.Errors.Count == 1).Aggregate(string.Empty, (current, a) => current + (a.Errors[0].ErrorMessage + ";   "));
                model.StatusMessage = "输入参数错误，" + msg;
                return model.ResponseToJson();
            }
            var response = _chargingSystemService.GetReInfoList(request, Request.GetQueryNameValuePairs());
            //_logAppInfo.Info(string.Format("获取续保列表接口返回结果：{0}", response.ToJson()));
            model.BusinessStatus = response.BusinessStatus;
            model.StatusMessage = response.StatusMessage;
            model.TotalCount = response.TotalCount;
            model.ReList = response.ReList ?? new List<Re>();
            return model.ResponseToJson();
        }

        public HttpResponseMessage GetReInfoDetails([FromUri]GetReInfoDetailRequest request)
        {
            _logInfo.Info(string.Format("获取续保详情接口请求串：{0}", Request.RequestUri));
            var model = new GetReInfoViewModel();
            if (!ModelState.IsValid)
            {
                model.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(a => a.Errors.Count == 1).Aggregate(string.Empty, (current, a) => current + (a.Errors[0].ErrorMessage + ";   "));
                model.StatusMessage = "输入参数错误，" + msg;
                return model.ResponseToJson();
            }
            if (!request.LicenseNo.IsValidLicenseno())
            {
                model.BusinessStatus = -10000;
                model.StatusMessage = "参数校验错误，请检查车牌号";
                return model.ResponseToJson();
            }
            var response = _chargingSystemService.GetReInfoDetail(request, Request.GetQueryNameValuePairs());
            //_logInfo.Info(string.Format("获取续保详情接口返回结果：{0}", response.ToJson()));
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
                model.BusinessStatus = response.BusinessStatus;
                model.StatusMessage = response.BusinessMessage;
                model.UserInfo = response.UserInfo.ConvertToViewModel(response.SaveQuote, response.CarInfo,
                    response.LastInfo);
                if (response.BusinessStatus == 1)
                {
                    model.SaveQuote = response.SaveQuote.ConvetToViewModel();
                }
                else
                {
                    model.UserInfo.BusinessExpireDate = "";
                    model.UserInfo.ForceExpireDate = "";
                    model.UserInfo.NextBusinessStartDate = "";
                    model.UserInfo.NextForceStartDate = "";
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
