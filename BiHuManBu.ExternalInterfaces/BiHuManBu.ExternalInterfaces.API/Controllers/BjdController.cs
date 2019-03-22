using BiHuManBu.ExternalInterfaces.API.Filters;
using BiHuManBu.ExternalInterfaces.Infrastructure;
using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Services.BjdServices.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Mapper;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using BiHuManBu.LogBuriedPoint.LogCollection;
using log4net;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BiHuManBu.ExternalInterfaces.API.Controllers
{
    public class BjdController : ApiController
    {
        private IBjdService _bjdService;
        private ILog logError;
        private ILog logInfo;
        private ILog _logAppInfo;
        private ICreateBjdInfoService _createBjdInfoService;
        private IGetBjdInfoService _bjdInfoService;


        public BjdController(IBjdService bjdService, ICreateBjdInfoService createBjdInfoService, IGetBjdInfoService bjdInfoService)
        {
            _bjdService = bjdService;
            _createBjdInfoService = createBjdInfoService;
            _bjdInfoService = bjdInfoService;
            logError = LogManager.GetLogger("ERROR");
            logInfo = LogManager.GetLogger("INFO");
            _logAppInfo = LogManager.GetLogger("APP");
        }


        /// <summary>
        /// 微信端，生成 手机报价单给别人看的
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public long UpdateBjdInfo([FromUri]CreateOrUpdateBjdInfoRequest request)
        {
            logInfo.Info(string.Format("分享报价单信息接口请求串：{0}", Request.RequestUri));
            var count = _createBjdInfoService.UpdateBjdInfo(request, Request.GetQueryNameValuePairs());//老代码在这里=>//var count = _bjdService.UpdateBjdInfo(request, Request.GetQueryNameValuePairs());//logInfo.Info(string.Format("分享报价单信息接口返回值：{0}", count.ToJson()));
            return count;
        }

        /// <summary>
        /// 微信端，查看别人分享出来的报价单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage Get([FromUri]GetBjdItemRequest request)
        {
            logInfo.Info(string.Format("获取报价单信息接口请求串：{0}", Request.RequestUri));
            var item = new BaojiaItemViewModel();
            if (!ModelState.IsValid)
            {
                item.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(a => a.Errors.Count == 1).Aggregate(string.Empty, (current, a) => current + (a.Errors[0].ErrorMessage + ";   "));
                item.StatusMessage = "输入参数错误，" + msg;
                return item.ResponseToJson();
            }
            try
            {
                var response = _bjdService.GetBjdInfo(request, Request.GetQueryNameValuePairs());
                item.BusinessStatus = 1;
                if (request.Test == 1)
                {//调试用
                    response.Baodanxinxi = null;
                }
                if (response.Baodanxinxi != null && response.Baodanxinxi.Id > 0)
                {
                    item = response.Baodanxinxi.ConvertToViewModel(response.Baodanxianzhong, response.ClaimDetail, response.Savequote, response.AgentDetail, response.Activitys);
                }
                else if(request.JieKouUrl==0)
                {//JieKouUrl为防止南北方都没数据循环调用
                    //南北方机房互相调用
                    var jifangConfig = ConfigurationManager.AppSettings["JiFangUrl"];
                    string jifangUrl = string.Empty;
                    if (jifangConfig != null)
                    {
                        jifangUrl = jifangConfig.ToString();
                    }
                    if (!string.IsNullOrEmpty(jifangUrl))
                    {
                        string geturl = string.Format("{0}/api/Bjd/Get?JieKouUrl=1&Bxid={1}", jifangUrl, request.Bxid);
                        string result = HttpWebAsk.HttpGet(geturl);
                        item = result.FromJson<BaojiaItemViewModel>();
                        return item.ResponseToJson();
                    }
                }
                logInfo.Info(string.Format("获取报价单信息接口返回值：{0}", item.ToJson()));
            }
            catch (Exception ex)
            {
                item.BusinessStatus = -10003;
                item.StatusMessage = "服务器发生异常";
                logError.Info("获取报价单信息单发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }

            return item.ResponseToJson();
        }
        /// <summary>
        /// 微信端，查看我的报价单列表
        /// 20170228账号统一，openid查询改为childagent查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetMyList([FromUri]GetMyBjdListRequest request)
        {
            logInfo.Info(string.Format("获取我的报价单列表接口请求串：{0}", Request.RequestUri));
            var viewModel = new BjdListViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(a => a.Errors.Count == 1).Aggregate(string.Empty, (current, a) => current + (a.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }
            try
            {
                int totalCount = 0;
                var response = _bjdService.GetMyList(request, Request.GetQueryNameValuePairs(), out totalCount);
                if (totalCount == 0)
                {
                    viewModel.BusinessStatus = -1;
                    viewModel.StatusMessage = "无报价信息";
                }
                else
                {
                    viewModel.BusinessStatus = 1;
                    viewModel.MyBaojiaList = response;
                    viewModel.TotalCount = totalCount;
                }
            }
            catch (Exception ex)
            {
                logError.Info("获取报价单列表接口发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return viewModel.ResponseToJson();
        }

        /// <summary>
        /// 微信端，查看我的报价单列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetQuoteInfoList([FromUri]GetMyBjdListRequest request)
        {
            logInfo.Info(string.Format("获取我的报价单列表接口请求串：{0}", Request.RequestUri));
            var viewModel = new BjdListViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(a => a.Errors.Count == 1).Aggregate(string.Empty, (current, a) => current + (a.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }
            try
            {
                int totalCount = 0;
                var response = _bjdService.GetMyList(request, Request.GetQueryNameValuePairs(), out totalCount);
                if (totalCount == 0)
                {
                    viewModel.BusinessStatus = -1;
                    viewModel.StatusMessage = "无报价信息";
                }
                else
                {
                    viewModel.BusinessStatus = 1;
                    viewModel.MyBaojiaList = response;
                    viewModel.TotalCount = totalCount;
                }
            }
            catch (Exception ex)
            {
                logError.Info("获取报价单列表接口发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return viewModel.ResponseToJson();
        }


        /// <summary>
        /// 微信端，查看我的报价单详情
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Log("报价", "获取报价", 2)]
        public HttpResponseMessage GetMyBjdDetail([FromUri]GetMyBjdDetailRequest request)
        {
            string traceId = LogAssistant.GetRequestHeaders(Request, "TraceId");
            BHFunctionLog fucnLog = LogAssistant.GenerateBHFuncLog(traceId, "service层获取获取我的报价单接口", "GetMyBjdDetail", 1);
            logInfo.Info(string.Format("获取我的报价单详情接口请求串：{0}", Request.RequestUri));
            var viewModel = new MyBaoJiaViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(a => a.Errors.Count == 1).Aggregate(string.Empty, (current, a) => current + (a.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }
            try
            {
                MyBaoJiaViewModel response = new MyBaoJiaViewModel();
                AspectF.Define.InfoFunctionLog(fucnLog).Do(() => { response = _bjdService.GetMyBjdDetail(request, Request.GetQueryNameValuePairs()); });
                viewModel = response;
            }
            catch (Exception ex)
            {
                logError.Info("获取报价单详情接口发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return viewModel.ResponseToJson();
        }

        /// <summary>
        /// APP端，查看我的续保列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //[AuthorizeFilter]
        [HttpGet]
        public HttpResponseMessage GetReInfoList([FromUri]GetReInfoListRequest request)
        {
            _logAppInfo.Info(string.Format("获取续保列表接口请求串：{0}", Request.RequestUri));
            var model = new ReInfoListViewModel();
            if (!ModelState.IsValid)
            {
                model.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(a => a.Errors.Count == 1).Aggregate(string.Empty, (current, a) => current + (a.Errors[0].ErrorMessage + ";   "));
                model.StatusMessage = "输入参数错误，" + msg;
                return model.ResponseToJson();
            }
            var response = _bjdService.GetReInfoList(request, Request.GetQueryNameValuePairs());
            //_logAppInfo.Info(string.Format("获取续保列表接口返回结果：{0}", response.ToJson()));
            model.BusinessStatus = response.BusinessStatus;
            model.StatusMessage = response.StatusMessage;
            model.TotalCount = response.TotalCount;
            model.ReInfoList = response.ReInfoList ?? new List<ReInfo>();
            return model.ResponseToJson();
        }

        /// <summary>
        /// APP端，查看我的续保详情
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //[AuthorizeFilter]
        [HttpGet]
        public HttpResponseMessage GetReInfoDetail([FromUri]GetReInfoDetailRequest request)
        {
            _logAppInfo.Info(string.Format("获取续保详情接口请求串：{0}", Request.RequestUri));
            var model = new AppReInfoViewModel();
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
            var response = _bjdService.GetReInfoDetail(request, Request.GetQueryNameValuePairs());
            //_logAppInfo.Info(string.Format("获取续保详情接口返回结果：{0}", response.ToJson()));
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
                if (model.SaveQuote != null)
                {
                    model.SaveQuote.Source = SourceGroupAlgorithm.GetNewSource((int)model.SaveQuote.Source);
                }
                model.WorkOrder = response.WorkOrder.ConverToViewModel();
                if (model.WorkOrder != null)
                {
                    if (model.WorkOrder.IntentionCompany.HasValue)
                        model.WorkOrder.IntentionCompany = SourceGroupAlgorithm.GetNewSource((int)model.WorkOrder.IntentionCompany.Value);
                }
                model.DetailList = response.DetailList.ConverToViewModel();
                //model.DetailList里的意向公司有问题
                model.IsDistrib = response.IsDistrib;
                model.Buid = response.Buid;
                model.Agent = response.Agent;
                model.AgentName = response.AgentName;
                model.SaAgent = response.SaAgent;
                model.SaAgentName = response.SaAgentName;

                //当获取续保信息失败（包括状态是3），将去年source制为-1，以免与 平安的0值混淆
                if (response.BusinessStatus == 1)
                {
                    return model.ResponseToJson();
                }
                if (model.SaveQuote != null)
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

        [HttpGet]
        public HttpResponseMessage BjdCountInfo([FromUri]BjdCountInfoRequest request)
        {
            logInfo.Info(string.Format("获取报价统计记录接口请求串：{0}", Request.RequestUri));
            var model = new BjdCountInfoViewModel();
            if (!ModelState.IsValid)
            {
                model.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(a => a.Errors.Count == 1).Aggregate(string.Empty, (current, a) => current + (a.Errors[0].ErrorMessage + ";   "));
                model.StatusMessage = "输入参数错误，" + msg;
                return model.ResponseToJson();
            }
            var response = _bjdService.BjdCountInfo(request, Request.GetQueryNameValuePairs());
            //logInfo.Info(string.Format("获取报价统计记录接口返回结果：{0}", response.ToJson()));
            //if (response.Status == HttpStatusCode.BadRequest || response.Status == HttpStatusCode.Forbidden)
            //{
            //    model.BusinessStatus = -10001;
            //    model.StatusMessage = "参数校验错误，请检查您的校验码";
            //    return model.ResponseToJson();
            //}
            //if (response.Status == HttpStatusCode.ExpectationFailed)
            //{
            //    model.BusinessStatus = -10003;
            //    model.StatusMessage = "服务发生异常";
            //    return model.ResponseToJson();
            //}
            model = response;
            return model.ResponseToJson();
        }

        /// <summary>
        /// 从报价历史取报价单详情 gpj 2018-07-18 /微信(百得利)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Log("报价", "获取报价", 2)]
        public HttpResponseMessage GetBjdDetailFromHistory([FromUri]GetBjdDetailFromHistoryRequest request)
        {
            logInfo.Info(string.Format("从历史获取报价详情接口请求串：{0}", Request.RequestUri));
            var viewModel = new MyBaoJiaViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(a => a.Errors.Count == 1).Aggregate(string.Empty, (current, a) => current + (a.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }
            try
            {
                MyBaoJiaViewModel response = new MyBaoJiaViewModel();
                response = _bjdService.GetBjdDetailFromHistory(request, Request.GetQueryNameValuePairs());
                viewModel = response;
                viewModel.BusinessStatus = 1;
            }
            catch (Exception ex)
            {
                logError.Info("获取报价单详情接口发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return viewModel.ResponseToJson();
        }
    }
}