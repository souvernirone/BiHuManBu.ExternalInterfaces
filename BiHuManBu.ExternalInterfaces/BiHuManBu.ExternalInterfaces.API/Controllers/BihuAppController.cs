using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BiHuManBu.ExternalInterfaces.Infrastructure;
using BiHuManBu.ExternalInterfaces.Services;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using log4net;

namespace BiHuManBu.ExternalInterfaces.API.Controllers
{
    //[AuthorizeFilter]
    public class BihuAppController : ApiController
    {
        private IAppAchieveService _appAchieveService;
        private ILog _logAppInfo;

        public BihuAppController(IAppAchieveService appAchieveService)
        {
            _appAchieveService = appAchieveService;
            _logAppInfo = LogManager.GetLogger("APP");
        }
        
        #region 续保、报价、核保 相关
        /// <summary>
        /// 请求续保
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<HttpResponseMessage> GetReInfo([FromUri]GetReInfoRequest request)
        {
            _logAppInfo.Info(string.Format("请求续保接口请求串：{0}", Request.RequestUri));
            var viewModel = new GetReInfoNewViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(a => a.Errors.Count == 1).Aggregate(string.Empty, (current, a) => current + (a.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }
            viewModel = await _appAchieveService.GetReInfo(request, Request.GetQueryNameValuePairs(), Request.RequestUri);
            //_logAppInfo.Info(string.Format("请求续保接口返回值：{0}", viewModel.ToJson()));
            return viewModel.ResponseToJson();
        }

        /// <summary>
        /// 请求报价/核保
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<HttpResponseMessage> PostPrecisePrice([FromUri]PostPrecisePriceRequest request)
        {
            _logAppInfo.Info(string.Format("请求报价/核保接口请求串：{0}", Request.RequestUri));
            var viewModel = new BaseViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }
            viewModel = await _appAchieveService.PostPrecisePrice(request, Request.GetQueryNameValuePairs(), Request.RequestUri);
            //_logAppInfo.Info(string.Format("请求报价/核保接口返回值：{0}", viewModel.ToJson()));
            return viewModel.ResponseToJson();
        }
        
        /// <summary>
        /// 获取报价信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<HttpResponseMessage> GetSpecialPrecisePrice([FromUri]GetPrecisePriceRequest request)
        {
            _logAppInfo.Info(string.Format("获取报价信息接口请求串：{0}", Request.RequestUri));
            var viewModel = new GetPrecisePriceNewViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }
            //此处调用未带special的方法，是因为该service返回值已经增加了buid
            viewModel = await _appAchieveService.GetPrecisePrice(request, Request.GetQueryNameValuePairs(), Request.RequestUri);
            //_logAppInfo.Info(string.Format("获取报价信息接口返回值：{0}", viewModel.ToJson()));
            return viewModel.ResponseToJson();
        }

