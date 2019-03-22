using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BiHuManBu.ExternalInterfaces.Infrastructure;
using BiHuManBu.ExternalInterfaces.Services;
using BiHuManBu.ExternalInterfaces.Services.IndependentService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.IndependentService.Mapper;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Mapper;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using BiHuManBu.ExternalInterfaces.Services.ViewModels.IndependentViewModel;
using BiHuManBu.ExternalInterfaces.Services.ViewModels.IndependentViewModel.FloatingInfoModel;
using log4net;
using ServiceStack.Text;

namespace BiHuManBu.ExternalInterfaces.API.Controllers
{
    public class IndependentController : ApiController
    {
        private readonly IGetSpecialAssumpsitService _getSpecialAssumpsitService;
        private readonly IGetFloatingInfoService _getFloatingInfoService;
        private readonly IPostIndependentSubmitService _postIndependentSubmitService;
        private readonly ILog _logInfo = LogManager.GetLogger("INFO");
        private readonly ILog _logError = LogManager.GetLogger("ERRO");
        public IndependentController(IGetSpecialAssumpsitService getSpecialAssumpsitService,
            IGetFloatingInfoService getFloatingInfoService,
            IPostIndependentSubmitService postIndependentSubmitService)
        {
            _getSpecialAssumpsitService = getSpecialAssumpsitService;
            _getFloatingInfoService = getFloatingInfoService;
            _postIndependentSubmitService = postIndependentSubmitService;
        }

        /// <summary>
        /// 获取特约检索信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet, ActionName("GetSpecialAssumpsit")]
        public async Task<HttpResponseMessage> GetSpecialAssumpsit([FromUri]GetSpecialAssumpsitRequest request)
        {
            _logInfo.Info(string.Format("获取特约检索请求串：{0}", Request.RequestUri));
            var viewModel = new GetSpecialAssumpsitViewModel();
            viewModel.SpecialContents = new List<SpecialVO>();
            try
            {
                if (!ModelState.IsValid)
                {
                    viewModel.BusinessStatus = -10000;
                    string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                    viewModel.StatusMessage = "输入参数错误，" + msg;
                    return viewModel.ResponseToJson();
                }
                if (!request.LicenseNo.IsValidLicenseno())
                {
                    viewModel.BusinessStatus = -10000;
                    viewModel.StatusMessage = "参数校验错误，请检查车牌号";
                    return viewModel.ResponseToJson();
                }
                GetSpecialAssumpsitResponse response = await _getSpecialAssumpsitService.GetSpecialAssumpsit(request, Request.GetQueryNameValuePairs());
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
                if (response.SpecialContents == null)
                {
                    viewModel.BusinessStatus = 0;
                    viewModel.StatusMessage = "无数据";
                }
                else
                {
                    viewModel.BusinessStatus = 1;
                    viewModel.StatusMessage = "获取成功";
                    viewModel.SpecialContents = response.SpecialContents;
                }
            }
            catch (Exception ex)
            {
                viewModel.BusinessStatus = -100003;
                viewModel.StatusMessage = "服务发生异常";
                _logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException + " 请求对象：" + Request.RequestUri);
            }
            return viewModel.ResponseToJson();
        }

        /// <summary>
        /// 获取浮动告知单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet, ActionName("GetFloatingInfo")]
        public async Task<HttpResponseMessage> GetFloatingInfo([FromUri]GetFloatingInfoRequest request)
        {
            _logInfo.Info(string.Format("获取浮动告知单请求串：{0}", Request.RequestUri));
            var viewModel = new GetFloatingInfoViewModel();
            try
            {
                if (!ModelState.IsValid)
                {
                    viewModel.BusinessStatus = -10000;
                    string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                    viewModel.StatusMessage = "输入参数错误，" + msg;
                    return viewModel.ResponseToJson();
                }
                if (!request.LicenseNo.IsValidLicenseno()) {
                    viewModel.BusinessStatus = -10000;
                    viewModel.StatusMessage = "参数校验错误，请检查车牌号";
                    return viewModel.ResponseToJson();
                }
                GetFloatingInfoResponse response = await _getFloatingInfoService.GetFloatingInfo(request, Request.GetQueryNameValuePairs());
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
                //模型转换
                viewModel = response.JSFloatingNotificationPrintList.ConverToViewModel();
                if (response.JSFloatingNotificationPrintList==null)
                {
                    viewModel.BusinessStatus = 0;
                    viewModel.StatusMessage = "无数据";
                }
                else
                {
                    viewModel.BusinessStatus = 1;
                    viewModel.StatusMessage = "获取成功";
                }
            }
            catch (Exception ex)
            {
                viewModel.BusinessStatus = -100003;
                viewModel.StatusMessage = "服务发生异常";
                _logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException + " 请求对象：" + Request.RequestUri);
            }
            return viewModel.ResponseToJson();
        }
        
        /// <summary>
        /// 安心请求核保
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet, ActionName("PostIndependentSubmit")]
        public async Task<HttpResponseMessage> PostIndependentSubmit([FromUri]PostIndependentSubmitRequest request)
        {
            BaseViewModel viewModel = new BaseViewModel();
            //基础校验
            _logInfo.Info(string.Format("安心请求核保接口请求串：{0}", Request.RequestUri));
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson(HttpStatusCode.Accepted);
            }
            if (!request.LicenseNo.IsValidLicenseno())
            {
                viewModel.BusinessStatus = -10000;
                viewModel.StatusMessage = "参数校验错误，请检查车牌号";
                return viewModel.ResponseToJson();
            }
            if (request.ChildAgent == 0)
            {
                request.ChildAgent = request.Agent;
            }
            //主体函数
            BaseResponse response = await _postIndependentSubmitService.PostIndependentSubmit(request, Request.GetQueryNameValuePairs());
            //返回值判断
            if (response.Status == HttpStatusCode.Forbidden)
            {
                viewModel.BusinessStatus = -10001;
                viewModel.StatusMessage = !string.IsNullOrWhiteSpace(response.ErrMsg) ? response.ErrMsg : "参数校验错误，请检查您的校验码";
                return viewModel.ResponseToJson(HttpStatusCode.Accepted);
            }
            //if (response.Status == HttpStatusCode.NotAcceptable)
            //{
            //    viewModel.BusinessStatus = -10005;
            //    viewModel.StatusMessage = "参数校验错误，请确认您之前在该渠道报价/核保过";
            //    return viewModel.ResponseToJson(HttpStatusCode.Accepted);
            //}
            if (response.Status == HttpStatusCode.ExpectationFailed)
            {
                viewModel.BusinessStatus = -10003;
                viewModel.StatusMessage = "服务发生异常";
                return viewModel.ResponseToJson(HttpStatusCode.Accepted);
            }
            //请求成功，返回值
            viewModel.BusinessStatus = 1;
            viewModel.StatusMessage = "请求发送成功";
            return viewModel.ResponseToJson();
        }

    }
}