        /// <summary>
        /// 获取核保信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<HttpResponseMessage> GetSubmitInfo([FromUri]GetSubmitInfoRequest request)
        {
            _logAppInfo.Info(string.Format("获取核保信息接口请求串：{0}", Request.RequestUri));
            var viewModel = new SubmitInfoNewViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }
            viewModel = await _appAchieveService.GetSubmitInfo(request, Request.GetQueryNameValuePairs(), Request.RequestUri);
            //_logAppInfo.Info(string.Format("获取核保信息接口返回值：{0}", viewModel.ToJson()));
            return viewModel.ResponseToJson();
        }

        /// <summary>
        /// 获取车辆出险信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<HttpResponseMessage> GetCreditInfo([FromUri]GetEscapedInfoRequest request)
        {
            _logAppInfo.Info(string.Format("获取车辆出险信息接口请求串：{0}", Request.RequestUri));
            var viewModel = new GetCreaditInfoViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }
            viewModel = await _appAchieveService.GetCreditInfo(request, Request.GetQueryNameValuePairs(), Request.RequestUri);
            //_logAppInfo.Info(string.Format("获取车辆出险信息接口返回值：{0}", viewModel.ToJson()));
            return viewModel.ResponseToJson();
        }

        /// <summary>
        /// 获取车型信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<HttpResponseMessage> GetVehicleInfo([FromUri]GetCarVehicleRequest request)
        {
            _logAppInfo.Info(string.Format("获取车型信息接口请求串：{0}", Request.RequestUri));
            var viewModel = new CarVehicleInfoNewViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }
            viewModel = await _appAchieveService.GetVehicleInfo(request, Request.GetQueryNameValuePairs(), Request.RequestUri);
            //_logAppInfo.Info(string.Format("获取车辆出险信息接口返回值：{0}", viewModel.ToJson()));
            return viewModel.ResponseToJson();
        }
        /// <summary>
        /// 车型报价校验
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<HttpResponseMessage> CheckVehicle([FromUri]GetNewCarSecondVehicleRequest request)
        {
            _logAppInfo.Info(string.Format("车型报价校验接口请求串：{0}", Request.RequestUri));
            var viewModel = new CheckCarVehicleInfoViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }
            viewModel = await _appAchieveService.CheckVehicle(request, Request.GetQueryNameValuePairs(), Request.RequestUri);
            //_logAppInfo.Info(string.Format("获取车辆出险信息接口返回值：{0}", viewModel.ToJson()));
            return viewModel.ResponseToJson();
        }
        #endregion

        #region 报价、续保的列表、详情
        /// <summary>
        /// 获取报价单、续保列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetMyList([FromUri]GetMyListRequest request)
        {
            _logAppInfo.Info(string.Format("请求报价续保列表接口请求串：{0}", Request.RequestUri));
            var viewModel = new MyListViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(a => a.Errors.Count == 1).Aggregate(string.Empty, (current, a) => current + (a.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }
            viewModel = _appAchieveService.GetMyList(request, Request.GetQueryNameValuePairs());
            //_logAppInfo.Info(string.Format("请求报价续保列表接口返回值：{0}", viewModel.ToJson()));
            return viewModel.ResponseToJson();
        }

        /// <summary>
        /// 报价单详情
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetPrecisePriceDetail([FromUri]GetMyBjdDetailRequest request)
        {
            _logAppInfo.Info(string.Format("获取报价详情接口请求串：{0}", Request.RequestUri));
            var viewModel = new MyBaoJiaViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(a => a.Errors.Count == 1).Aggregate(string.Empty, (current, a) => current + (a.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }
            viewModel = _appAchieveService.GetPrecisePriceDetail(request, Request.GetQueryNameValuePairs(), Request.RequestUri);
            //_logAppInfo.Info(string.Format("获取报价详情接口返回值：{0}", viewModel.ToJson()));
            return viewModel.ResponseToJson();
        }

        /// <summary>
        /// 续保详情
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetReInfoDetail([FromUri]GetReInfoRequest request)
        {
            _logAppInfo.Info(string.Format("获取续保详情接口请求串：{0}", Request.RequestUri));
            var viewModel = new GetReInfoNewViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(a => a.Errors.Count == 1).Aggregate(string.Empty, (current, a) => current + (a.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }
            viewModel = _appAchieveService.GetReInfoDetail(request, Request.GetQueryNameValuePairs(), Request.RequestUri);
            //_logAppInfo.Info(string.Format("获取续保详情接口返回值：{0}", viewModel.ToJson()));
            return viewModel.ResponseToJson();
        }
        #endregion

        #region 分享报价单
        /// <summary>
        /// 生成报价单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage SharePrecisePrice([FromUri]CreateOrUpdateBjdInfoRequest request)
        {
            _logAppInfo.Info(string.Format("分享报价单接口请求串：{0}", Request.RequestUri));
            var viewModel = new BaseViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(a => a.Errors.Count == 1).Aggregate(string.Empty, (current, a) => current + (a.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }
            viewModel = _appAchieveService.SharePrecisePrice(request, Request.GetQueryNameValuePairs(), Request.RequestUri);
            //_logAppInfo.Info(string.Format("分享报价单接口返回值：{0}", viewModel.ToJson()));
            return viewModel.ResponseToJson();
        }

        /// <summary>
        /// 查看被分享的报价单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetShare([FromUri]GetBjdItemRequest request)
        {
            _logAppInfo.Info(string.Format("获取分享接口请求串：{0}", Request.RequestUri));
            var viewModel = new BaojiaItemViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(a => a.Errors.Count == 1).Aggregate(string.Empty, (current, a) => current + (a.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }
            viewModel = _appAchieveService.GetShare(request, Request.GetQueryNameValuePairs(), Request.RequestUri);
            //_logAppInfo.Info(string.Format("获取分享接口返回值：{0}", viewModel.ToJson()));
            return viewModel.ResponseToJson();
        }
        #endregion

        #region 回访记录
        [HttpPost]
        public HttpResponseMessage AddReVisited([FromBody]AddReVisitedRequest request)
        {
            _logAppInfo.Info(string.Format("添加回访记录接口请求串：{0}", Request.RequestUri));
            var viewModel = new BaseViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }
            viewModel = _appAchieveService.AddReVisited(request, Request.GetQueryNameValuePairs());
            //_logAppInfo.Info(string.Format("添加回访记录列表接口返回值：{0}", viewModel.ToJson()));
            return viewModel.ResponseToJson();
        }

        /// <summary>
        /// 回访记录列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage ReVisitedList([FromUri]ReVisitedListRequest request)
        {
            _logAppInfo.Info(string.Format("获取回访记录列表接口请求串：{0}", Request.RequestUri));
            var viewModel = new ReVisitedListViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }
            viewModel = _appAchieveService.ReVisitedList(request, Request.GetQueryNameValuePairs());
            //_logAppInfo.Info(string.Format("获取回访记录列表接口返回值：{0}", viewModel.ToJson()));
            return viewModel.ResponseToJson();
        }
        #endregion

        #region 系统基础信息
        /// <summary>
        /// 获取顶级代理的渠道资源
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetAgentSource([FromUri]AppBaseRequest request)
        {
            _logAppInfo.Info(string.Format("获取代理渠道列表接口请求串：{0}", Request.RequestUri));
            var viewModel = new AppAgentSourceViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }
            viewModel = _appAchieveService.GetAgentSource(request, Request.GetQueryNameValuePairs(), Request.RequestUri);
            //_logAppInfo.Info(string.Format("获取代理渠道列表接口返回值：{0}", viewModel.ToJson()));
            return viewModel.ResponseToJson();
        }
        
        #endregion
    }
}
