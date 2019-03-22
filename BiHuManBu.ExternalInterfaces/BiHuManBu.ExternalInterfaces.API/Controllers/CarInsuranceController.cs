using BiHuManBu.DsitributedMonitor;
using BiHuManBu.ExternalInterfaces.API.Filters;
using BiHuManBu.ExternalInterfaces.Infrastructure;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Services;
using BiHuManBu.ExternalInterfaces.Services.GetCarInfoService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.GetReInfoService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Mapper;
using BiHuManBu.ExternalInterfaces.Services.Messages.RemoteMessage;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.PrecisePriceService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.SubmitInfoService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using BiHuManBu.ExternalInterfaces.Services.ViewModels.CarVehicle;
using BiHuManBu.LogBuriedPoint.LogCollection;
using log4net;
using Metrics;
using Metrics.InfluxDB;
using Metrics.InfluxDB.Adapters;
using MetricsLibrary;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using WebApiThrottle;

namespace BiHuManBu.ExternalInterfaces.API.Controllers
{
    /// <summary>
    /// 车险
    /// </summary>
    [DisableThrotting()]
    public class CarInsuranceController : ApiController
    {
        private static readonly Dictionary<long, int> _suportedComDictionary = new Dictionary<long, int>() { { 1, 1 }, { 2, 0 }, { 4, 2 }, { 8, 3 }, { 16, 4 }, { 32, 5 }, { 64, 6 }, { 128, 7 }, { 256, 8 }, { 512, 9 }, { 1024, 10 }, { 2048, 11 }, { 4096, 12 }, { 8192, 13 }, { 34359738368, 35 } };
        private readonly Timer timer = Metric.Timer("renewal_timer", Unit.Requests);
        private readonly ILog logInfo;
        private readonly ILog logError;
        private static readonly string _baoxianCenter = ConfigurationManager.AppSettings["baoxianCenterApi"];
        private ICarInsuranceService _carInsuranceService;
        private readonly IPostSubmitInfoService _postSubmitInfoService;
        private readonly ICheckRequestPostPrecisePrice _checkRequestPostPrecisePrice;
        private readonly IRenewalStatusService _renewalStatusService;
        private readonly ITempDemoShowService _tempDemoShowService;
        private readonly IRecordNewCarService _recordNewCarService;
        private readonly IGetIntelligentReInfoService _getIntelligentReInfoService;
        private readonly ICheckReInfoService _checkReInfoService;
        private readonly IGetNewVehicleInfoService _getNewVehicleInfoService;
        private readonly IGetFirstVehicleInfoService _getFirstVehicleInfoService;
        private readonly IGetSecondVehicleInfoService _getSecondVehicleInfoService;
        private readonly IPackagingResponseService _packagingResponseService;
        public CarInsuranceController(ICarInsuranceService carInsuranceService, IPostSubmitInfoService postSubmitInfoService, ICheckRequestPostPrecisePrice checkRequestPostPrecisePrice,
            IRenewalStatusService renewalStatusService, ITempDemoShowService tempDemoShowService,
            IRecordNewCarService recordNewCarService, IGetIntelligentReInfoService getIntelligentReInfoService, ICheckReInfoService checkReInfoService,
            IGetNewVehicleInfoService getNewVehicleInfoService, IGetFirstVehicleInfoService getFirstVehicleInfoService, IGetSecondVehicleInfoService getSecondVehicleInfoService,
            IPackagingResponseService packagingResponseService)
        {
            logInfo = LogManager.GetLogger("INFO");
            logError = LogManager.GetLogger("ERROR");
            _carInsuranceService = carInsuranceService;
            _postSubmitInfoService = postSubmitInfoService;
            _checkRequestPostPrecisePrice = checkRequestPostPrecisePrice;
            _renewalStatusService = renewalStatusService;
            _tempDemoShowService = tempDemoShowService;
            _recordNewCarService = recordNewCarService;
            _getIntelligentReInfoService = getIntelligentReInfoService;
            _checkReInfoService = checkReInfoService;
            _getNewVehicleInfoService = getNewVehicleInfoService;
            _getFirstVehicleInfoService = getFirstVehicleInfoService;
            _getSecondVehicleInfoService = getSecondVehicleInfoService;
            _packagingResponseService = packagingResponseService;
        }

        //续保接口
        [HttpGet]
        [Log("续保", "获取续保信息", 1)]
        [ActionName("GetReInfo")]
        [EnableThrottling()]
        public async Task<HttpResponseMessage> FetchReInsuranceInfo([FromUri]GetReInfoRequest request)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("续保接口请求串：{0} {1} ", Request.RequestUri, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

            var _address = ConfigurationManager.AppSettings["MetricsAddress"];
            var _db = ConfigurationManager.AppSettings["MetricsDbName"];
            if (!string.IsNullOrWhiteSpace(_address) && !string.IsNullOrWhiteSpace(_db))
            {
                Metric.Config
                .WithReporting(report => report
                    .WithInfluxDbHttp(_address, _db, TimeSpan.FromSeconds(2), null, c => c
                           .WithConverter(new DefaultConverter().WithGlobalTags("host=" + Dns.GetHostAddresses(Dns.GetHostName()).LastOrDefault() + ",env=dev"))
                                   .WithFormatter(new DefaultFormatter().WithLowercase(true))
                                            .WithWriter(new InfluxdbHttpWriter(c, 1))));
            }
            sb.AppendLine(string.Format(" 添加日志记录：{0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            request.YZMArea = null;
            if (!string.IsNullOrEmpty(request.Point))
            { request.YZMArea = request.Point.FromJson<List<PointFloat>>(); }
            string traceId = LogAssistant.GetRequestHeaders(Request, "TraceId");
            BHFunctionLog fucnLog = LogAssistant.GenerateBHFuncLog(traceId, "检测请求字符串", "CheckXuBao", 1);
            //AddTrace(Request);// 埋点
            sb.AppendLine(string.Format(" 开始续保：{0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            Stopwatch sw = new Stopwatch();
            sw.Start();

            int viewCityCode = request.CityCode;
            string viewCustkey = request.CustKey;
            GetReInfoViewModel viewModel = new GetReInfoViewModel();
            using (timer.NewContext())
            {
                if (!ModelState.IsValid)
                {
                    viewModel.BusinessStatus = -10000;
                    string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                    viewModel.StatusMessage = "输入参数错误，" + msg;
                    return viewModel.ResponseToJson(HttpStatusCode.Accepted);
                }
                //顶级代理人赋值保存
                int topAgent = request.Agent;
                string checkMsg = "";
                AspectF.Define.InfoFunctionLog(fucnLog).Do(() => { checkMsg = _checkReInfoService.CheckXuBao(request); if (string.IsNullOrWhiteSpace(checkMsg)) { fucnLog.FunctionLogMsg = "请求参数验证通过"; } });
                if (!string.IsNullOrWhiteSpace(checkMsg))
                {
                    viewModel.BusinessStatus = -10000;
                    string msg = checkMsg;
                    viewModel.StatusMessage = "输入参数错误，" + msg;
                    return viewModel.ResponseToJson(HttpStatusCode.Accepted);
                }
                if (request.RenewalSource > 0)
                {
                    List<long> canGroups = new List<long>();
                    fucnLog = LogAssistant.GenerateBHFuncLog(traceId, "获取组", "GetGroups", 1);
                    AspectF.Define.InfoFunctionLog(fucnLog).Do(() => { canGroups = AdditionGroupAlgorithm.GetGroups(_suportedComDictionary.Keys.ToArray()); fucnLog.FunctionLogMsg = "获得的个数为" + canGroups.Count(); });

                    if (canGroups.All(x => x != request.RenewalSource))
                    {
                        viewModel.BusinessStatus = -10000;
                        string msg = "您指定的RenewalSource参数不在我们支持保司范围内";
                        viewModel.StatusMessage = "输入参数错误，" + msg;
                        return viewModel.ResponseToJson(HttpStatusCode.Accepted);
                    }

                }
                //针对 按照车架号和发动机号续保的内容
                fucnLog = LogAssistant.GenerateBHFuncLog(traceId, "校验车牌号和发动机号", "CheckLicensenoOrCarvin", 1);
                AspectF.Define.InfoFunctionLog(fucnLog).Do(() => { CheckLicensenoOrCarvin(request); fucnLog.FunctionLogMsg = "校验完成"; });


                try
                {
                    fucnLog = LogAssistant.GenerateBHFuncLog(traceId, "续保信息服务层", "GetReInfo", 1);
                    GetReInfoResponse response = await AspectF.Define.InfoFunctionLog(fucnLog).Return(() => { return _carInsuranceService.GetReInfo(request, Request.GetQueryNameValuePairs()); });
                    viewModel =await _packagingResponseService.GetViewModel(request, response, viewCityCode, viewCustkey, topAgent, Request.RequestUri.Authority, fucnLog, traceId);
                }
                catch (Exception ex)
                {
                    MetricUtil.UnitReports("renewal");
                    //埋点
                    //AddErrorTrace(Request,ex);
                    sw.Stop();
                    sb.AppendLine("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
                    logError.Info(sb + ";异常了 ===========共耗时(ms)：" + sw.ElapsedMilliseconds);
                }
                //结果埋点
                //AddResultTrace(Request, viewModel.ToJson());
                sw.Stop();
                sb.AppendLine("车辆续保信息记录:" + viewModel.ToJson());
                logInfo.Info(sb + "; 成功了===========共耗时(ms)：" + sw.ElapsedMilliseconds);
            }

            return viewModel.ResponseToJson();
        }
        private DmContext getDmContext(HttpRequestMessage request)
        {
            DmContext dm = null;
            try
            {
                var item = request.Headers.GetValues("DM");
                var enumerable = item as IList<string> ?? item.ToList();
                if (enumerable.Any())
                {
                    dm = enumerable.FirstOrDefault().FromJson<DmContext>();
                }
            }
            catch (Exception)
            {
                var id = System.Guid.NewGuid().ToString();
                dm = new DmContext()
                {
                    RootId = id,
                    ParentId = 1,
                    ChildId = 1,
                };
            }
            return dm;
        }
        private void AddTrace(HttpRequestMessage request)
        {
            DmContext dm = getDmContext(request);
            Message msgtra = new Message
            {
                ActionName = "GetReInfo",
                Body = "消息体",
                ExecuteTime = DateTime.Now,
                ServerIp = NetworkInterfaceManager.GetLocalHostAddress()
            };
            //构建本机跟踪参数
            dm.ParentId = dm.ChildId;
            dm.ChildId += dm.ChildId;
            DmContainer container = new DmContainer
            {
                Dc = dm,
                Msg = msgtra
            };
            //存储
            IDmCacheOperator trace = new DmCacheOperator();
            trace.AddTrace(container);
        }
        private void AddErrorTrace(HttpRequestMessage request, Exception ex)
        {
            DmContext dm = getDmContext(request);
            Message msgtra = new Message
            {
                ActionName = "GetReInfo",
                Body = "发生异常:" + ex.Source + ex.StackTrace + ex.Message + ex.InnerException,
                ExecuteTime = DateTime.Now,
                ServerIp = NetworkInterfaceManager.GetLocalHostAddress()
            };
            //构建本机跟踪参数
            dm.ParentId = dm.ChildId;
            dm.ChildId += dm.ChildId;
            DmContainer container = new DmContainer
            {
                Dc = dm,
                Msg = msgtra
            };
            //存储
            IDmCacheOperator trace = new DmCacheOperator();
            trace.AddTrace(container);
        }
        private void AddResultTrace(HttpRequestMessage request, string result)
        {
            DmContext dm = getDmContext(request);
            Message msgtra = new Message
            {
                ActionName = "GetReInfo",
                Body = result,
                ExecuteTime = DateTime.Now,
                ServerIp = NetworkInterfaceManager.GetLocalHostAddress()
            };
            //构建本机跟踪参数
            dm.ParentId = dm.ChildId;
            dm.ChildId += dm.ChildId;
            DmContainer container = new DmContainer
            {
                Dc = dm,
                Msg = msgtra
            };
            //存储
            IDmCacheOperator trace = new DmCacheOperator();
            trace.AddTrace(container);
        }
        /// <summary>
        /// 请求报价/核保信息
        /// </summary>
        /// <returns></returns>
        [Log("报价", "请求报价", 1)]
        [HttpGet, ActionName("PostPrecisePrice")]
        [EnableThrottling()]
        public async Task<HttpResponseMessage> PostPrecisePrice([FromUri]PostPrecisePriceRequest request)
        {
            string traceId = LogAssistant.GetRequestHeaders(Request, "TraceId");
            BHFunctionLog fucnLog = LogAssistant.GenerateBHFuncLog(traceId, "请求报价，校验请求参数", "CheckRequest", 1);
            BaseViewModel viewModel = new BaseViewModel();
            logInfo.Info(string.Format("请求报价/核保信息接口请求串：{0}", Request.RequestUri));
            #region 初始化参数
            //关系人证件号
            if (!string.IsNullOrWhiteSpace(request.HolderIdCard)) { request.HolderIdCard = request.HolderIdCard.ToUpper(); }
            if (!string.IsNullOrWhiteSpace(request.InsuredIdCard)) { request.InsuredIdCard = request.InsuredIdCard.ToUpper(); }
            if (!string.IsNullOrWhiteSpace(request.IdCard)) { request.IdCard = request.IdCard.ToUpper(); }
            //车牌、车架、发动机号
            if (!string.IsNullOrWhiteSpace(request.LicenseNo)) { request.LicenseNo = request.LicenseNo.ToUpper(); }
            if (!string.IsNullOrWhiteSpace(request.CarVin)) { request.CarVin = request.CarVin.ToUpper(); }
            if (!string.IsNullOrWhiteSpace(request.EngineNo)) { request.EngineNo = request.EngineNo.ToUpper(); }
            if (!string.IsNullOrWhiteSpace(request.UpdateLicenseNo)) { request.UpdateLicenseNo = request.UpdateLicenseNo.ToUpper(); }
            //精友码
            if (!string.IsNullOrWhiteSpace(request.AutoMoldCode)) { request.AutoMoldCode = request.AutoMoldCode.ToUpper(); }
            #endregion
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson(HttpStatusCode.Accepted);
            }
            #region 吉利特殊处理
            string tempshowKey = string.Empty;
            bool tempshow = _tempDemoShowService.BackTempDemoShow(request, out tempshowKey);
            if (tempshow)
            {
                await Task.Delay(TimeSpan.FromSeconds(new Random().Next(10, 20)));
                viewModel.BusinessStatus = 1;
                viewModel.StatusMessage = "请求发送成功";
                return viewModel.ResponseToJson();
            }
            if (!string.IsNullOrEmpty(tempshowKey))
            {
                CacheProvider.Set(tempshowKey, "1", 21600);
                logError.Info(string.Format("{0}成功设置缓存", tempshowKey));
            }
            #endregion

            AspectF.Define.InfoFunctionLog(fucnLog).Do(() => { viewModel = _checkRequestPostPrecisePrice.CheckRequest(request); });
            if (viewModel.BusinessStatus == -10000)
            {
                return viewModel.ResponseToJson(HttpStatusCode.Accepted);
            }
            var keyvaluePaires = Request.GetQueryNameValuePairs();
            fucnLog = LogAssistant.GenerateBHFuncLog(traceId, "请求报价，更新数据库记录", "PostPreciseFactory", 1);
            PostPrecisePriceResponse response = await AspectF.Define.InfoFunctionLog(fucnLog).Return(() => { return PostPreciseFactory(request, keyvaluePaires); });
            if (response.Status == HttpStatusCode.BadRequest || response.Status == HttpStatusCode.Forbidden)
            {
                viewModel.BusinessStatus = -10001;
                if (!string.IsNullOrWhiteSpace(response.StatusMessage))
                {
                    viewModel.StatusMessage = response.StatusMessage;
                }
                else
                {
                    viewModel.StatusMessage = "参数校验错误，请检查您的校验码";
                }
                return viewModel.ResponseToJson(HttpStatusCode.Accepted);
            }
            if (response.Status == HttpStatusCode.NotFound)
            {
                viewModel.BusinessStatus = -10001;
                viewModel.StatusMessage = "请求信息不完整,有两种可能：1.您如果调用过续保接口，有可能是custkey更换了导致此问题。2.如果是直接请求该接口，请完善行驶证信息（车架号、发动机号、品牌型号、初登日期）";
                return viewModel.ResponseToJson(HttpStatusCode.Accepted);
            }
            if (response.Status == HttpStatusCode.NoContent)
            {
                viewModel.BusinessStatus = -10006;
                viewModel.StatusMessage = "您选择的这个投保城市还没有报价渠道，需要（壁虎）事先配置好渠道才可以使用";
                return viewModel.ResponseToJson(HttpStatusCode.Accepted);
            }
            if (response.Status == HttpStatusCode.NotAcceptable)
            {
                viewModel.BusinessStatus = -10005;
                viewModel.StatusMessage = "参数校验错误，请检查您的报价/核保(QuoteGroup,SubmitGroup)组合参数,请确认1.这个城市是否给您配置了对应的渠道（可以调用接口10） 2.submitgroup须是quotegroup的子集";
                return viewModel.ResponseToJson(HttpStatusCode.Accepted);
            }
            if (request.QuoteParalelConflict == 1)
            {
                if (response.Status == HttpStatusCode.Conflict)
                {
                    viewModel.BusinessStatus = -10008;
                    viewModel.StatusMessage = "此车辆正在报价，请勿重复提交";
                    return viewModel.ResponseToJson(HttpStatusCode.Accepted);
                }
            }

            if (response.Status == HttpStatusCode.NonAuthoritativeInformation)
            {
                viewModel.BusinessStatus = -10007;
                viewModel.StatusMessage = "需要完善行驶证信息，才可以报价";
                return viewModel.ResponseToJson(HttpStatusCode.Accepted);
            }
            if (response.Status == HttpStatusCode.ExpectationFailed)
            {
                viewModel.BusinessStatus = -10003;
                viewModel.StatusMessage = "服务发生异常";
                return viewModel.ResponseToJson(HttpStatusCode.Accepted);
            }
            viewModel.BusinessStatus = 1;
            viewModel.StatusMessage = "请求发送成功";
            return viewModel.ResponseToJson();
        }

        [Log("报价", "请求报价（含特约）", 1)]
        [HttpPost, ActionName("PostNewPrecisePrice")]
        //[EnableThrottling()]
        public async Task<HttpResponseMessage> PostNewPrecisePrice([FromBody]PostPrecisePriceRequest request)
        {
            string traceId = LogAssistant.GetRequestHeaders(Request, "TraceId");
            BHFunctionLog fucnLog = LogAssistant.GenerateBHFuncLog(traceId, "请求报价，校验请求参数", "CheckRequest", 1);
            BaseViewModel viewModel = new BaseViewModel();
            logInfo.Info(string.Format("请求报价/核保信息接口请求串：{0};{1}", Request.RequestUri, request.ToJson()));
            #region 初始化参数
            //关系人证件号
            if (!string.IsNullOrWhiteSpace(request.HolderIdCard)) { request.HolderIdCard = request.HolderIdCard.ToUpper(); }
            if (!string.IsNullOrWhiteSpace(request.InsuredIdCard)) { request.InsuredIdCard = request.InsuredIdCard.ToUpper(); }
            if (!string.IsNullOrWhiteSpace(request.IdCard)) { request.IdCard = request.IdCard.ToUpper(); }
            //车牌、车架、发动机号
            if (!string.IsNullOrWhiteSpace(request.LicenseNo)) { request.LicenseNo = request.LicenseNo.ToUpper(); }
            if (!string.IsNullOrWhiteSpace(request.CarVin)) { request.CarVin = request.CarVin.ToUpper(); }
            if (!string.IsNullOrWhiteSpace(request.EngineNo)) { request.EngineNo = request.EngineNo.ToUpper(); }
            if (!string.IsNullOrWhiteSpace(request.UpdateLicenseNo)) { request.UpdateLicenseNo = request.UpdateLicenseNo.ToUpper(); }
            //精友码
            if (!string.IsNullOrWhiteSpace(request.AutoMoldCode)) { request.AutoMoldCode = request.AutoMoldCode.ToUpper(); }
            #endregion
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson(HttpStatusCode.Accepted);
            }
            AspectF.Define.InfoFunctionLog(fucnLog).Do(() => { viewModel = _checkRequestPostPrecisePrice.CheckRequest(request); });
            if (viewModel.BusinessStatus == -10000)
            {
                return viewModel.ResponseToJson(HttpStatusCode.Accepted);
            }
            var requestDate = Request.Content.ReadAsStringAsync().Result;
            List<KeyValuePair<string, string>> keyvaluePaires = new List<KeyValuePair<string, string>>();
            if (requestDate.Length > 0)
            {
                requestDate = requestDate.Replace("?", "");
                var arrKeyValues = requestDate.Split('&');
                for (int i = 0; i < arrKeyValues.Length; i++)
                {
                    var keyValue = arrKeyValues[i].Split('=');
                    if (keyValue.Length == 2)
                    {
                        keyvaluePaires.Add(new KeyValuePair<string, string>(keyValue[0], keyValue[1]));
                    }
                }
            }
            fucnLog = LogAssistant.GenerateBHFuncLog(traceId, "请求报价，更新数据库记录", "PostPreciseFactory", 1);
            PostPrecisePriceResponse response = await AspectF.Define.InfoFunctionLog(fucnLog).Return(() => { return PostPreciseFactory(request, keyvaluePaires); });
            if (response.Status == HttpStatusCode.BadRequest || response.Status == HttpStatusCode.Forbidden)
            {
                viewModel.BusinessStatus = -10001;
                if (!string.IsNullOrWhiteSpace(response.StatusMessage))
                {
                    viewModel.StatusMessage = response.StatusMessage;
                }
                else
                {
                    viewModel.StatusMessage = "参数校验错误，请检查您的校验码";
                }
                return viewModel.ResponseToJson(HttpStatusCode.Accepted);
            }
            if (response.Status == HttpStatusCode.NotFound)
            {
                viewModel.BusinessStatus = -10001;
                viewModel.StatusMessage = "请求信息不完整,有两种可能：1.您如果调用过续保接口，有可能是custkey更换了导致此问题。2.如果是直接请求该接口，请完善行驶证信息（车架号、发动机号、品牌型号、初登日期）";
                return viewModel.ResponseToJson(HttpStatusCode.Accepted);
            }
            if (response.Status == HttpStatusCode.NoContent)
            {
                viewModel.BusinessStatus = -10006;
                viewModel.StatusMessage = "您选择的这个投保城市还没有报价渠道，需要（壁虎）事先配置好渠道才可以使用";
                return viewModel.ResponseToJson(HttpStatusCode.Accepted);
            }
            if (response.Status == HttpStatusCode.NotAcceptable)
            {
                viewModel.BusinessStatus = -10005;
                viewModel.StatusMessage = "参数校验错误，请检查您的报价/核保(QuoteGroup,SubmitGroup)组合参数,请确认1.这个城市是否给您配置了对应的渠道（可以调用接口10） 2.submitgroup须是quotegroup的子集";
                return viewModel.ResponseToJson(HttpStatusCode.Accepted);
            }
            if (request.QuoteParalelConflict == 1)
            {
                if (response.Status == HttpStatusCode.Conflict)
                {
                    viewModel.BusinessStatus = -10008;
                    viewModel.StatusMessage = "此车辆正在报价，请勿重复提交";
                    return viewModel.ResponseToJson(HttpStatusCode.Accepted);
                }
            }

            if (response.Status == HttpStatusCode.NonAuthoritativeInformation)
            {
                viewModel.BusinessStatus = -10007;
                viewModel.StatusMessage = "需要完善行驶证信息，才可以报价";
                return viewModel.ResponseToJson(HttpStatusCode.Accepted);
            }
            if (response.Status == HttpStatusCode.ExpectationFailed)
            {
                viewModel.BusinessStatus = -10003;
                viewModel.StatusMessage = "服务发生异常";
                return viewModel.ResponseToJson(HttpStatusCode.Accepted);
            }
            viewModel.BusinessStatus = 1;
            viewModel.StatusMessage = "请求发送成功";
            return viewModel.ResponseToJson();
        }
        /// <summary>
        /// 请求报价/核保信息 //废弃
        /// </summary>
        /// <returns></returns>
        [HttpGet, ActionName("PostPrecisePriceAgain")]
        public async Task<HttpResponseMessage> PostPrecisePriceAgain([FromUri]PostPrecisePriceRequestAgain request)
        {
            BaseViewModel viewModel = new BaseViewModel();
            logInfo.Info(string.Format("重新请求报价/核保信息接口请求串：{0}", Request.RequestUri));
            var req = Request.RequestUri;
            string[] defaultSettings = new string[]
            {
                "http://qa.interfaces.com","http://it.91bihu.me","http://192.168.5.88:8099/"
            };
            var settings = System.Configuration.ConfigurationManager.AppSettings["CanCallOnly"];
            var splitSettings = string.IsNullOrWhiteSpace(settings) ? defaultSettings : settings.Split(',');
            var isValid = false;
            foreach (var route in splitSettings)
            {
                if (req.AbsoluteUri.IndexOf(route) == 0)
                {
                    isValid = true;
                    break;
                }
            }
            if (!isValid)
            {
                viewModel.BusinessStatus = -10003;
                viewModel.StatusMessage = "服务发生异常";
                logInfo.Info(string.Format("有非合法路径进来的重新请求报价/核保信息接口请求串：{0}", Request.RequestUri));
                return viewModel.ResponseToJson(HttpStatusCode.Accepted);
            }
            var keyvaluePaires = Request.GetQueryNameValuePairs();
            var response = await _carInsuranceService.UpdateDrivingLicenseAgain(request, keyvaluePaires);
            if (response.Status == HttpStatusCode.ExpectationFailed)
            {
                viewModel.BusinessStatus = -10003;
                viewModel.StatusMessage = "服务发生异常";
                return viewModel.ResponseToJson(HttpStatusCode.Accepted);
            }
            viewModel.BusinessStatus = 1;
            viewModel.StatusMessage = "请求发送成功";
            return viewModel.ResponseToJson();
        }
        /// <summary>
        /// 获取车辆报价信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Log("报价", "获取报价", 2)]
        [HttpGet, ActionName("GetPrecisePrice")]
        public async Task<HttpResponseMessage> FetchPrecisePrice([FromUri]GetPrecisePriceRequest request)
        {
            string traceId = LogAssistant.GetRequestHeaders(Request, "TraceId");
            logInfo.Info(string.Format("获取车辆报价信息接口请求串：{0}", Request.RequestUri));
            BHFunctionLog fucnLog = LogAssistant.GenerateBHFuncLog(traceId, "获取车辆报价信息", "GetPrecisePrice", 1);
            var viewModel = new GetPrecisePriceViewModel();
            try
            {
                viewModel.CustKey = request.CustKey;
                viewModel.CheckCode = request.CheckCode;
                if (!ModelState.IsValid)
                {
                    viewModel.BusinessStatus = -10000;
                    string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                    viewModel.StatusMessage = "输入参数错误，" + msg;
                    return viewModel.ResponseToJson();
                }
                GetPrecisePriceReponse response = await AspectF.Define.InfoFunctionLog(fucnLog).Return(() => { return _carInsuranceService.GetPrecisePrice(request, Request.GetQueryNameValuePairs()); });


                if (response.Status == HttpStatusCode.BadRequest || response.Status == HttpStatusCode.Forbidden)
                {
                    viewModel.BusinessStatus = -10001;
                    viewModel.StatusMessage = !string.IsNullOrEmpty(response.ErrMsg) ? response.ErrMsg : "参数校验错误，请检查您的校验码";
                    return viewModel.ResponseToJson();
                }
                if (response.Status == HttpStatusCode.ExpectationFailed)
                {
                    viewModel.BusinessStatus = -10003;
                    viewModel.StatusMessage = "服务器发生异常";
                    return viewModel.ResponseToJson();
                }
                fucnLog = LogAssistant.GenerateBHFuncLog(traceId, "报价信息的 userinfo部分", "ConvertToPreciseViewModel", 1);
                AspectF.Define.InfoFunctionLog(fucnLog).Do(() => { viewModel.UserInfo = response.UserInfo.ConvertToPreciseViewModel(response.LastInfo, response.QuoteResult, response.CarInfo, request.TimeFormat); });

                if (response.SubmitInfo != null)
                {
                    viewModel.BusinessStatus = 1;
                    viewModel.StatusMessage = "获取报价信息成功";
                    viewModel.Item = response.UserInfo.ConvertToViewModel(response.SaveQuote,
                    response.QuoteResult, response.SubmitInfo, response.YwxDetails);
                    if (request.Agent == 3820)
                    {
                        viewModel.CarInfo = response.CarInfo.ConvertToViewModel();
                        viewModel.CarInfo.Source = int.Parse(request.IntentionCompany);
                    }
                    if (request.ShowCarInfo == 1)
                    {
                        viewModel.CarInfo = response.CarInfo.ConvertToViewModel();
                        viewModel.CarInfo.Source = int.Parse(request.IntentionCompany);
                        if (request.ShowXinZhuanXu == 0)
                        {
                            viewModel.CarInfo.XinZhuanXu = null;
                        }
                    }
                    viewModel.Item.Source = int.Parse(request.IntentionCompany);
                    if (request.QuoteGroup > 0)
                    {
                        viewModel.Item.Source = request.QuoteGroup;
                    }
                    viewModel.Item.ValidateCar = new ValidateCar()
                    {
                        BizValidateCar = (response.SubmitInfo.BizcInspectorNme ?? 0).ToString(),
                        ForceValidateCar = (response.SubmitInfo.ForcecInspectorNme ?? 0).ToString(),
                        IsValidateCar = ((response.SubmitInfo.ForcecInspectorNme ?? 0) | (response.SubmitInfo.BizcInspectorNme ?? 0)).ToString()
                    };
                }
                else
                {
                    viewModel.Item = new PrecisePriceItemViewModel
                    {
                        Source = request.QuoteGroup > 0 ? request.QuoteGroup : int.Parse(request.IntentionCompany),
                        JiaYi = new List<JiaYiModel>()
                    };
                    viewModel.BusinessStatus = -10002;
                    viewModel.StatusMessage = "获取报价信息失败";
                }
                if (request.ShowXiuLiChangType == 0)
                {
                    viewModel.Item.HcXiuLiChangType = null;
                }
                if (request.ShowMobile == 0)
                {
                    viewModel.UserInfo.Mobile = null;
                }
                if (request.ShowEmail == 0)
                {
                    viewModel.UserInfo.Email = null;
                }
                if (request.ShowVehicleInfo == 0)
                {
                    viewModel.UserInfo.AutoMoldCode = null;
                    viewModel.UserInfo.VehicleInfo = null;
                }
                if (request.ShowSheBei == 0)
                {
                    viewModel.Item.SheBeis = null;
                    viewModel.Item.SheBeiSunShi = null;
                    viewModel.Item.BjmSheBeiSunShi = null;
                }
                if (request.ShowFybc == 0)
                {
                    viewModel.Item.Fybc = null;
                    viewModel.Item.FybcDays = null;
                }
                if (request.ShowPingAnScore == 0)
                {
                    viewModel.Item.PingAnScore = null;
                }
                if (request.ShowBusyForceType == 0)
                {
                    if (request.ShowCarInfo == 1)
                    {
                        viewModel.CarInfo.SyVehicleClaimType = null;
                        viewModel.CarInfo.JqVehicleClaimType = null;
                    }
                }

                if (request.ShowVehicleStyle == 0)
                {
                    if (request.ShowCarInfo == 1)
                    {
                        viewModel.CarInfo.VehicleStyle = null;
                    }
                }
                if (request.ShowSanZheJieJiaRi == 1)
                {
                    viewModel.Item.SanZheJieJiaRi = new XianZhongUnit()
                    {
                        BaoE = (response.SaveQuote.SanZheJieJiaRi.HasValue ? response.SaveQuote.SanZheJieJiaRi.Value : 0) > 0 ? 1 : 0,
                        BaoFei = response.QuoteResult.SanZheJieJiaRi.HasValue ? response.QuoteResult.SanZheJieJiaRi.Value : 0
                    };
                }
                if (request.ShowRelationInfo == 0)
                {
                    if (viewModel.UserInfo != null)
                    {
                        viewModel.UserInfo.CarOwnerPerson = null;
                        viewModel.UserInfo.HolderPerson = null;
                        viewModel.UserInfo.InsuredPerson = null;
                    }
                }
                if (request.ShowPostStartDateTime == 1)
                {
                    if (viewModel.UserInfo != null)
                    {
                        if (viewModel.Item.QuoteStatus != 1)
                        {
                            viewModel.UserInfo.PostStartDate = new PostStartDateTime
                            {
                                BusinessStartDate = response.ReqInfo != null
                                    ? (response.ReqInfo.biz_start_date.HasValue
                                        ? (request.TimeFormat == 1
                                            ? response.ReqInfo.biz_start_date.Value.ToString("yyyy-MM-dd HH:mm:ss")
                                            : response.ReqInfo.biz_start_date.Value.ToString("yyyy-MM-dd"))
                                        : string.Empty)
                                    : string.Empty,
                                ForceStartDate = response.ReqInfo != null
                                    ? (response.ReqInfo.force_start_date.HasValue
                                        ? (request.TimeFormat == 1
                                            ? response.ReqInfo.force_start_date.Value.ToString("yyyy-MM-dd HH:mm:ss")
                                            : response.ReqInfo.force_start_date.Value.ToString("yyyy-MM-dd"))
                                        : string.Empty)
                                    : string.Empty
                            };
                        }
                        else
                        {
                            viewModel.UserInfo.PostStartDate = new PostStartDateTime
                            {
                                BusinessStartDate = viewModel.UserInfo.BusinessStartDate,
                                ForceStartDate = viewModel.UserInfo.ForceStartDate
                            };
                        }

                    }
                }
                if (request.ShowTotalRate == 0)
                {
                    viewModel.Item.TotalRate = null;
                }
                //是否显示太平洋最小折扣率
                //注释做修改，20180915，此处增加人保的最小折扣率，平安返回的不做处理，前端默认固定值
                if (request.ShowActualDiscounts == 0)
                {
                    viewModel.Item.ActualDiscounts = null;
                }
                //是否展示渠道信息
                if (request.ShowChannel == 1)
                {
                    if (response.AgentConifg != null)
                    {
                        viewModel.Item.Channel = new ChannelInfo
                        {
                            ChannelId = response.AgentConifg.id,
                            ChannelName =
                                string.IsNullOrWhiteSpace(response.AgentConifg.config_name)
                                    ? string.Empty
                                    : response.AgentConifg.config_name,
                            IsPaicApi = response.AgentConifg.is_paic_api.ToString()
                        };
                    }
                    else
                    {
                        viewModel.Item.Channel = new ChannelInfo
                        {
                            ChannelId = 0,
                            ChannelName = string.Empty,
                            IsPaicApi = "0"
                        };
                    }
                }
                if (request.ShowRepeatSubmit == 0)
                {
                    viewModel.Item.RepeatSubmitResult = null;
                }
                viewModel.CheckCode = string.IsNullOrWhiteSpace(request.CheckCode) ? response.CheckCode : request.CheckCode;
                //预期赔付率
                if (request.ShowExpectedLossRate == 0)
                {
                    viewModel.Item.ExpectedLossRate = null;
                }
                //商业太保分，交商合计太保分，交商预期赔付率
                if (request.ShowTaiBaoFen == 0)
                {
                    viewModel.Item.BizCpicScore = null;
                    viewModel.Item.TotalCpicScore = null;
                    viewModel.Item.TotalEcompensationRate = null;
                }

                if (request.ShowVersion == 0)
                {
                    viewModel.Item.VersionType = null;
                    viewModel.Item.IsRB3Version = null;
                }
                if (request.ShowIdCard == 0 && viewModel.UserInfo != null)
                {
                    viewModel.UserInfo.SixDigitsAfterIdCard = null;
                }
                //是否显示错误编码
                if (request.ShowErrorCode == 0)
                {
                    viewModel.Item.QuoteErrorCode = null;
                    viewModel.Item.QuoteErrorResult = null;
                }
                //是否显示不计免总额
                if (request.ShowBuJiMianFuJia == 0)
                {
                    viewModel.Item.BuJiMianFuJia = null;
                }
                if (request.ShowEndDate == 0)
                {
                    viewModel.UserInfo.BusinessEndDate = null;
                    viewModel.UserInfo.ForceEndDate = null;
                }
                else if (request.ShowEndDate == 1)
                {
                    if (string.IsNullOrEmpty(viewModel.UserInfo.BusinessEndDate))
                    {
                        viewModel.UserInfo.BusinessEndDate = "";
                    }
                    if (string.IsNullOrEmpty(viewModel.UserInfo.ForceEndDate))
                    {
                        viewModel.UserInfo.ForceEndDate = "";
                    }
                }

                if (viewModel.CarInfo != null)
                {
                    if (request.ShowCarProperties == 0)
                    {
                        viewModel.CarInfo.TransferDate = null;
                        viewModel.CarInfo.IsLoans = null;
                    }
                    else if (request.ShowCarProperties > 0)
                    {
                        if (response.ReqInfo != null)
                        {
                            viewModel.CarInfo.TransferDate = response.ReqInfo.transfer_date.HasValue ? response.ReqInfo.transfer_date.Value.ToString("yyyy-MM-dd HH:mm:ss") : "";
                            viewModel.CarInfo.IsLoans = response.ReqInfo.is_loans.HasValue ? response.ReqInfo.is_loans.Value : -1;
                        }
                        else
                        {
                            viewModel.CarInfo.TransferDate = "-1";
                            viewModel.CarInfo.IsLoans = -1;
                        }
                    }
                }

                //安心验车
                if (request.ShowAnXinYanChe == 0)
                {
                    viewModel.Item.ValidateCar = null;
                }
                if (request.ShowVehicleInfoOther == 0)
                {
                    if (viewModel.CarInfo != null)
                    {
                        viewModel.CarInfo.VehicleAlias = null;
                        viewModel.CarInfo.VehicleYear = null;
                    }
                }
                //驾意险展示
                if (request.ShowJiaYi == 0)
                {
                    viewModel.Item.JiaYi = null;
                    viewModel.Item.JiaYiTotal = null;
                }

            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException + " 请求对象：" + Request.RequestUri);
            }

            return viewModel.ResponseToJson();
        }
        /// <summary>
        /// 报价信息 含有buid接口 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet, ActionName("GetSpecialPrecisePrice")]
        [Log("报价", "获取报价", 2)]
        public async Task<HttpResponseMessage> FetchSpecialPrecisePrice([FromUri]GetPrecisePriceRequest request)
        {
            string traceId = LogAssistant.GetRequestHeaders(Request, "TraceId");
            BHFunctionLog fucnLog = LogAssistant.GenerateBHFuncLog(traceId, "获取车辆报价信息", "GetPrecisePrice", 1);
            var viewModel = new GetPrecisePriceViewModelWithBuid();
            try
            {
                viewModel.CustKey = request.CustKey;
                viewModel.CheckCode = request.CheckCode;
                logInfo.Info(string.Format("获取车辆报价信息接口请求串：{0}", Request.RequestUri));
                if (!ModelState.IsValid)
                {
                    viewModel.BusinessStatus = -10000;
                    string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                    viewModel.StatusMessage = "输入参数错误，" + msg;
                    return viewModel.ResponseToJson();
                }

                GetPrecisePriceReponse response = await AspectF.Define.InfoFunctionLog(fucnLog).Return(() => { return _carInsuranceService.GetPrecisePrice(request, Request.GetQueryNameValuePairs()); });

                if (response.Status == HttpStatusCode.BadRequest || response.Status == HttpStatusCode.Forbidden)
                {
                    viewModel.BusinessStatus = -10001;
                    viewModel.StatusMessage = !string.IsNullOrEmpty(response.ErrMsg) ? response.ErrMsg : "参数校验错误，请检查您的校验码";
                    return viewModel.ResponseToJson();
                }
                if (response.Status == HttpStatusCode.ExpectationFailed)
                {
                    viewModel.BusinessStatus = -10003;
                    viewModel.StatusMessage = "服务器发生异常";
                    return viewModel.ResponseToJson();
                }
                fucnLog = LogAssistant.GenerateBHFuncLog(traceId, "报价信息的 userinfo部分", "ConvertToPreciseViewModel", 1);
                AspectF.Define.InfoFunctionLog(fucnLog).Do(() => { viewModel.UserInfo = response.UserInfo.ConvertToPreciseViewModel(response.LastInfo, response.QuoteResult, response.CarInfo, request.TimeFormat); });


                if (response.SubmitInfo != null)
                {
                    viewModel.BusinessStatus = 1;
                    viewModel.StatusMessage = "获取报价信息成功";
                    fucnLog = LogAssistant.GenerateBHFuncLog(traceId, "对内报价详情 带有Buid", "ConvertToViewModelWithBuid", 1);
                    AspectF.Define.InfoFunctionLog(fucnLog).Do(() => { viewModel.Item = response.UserInfo.ConvertToViewModelWithBuid(response.SaveQuote, response.QuoteResult, response.SubmitInfo, response.YwxDetails); });
                    if (request.ShowCarInfo == 1)
                    {
                        AspectF.Define.InfoFunctionLog(fucnLog).Do(() => { viewModel.CarInfo = response.CarInfo.ConvertToViewModel(); });
                        viewModel.CarInfo.Source = int.Parse(request.IntentionCompany);
                        if (request.ShowXinZhuanXu == 0)
                        {
                            viewModel.CarInfo.XinZhuanXu = null;
                        }
                    }
                    viewModel.Item.Source = int.Parse(request.IntentionCompany);
                    if (request.QuoteGroup > 0)
                    {
                        viewModel.Item.Source = request.QuoteGroup;
                    }
                    viewModel.Item.ValidateCar = new ValidateCar()
                    {
                        BizValidateCar = (response.SubmitInfo.BizcInspectorNme ?? 0).ToString(),
                        ForceValidateCar = (response.SubmitInfo.ForcecInspectorNme ?? 0).ToString(),
                        IsValidateCar = ((response.SubmitInfo.ForcecInspectorNme ?? 0) | (response.SubmitInfo.BizcInspectorNme ?? 0)).ToString()
                    };
                }
                else
                {
                    viewModel.Item = new PrecisePriceItemViewModelWithBuid
                    {
                        Source = request.QuoteGroup > 0 ? request.QuoteGroup : int.Parse(request.IntentionCompany),
                        BuId = response.UserInfo.Id
                    };
                    viewModel.BusinessStatus = -10002;
                    viewModel.StatusMessage = "获取报价信息失败";
                }
                if (request.ShowSheBei == 0)
                {
                    viewModel.Item.SheBeis = null;
                    viewModel.Item.HcSheBeiSunshi = null;
                    viewModel.Item.BjmSheBeiSunShi = null;
                }
                if (request.ShowSanZheJieJiaRi == 0)
                {
                    viewModel.Item.SanZheJieJiaRi = null;
                }
                viewModel.ReqInfo = new QuoteReqCarInfoViewModel()
                {
                    AutoMoldCode = response.ReqInfo == null ? null : response.ReqInfo.auto_model_code ?? "",
                    IsNewCar = response.ReqInfo == null ? 2 : (response.ReqInfo.is_newcar ?? 2),
                    NegotiatePrice = response.ReqInfo == null ? 0 : (response.ReqInfo.co_real_value ?? 0),
                    IsPublic = response.ReqInfo == null ? 0 : (response.ReqInfo.is_public ?? 0),
                    CarUsedType = response.ReqInfo == null ? 0 : (response.ReqInfo.car_used_type ?? 0),
                    AutoMoldCodeSource = response.ReqInfo == null ? -1 : (response.ReqInfo.auto_model_code_source ?? -1),
                    DriveLicenseTypeName = response.ReqInfo == null ? "" : (response.ReqInfo.drivlicense_cartype_name ?? ""),
                    DriveLicenseTypeValue = response.ReqInfo == null ? "" : (response.ReqInfo.drivlicense_cartype_value ?? ""),
                    SeatUpdated = response.ReqInfo == null ? "-1" : (response.ReqInfo.seatflag ?? -1).ToString(),
                    RequestActualDiscounts = response.ReqInfo == null ? "" : (response.ReqInfo.ActualDiscounts ?? 0).ToString(),
                    RequestIsPaFloorPrice = response.ReqInfo == null ? "" : (response.ReqInfo.IsPaFloorPrice ?? 0).ToString(),
                    DriverCard = response.ReqInfo == null ? "" : response.ReqInfo.DriverCard ?? "",
                    DriverCardType = response.ReqInfo == null ? "" : response.ReqInfo.DriverCardType ?? "",
                };
                //modify20181124如果没拿到报价结果的座位数，取请求的座位数
                if (response.CarInfo != null)
                {
                    viewModel.Item.SeatCount = response.CarInfo.seat_count ?? 0;
                }
                if (viewModel.Item.SeatCount == 0 && response.ReqInfo != null)
                {
                    viewModel.Item.SeatCount = response.ReqInfo.seat_count ?? 0;
                }
                //addby20180915继续拼装viewModel.ReqInfo对象，这是新增的折扣率的对象
                if (response.ReqInfo != null)
                {
                    List<DiscountViewModel> dclist = new List<DiscountViewModel>();
                    DiscountViewModel dc;
                    Dictionary<int, decimal> dictionary = new Dictionary<int, decimal>();
                    dictionary = response.ReqInfo.actualdiscounts_ratio.FromJson<Dictionary<int, decimal>>();
                    if (dictionary != null)
                    {
                        foreach (var item in dictionary)
                        {
                            dc = new DiscountViewModel()
                            {
                                Source = SourceGroupAlgorithm.GetNewSource(item.Key),
                                AD = item.Value,
                                CR = 0,
                                SR = 0,
                                TRCR = ""
                            };
                            //平安的特殊处理，取单独的字段
                            if (item.Key == 0)
                            {
                                dc.CR = response.ReqInfo.ChannelRate ?? 0;
                                dc.SR = response.ReqInfo.SubmitRate ?? 0;
                                dc.TRCR = response.ReqInfo.TrCausesWhy ?? "";
                            }
                            dclist.Add(dc);
                        }
                    }
                    viewModel.ReqInfo.RequestDiscount = dclist;
                }
                if (request.ShowXiuLiChangType == 0)
                {
                    viewModel.Item.HcXiuLiChangType = null;
                }

                if (request.ShowMobile == 0)
                {
                    viewModel.UserInfo.Mobile = null;
                }
                if (request.ShowEmail == 0)
                {
                    viewModel.UserInfo.Email = null;
                }
                if (request.ShowVehicleInfo == 0)
                {
                    viewModel.UserInfo.AutoMoldCode = null;
                    viewModel.UserInfo.VehicleInfo = null;
                }
                if (request.ShowSheBei == 0)
                {
                    viewModel.Item.SheBeis = null;
                    viewModel.Item.SheBeiSunShi = null;
                    viewModel.Item.BjmSheBeiSunShi = null;
                }
                if (request.ShowFybc == 0)
                {
                    viewModel.Item.Fybc = null;
                    viewModel.Item.FybcDays = null;
                }
                if (request.ShowPingAnScore == 0)
                {
                    viewModel.Item.PingAnScore = null;
                }
                if (request.ShowBusyForceType == 0)
                {
                    if (request.ShowCarInfo == 1)
                    {
                        viewModel.CarInfo.SyVehicleClaimType = null;
                        viewModel.CarInfo.JqVehicleClaimType = null;
                    }

                }

                if (request.ShowVehicleStyle == 0)
                {
                    if (request.ShowCarInfo == 1)
                    {
                        viewModel.CarInfo.VehicleStyle = null;
                    }

                }
                if (request.ShowRelationInfo == 0)
                {
                    if (viewModel.UserInfo != null)
                    {
                        viewModel.UserInfo.CarOwnerPerson = null;
                        viewModel.UserInfo.HolderPerson = null;
                        viewModel.UserInfo.InsuredPerson = null;
                    }
                }
                if (request.ShowPostStartDateTime == 1)
                {
                    if (viewModel.UserInfo != null)
                    {
                        if (viewModel.Item.QuoteStatus != 1)
                        {
                            viewModel.UserInfo.PostStartDate = new PostStartDateTime
                            {
                                BusinessStartDate = response.ReqInfo != null
                                    ? (response.ReqInfo.biz_start_date.HasValue
                                        ? (request.TimeFormat == 1
                                            ? response.ReqInfo.biz_start_date.Value.ToString("yyyy-MM-dd HH:mm:ss")
                                            : response.ReqInfo.biz_start_date.Value.ToString("yyyy-MM-dd"))
                                        : string.Empty)
                                    : string.Empty,
                                ForceStartDate = response.ReqInfo != null
                                    ? (response.ReqInfo.force_start_date.HasValue
                                        ? (request.TimeFormat == 1
                                            ? response.ReqInfo.force_start_date.Value.ToString("yyyy-MM-dd HH:mm:ss")
                                            : response.ReqInfo.force_start_date.Value.ToString("yyyy-MM-dd"))
                                        : string.Empty)
                                    : string.Empty
                            };
                        }
                        else
                        {
                            viewModel.UserInfo.PostStartDate = new PostStartDateTime
                            {
                                BusinessStartDate = viewModel.UserInfo.BusinessStartDate,
                                ForceStartDate = viewModel.UserInfo.ForceStartDate
                            };
                        }

                    }
                }
                if (request.ShowTotalRate == 0)
                {
                    viewModel.Item.TotalRate = null;
                }
                //是否显示太平洋最小折扣率
                //注释做修改，20180915，此处增加人保的最小折扣率，平安返回的不做处理，前端默认固定值
                if (request.ShowActualDiscounts == 0)
                {
                    viewModel.Item.ActualDiscounts = null;
                }
                //是否展示渠道信息
                if (request.ShowChannel == 1)
                {
                    if (response.AgentConifg != null)
                    {
                        viewModel.Item.Channel = new ChannelInfo
                        {
                            ChannelId = response.AgentConifg.id,
                            ChannelName =
                                string.IsNullOrWhiteSpace(response.AgentConifg.config_name)
                                    ? string.Empty
                                    : response.AgentConifg.config_name,
                            IsPaicApi = response.AgentConifg.is_paic_api.ToString()
                        };
                    }
                    else
                    {
                        viewModel.Item.Channel = new ChannelInfo
                        {
                            ChannelId = 0,
                            ChannelName = string.Empty,
                            IsPaicApi = "0"
                        };
                    }
                }
                if (request.ShowRepeatSubmit == 0)
                {
                    viewModel.Item.RepeatSubmitResult = null;
                }
                viewModel.CheckCode = string.IsNullOrWhiteSpace(request.CheckCode) ? response.CheckCode : request.CheckCode;
                //预期赔付率
                if (request.ShowExpectedLossRate == 0)
                {
                    viewModel.Item.ExpectedLossRate = null;
                }

                //商业太保分，交商合计太保分，交商预期赔付率
                if (request.ShowTaiBaoFen == 0)
                {
                    viewModel.Item.BizCpicScore = null;
                    viewModel.Item.TotalCpicScore = null;
                    viewModel.Item.TotalEcompensationRate = null;
                }

                //是否修改座位数
                if (request.ShowSeatUpdated == 0)
                {
                    viewModel.ReqInfo.SeatUpdated = null;
                }
                if (request.ShowVersion == 0)
                {
                    viewModel.Item.VersionType = null;
                    viewModel.Item.IsRB3Version = null;
                }
                if (request.ShowIdCard == 0 && viewModel.UserInfo != null)
                {
                    viewModel.UserInfo.SixDigitsAfterIdCard = null;
                }
                //安心验车
                if (request.ShowAnXinYanChe == 0)
                {
                    viewModel.Item.ValidateCar = null;
                }
                //是否显示错误编码
                if (request.ShowErrorCode == 0)
                {
                    viewModel.Item.QuoteErrorCode = null;
                    viewModel.Item.QuoteErrorResult = null;
                }
                if (request.ShowEndDate == 0)
                {
                    viewModel.UserInfo.BusinessEndDate = null;
                    viewModel.UserInfo.ForceEndDate = null;
                }
                else if (request.ShowEndDate > 0)
                {
                    if (string.IsNullOrEmpty(viewModel.UserInfo.BusinessEndDate))
                    {
                        viewModel.UserInfo.BusinessEndDate = "";
                    }
                    else if (string.IsNullOrEmpty(viewModel.UserInfo.ForceEndDate))
                    {
                        viewModel.UserInfo.ForceEndDate = "";
                    }
                }
                if (request.ShowVehicleInfoOther == 0)
                {
                    if (viewModel.CarInfo != null)
                    {
                        viewModel.CarInfo.VehicleAlias = null;
                        viewModel.CarInfo.VehicleYear = null;
                    }
                }
                logInfo.Info("获取报价结果：" + viewModel.ToJson());
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException + " 请求对象：" + Request.RequestUri);
            }

            return viewModel.ResponseToJson();
        }
        /// <summary>
        /// 获取核保信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet, ActionName("GetSubmitInfo")]
        public async Task<HttpResponseMessage> FetchSubmitInfo([FromUri]GetSubmitInfoRequest request)
        {
            logInfo.Info(string.Format("获取核保信息请求串：{0}", Request.RequestUri));
            SubmitInfoViewModel viewModel = new SubmitInfoViewModel();
            viewModel.OrderId = request.OrderId;
            viewModel.CustKey = request.CustKey;
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }
            GetSubmitInfoResponse response = await _carInsuranceService.GetSubmitInfo(request, Request.GetQueryNameValuePairs());
            if (response.Status == HttpStatusCode.BadRequest || response.Status == HttpStatusCode.Forbidden)
            {
                viewModel.BusinessStatus = -10001;
                viewModel.StatusMessage = !string.IsNullOrEmpty(response.ErrMsg) ? response.ErrMsg : "参数校验错误，请检查您的校验码";
                return viewModel.ResponseToJson();
            }
            if (response.Status == HttpStatusCode.ExpectationFailed)
            {
                viewModel.BusinessStatus = -10003;
                viewModel.StatusMessage = "服务器发生异常";
                return viewModel.ResponseToJson();
            }
            if (response.SubmitInfo != null)
            {
                if (request.Agent == 4405)
                {
                    viewModel = response.SubmitInfo.ConverToViewModelForCustom();
                }
                else
                {
                    viewModel = response.SubmitInfo.ConverToViewModelForDaiLi();
                }
                if (request.ShowChannel == 0)
                {
                    viewModel.Item.ChannelId = null;
                }
                //if (response.CustKey.Length > 0)
                //{
                //    viewModel.CustKey = response.CustKey.Substring(response.CustKey.IndexOf("-", System.StringComparison.Ordinal) + 1);
                //}
                viewModel.BusinessStatus = 1;
                viewModel.StatusMessage = "成功获取核保信息";
                //viewModel.OrderId = request.OrderId;
                viewModel.CustKey = request.CustKey;
                viewModel.OrderId = string.IsNullOrEmpty(request.OrderId) ? response.OrderId : request.OrderId;
                viewModel.CheckCode = string.IsNullOrEmpty(request.CheckCode) ? response.CheckCode : request.CheckCode;
                if (request.SubmitGroup > 0)
                {
                    viewModel.Item.Source = request.SubmitGroup;
                }
                if (request.ShowJs == 0)
                {
                    viewModel.Item.JingSuanKouJing = null;
                }
                if (request.ShowOrderNo > 0)
                {
                    viewModel.Item.OrderNo = string.IsNullOrEmpty(response.SubmitInfo.orderNo) ? "" : response.SubmitInfo.orderNo;
                }
            }
            else
            {
                viewModel.Item = new SubmitInfoDetail()
                {
                    Source = request.SubmitGroup > 0 ? request.SubmitGroup : request.IntentionCompany
                };
                viewModel.OrderId = string.IsNullOrEmpty(request.OrderId) ? response.OrderId : request.OrderId;
                viewModel.CustKey = request.CustKey;
                viewModel.CheckCode = string.IsNullOrEmpty(request.CheckCode) ? response.CheckCode : request.CheckCode;
                viewModel.BusinessStatus = -10002;
                viewModel.StatusMessage = "获取核保消息失败";
                if (request.ShowOrderNo > 0)
                {
                    viewModel.Item.OrderNo = "";
                }
            }
            if (request.ShowOrderNo == 0)
            {
                viewModel.Item.OrderNo = null;
            }
            logInfo.Info("获取核保结果：" + viewModel.ToJson());
            return viewModel.ResponseToJson();
        }
        /// <summary>
        /// 取消核保 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [EnableThrottling()]
        public async Task<HttpResponseMessage> GetCancelSubmit([FromUri] GetCancelSubmitRequest request)
        {
            logInfo.Info(string.Format("获取核保信息请求串：{0}", Request.RequestUri));
            CancelSubmitViewModel viewModel = new CancelSubmitViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }
            if (string.IsNullOrWhiteSpace(request.BizNo) && string.IsNullOrWhiteSpace(request.ForceNo))
            {
                viewModel.BusinessStatus = -10000;
                viewModel.StatusMessage = "输入参数错误，投保单号不能全为空";
                return viewModel.ResponseToJson();
            }
            GetCancelSubmitResponse response = await _carInsuranceService.GetCancelSubmit(request, Request.GetQueryNameValuePairs());
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
            viewModel.BusinessStatus = response.BusinessStatus;
            viewModel.StatusMessage = response.BusinessMessage;
            viewModel.Result = response.Result;
            return viewModel.ResponseToJson();
        }
        #region 续保拆分
        [HttpGet]
        [ActionName("GetLicenseInfo")]
        [EnableThrottling()]
        public async Task<HttpResponseMessage> FetchLincenseInfo([FromUri]GetReInfoRequest request)
        {
            logInfo.Info(string.Format("续保接口请求串：{0}", Request.RequestUri));
            LicenseInfoViewModel viewModel = new LicenseInfoViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson(HttpStatusCode.Accepted);
            }
            if (string.IsNullOrWhiteSpace(request.LicenseNo))
            {
                viewModel.BusinessStatus = -10000;
                viewModel.StatusMessage = "输入参数错误，" + "请输入正确的车牌号"; ;
                return viewModel.ResponseToJson(HttpStatusCode.Accepted);
            }
            if (!request.LicenseNo.IsValidLicenseno())
            {
                viewModel.BusinessStatus = -10000;
                viewModel.StatusMessage = "输入参数错误，" + "请输入正确的车牌号"; ;
                return viewModel.ResponseToJson(HttpStatusCode.Accepted);

            }

            //var checkMsg = CheckXuBao(request);
            //if (!string.IsNullOrWhiteSpace(checkMsg))
            //{
            //    viewModel.BusinessStatus = -10000;
            //    string msg = checkMsg;
            //    viewModel.StatusMessage = "输入参数错误，" + msg;
            //    return viewModel.ResponseToJson(HttpStatusCode.Accepted);
            //}
            //针对 按照车架号和发动机号续保的内容
            // CheckLicensenoOrCarvin(request);
            try
            {
                var response = await _carInsuranceService.GetVenchileLincenseInfo(request, Request.GetQueryNameValuePairs());
                Task.Factory.StartNew(() =>
                {
                    _renewalStatusService.AddRenewalStatus(response.BusinessStatus, request);
                });

                viewModel.VehicleInfo = new LicenseViewModel
                {
                    CarVin = string.Empty,
                    EngineNo = string.Empty,
                    LicenseNo = request.LicenseNo,
                    MoldName = string.Empty,
                    RegisterDate = string.Empty,
                    CarOwnersName = string.Empty
                };
                if (response.Status == HttpStatusCode.BadRequest || response.Status == HttpStatusCode.Forbidden)
                {
                    viewModel.BusinessStatus = -10001;
                    if (!string.IsNullOrWhiteSpace(response.BusinessMessage))
                    {
                        viewModel.StatusMessage = response.BusinessMessage;
                    }
                    else
                    {
                        viewModel.StatusMessage = "参数校验错误，请检查您的校验码";
                    }

                    return viewModel.ResponseToJson(HttpStatusCode.Accepted);
                }
                if (response.Status == HttpStatusCode.ExpectationFailed)
                {
                    viewModel.BusinessStatus = -10003;
                    viewModel.StatusMessage = "服务发生异常";
                    return viewModel.ResponseToJson(HttpStatusCode.Accepted);
                }
                else
                {
                    viewModel.BusinessStatus = response.BusinessStatus;
                    viewModel.StatusMessage = response.BusinessMessage;
                    if (response.BusinessStatus == 1)
                    {
                        viewModel.VehicleInfo = new LicenseViewModel
                        {
                            CarVin = response.Carinfo.vin_no,
                            EngineNo = response.Carinfo.engine_no,
                            LicenseNo = response.Carinfo.license_no,
                            MoldName = response.Carinfo.mold_name,
                            RegisterDate = response.Carinfo.register_date.HasValue ? response.Carinfo.register_date.Value.ToString("yyyy-MM-dd") : string.Empty,
                            CarOwnersName = "",
                            CarType = response.Carinfo.car_type ?? 0,
                            CarUsedType = response.Carinfo.car_used_type ?? 0,
                            SeatCount = response.Carinfo.seat_count ?? 0,
                            RenewalCarType = response.Carinfo.RenewalCarType ?? 0,
                            ExhaustScale = (response.Carinfo.exhaust_scale ?? 0).ToString()
                        };
                    }
                }
                //独立增加排量信息
                if (request.CanShowExhaustScale == 0)
                {
                    if (viewModel.VehicleInfo != null)
                    {
                        viewModel.VehicleInfo.ExhaustScale = null;
                    }
                }
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            logInfo.Info("车辆续保信息记录:" + viewModel.ToJson());
            return viewModel.ResponseToJson();
        }
        [HttpGet]
        [ActionName("GetRenewalInfo")]
        [EnableThrottling()]
        public async Task<HttpResponseMessage> FetchRenewalInfo([FromUri]GetReInfoRequest request)
        {
            logInfo.Info(string.Format("续保拆分获取险种接口请求串：{0}", Request.RequestUri));

            GetReInfoViewModel viewModel = new GetReInfoViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson(HttpStatusCode.Accepted);
            }
            //if (!CheckLicenseno(request.LicenseNo, request.CarVin))
            //{
            //    viewModel.BusinessStatus = -10000;
            //    viewModel.StatusMessage = "输入参数错误，请检查您输入的车牌号或者carvin是否正确";
            //    return viewModel.ResponseToJson(HttpStatusCode.Accepted);
            //}
            var checkMsg = _checkReInfoService.CheckXuBao(request);
            if (!string.IsNullOrWhiteSpace(checkMsg))
            {
                viewModel.BusinessStatus = -10000;
                string msg = checkMsg;
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson(HttpStatusCode.Accepted);
            }
            //针对 按照车架号和发动机号续保的内容
            CheckLicensenoOrCarvin(request);
            try
            {
                GetReInfoResponse response = await _carInsuranceService.GetRenewalInfo(request, Request.GetQueryNameValuePairs());
                if (response.Status == HttpStatusCode.PartialContent)
                {
                    viewModel.BusinessStatus = -10001;
                    viewModel.StatusMessage = response.BusinessMessage;
                    return viewModel.ResponseToJson(HttpStatusCode.Accepted);
                }
                if (response.Status == HttpStatusCode.BadRequest || response.Status == HttpStatusCode.Forbidden)
                {
                    viewModel.BusinessStatus = -10001;
                    if (!string.IsNullOrWhiteSpace(response.BusinessMessage))
                    {
                        viewModel.StatusMessage = response.BusinessMessage;
                    }
                    else
                    {
                        viewModel.StatusMessage = "参数校验错误，请检查您的校验码";
                    }
                    return viewModel.ResponseToJson(HttpStatusCode.Accepted);
                }
                if (response.Status == HttpStatusCode.ExpectationFailed)
                {
                    viewModel.BusinessStatus = -10003;
                    viewModel.StatusMessage = "服务发生异常";
                    return viewModel.ResponseToJson(HttpStatusCode.Accepted);
                }
                else
                {
                    viewModel.BusinessStatus = response.BusinessStatus;
                    viewModel.StatusMessage = response.BusinessMessage;
                    viewModel.UserInfo = response.UserInfo.ConvertToViewModel(response.SaveQuote, response.CarInfo, response.LastInfo);
                    //显示商业险交强险投保单号 
                    if (request.CanShowNo == 0)
                    {
                        viewModel.UserInfo.BizNo = null;
                        viewModel.UserInfo.ForceNo = null;
                    }
                    if (request.CanShowExhaustScale == 0)
                    {
                        viewModel.UserInfo.ExhaustScale = null;
                    }
                    if (response.ReqCarinfo != null)
                    {
                        viewModel.UserInfo.IsPublic = response.ReqCarinfo.is_public.HasValue
                        ? response.ReqCarinfo.is_public.Value
                        : 0;
                    }
                    viewModel.UserInfo.CityCode = request.CityCode;
                    //if (response.UserInfo.OpenId.Length > 0)
                    //{
                    //    viewModel.CustKey = response.UserInfo.OpenId.Substring(response.UserInfo.OpenId.IndexOf("-", System.StringComparison.Ordinal)+1);
                    //}
                    viewModel.CustKey = request.CustKey;
                    if (response.BusinessStatus == 1)
                    {
                        viewModel.SaveQuote = response.SaveQuote.ConvetToViewModel();
                        if (request.Group > 0)
                        {
                            if (viewModel.SaveQuote.Source == 0)
                            {
                                viewModel.SaveQuote.Source = 2;
                            }
                            else if (viewModel.SaveQuote.Source == 1)
                            {
                                viewModel.SaveQuote.Source = 1;
                            }
                            else if (viewModel.SaveQuote.Source == 2)
                            {
                                viewModel.SaveQuote.Source = 4;
                            }
                            else if (viewModel.SaveQuote.Source == 3)
                            {
                                viewModel.SaveQuote.Source = 8;
                            }
                            else if (viewModel.SaveQuote.Source == 4)
                            {
                                viewModel.SaveQuote.Source = 16;
                            }
                            else if (viewModel.SaveQuote.Source == 5)
                            {
                                viewModel.SaveQuote.Source = 32;
                            }
                            else if (viewModel.SaveQuote.Source == 6)
                            {
                                viewModel.SaveQuote.Source = 64;
                            }
                            else if (viewModel.SaveQuote.Source == 7)
                            {
                                viewModel.SaveQuote.Source = 128;
                            }
                            else if (viewModel.SaveQuote.Source == 8)
                            {
                                viewModel.SaveQuote.Source = 256;
                            }
                            else if (viewModel.SaveQuote.Source == 9)
                            {
                                viewModel.SaveQuote.Source = 512;
                            }
                            else if (viewModel.SaveQuote.Source == 10)
                            {
                                viewModel.SaveQuote.Source = 1024;
                            }
                            else if (viewModel.SaveQuote.Source == 11)
                            {
                                viewModel.SaveQuote.Source = 2048;
                            }
                            else if (viewModel.SaveQuote.Source == 35)
                            {
                                viewModel.SaveQuote.Source = 34359738368;
                            }
                        }
                    }
                    else
                    {
                        viewModel.SaveQuote = new SaveQuoteViewModel();
                    }
                    if (response.BusinessStatus == 1)
                    {
                        viewModel.StatusMessage = "续保成功";
                    }
                    else if (response.BusinessStatus == 2)
                    {
                        viewModel.StatusMessage = "需要完善行驶证信息（车辆信息和险种都没有获取到）";
                    }
                    else if (response.BusinessStatus == 3)
                    {
                        viewModel.StatusMessage = "获取车辆信息成功(车架号，发动机号，品牌型号及初登日期)，险种获取失败";
                    }
                    else if (response.BusinessStatus == -10002)
                    {
                        viewModel.StatusMessage = "获取续保信息失败";
                    }
                    else if (response.BusinessStatus == 8)
                    {
                        viewModel.UserInfo.ForceExpireDate = response.LastInfo.last_end_date;
                        viewModel.UserInfo.BusinessExpireDate = response.LastInfo.last_business_end_date;
                        if (!string.IsNullOrWhiteSpace(viewModel.UserInfo.ForceExpireDate))
                        {
                            var nb = DateTime.Parse(viewModel.UserInfo.ForceExpireDate);
                            if (nb.Date == DateTime.MinValue.Date)
                            {
                                viewModel.UserInfo.ForceExpireDate = "";
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(viewModel.UserInfo.BusinessExpireDate))
                        {
                            var nb = DateTime.Parse(viewModel.UserInfo.BusinessExpireDate);
                            if (nb.Date == DateTime.MinValue.Date)
                            {
                                viewModel.UserInfo.BusinessExpireDate = "";
                            }
                        }
                        viewModel.StatusMessage = "投保公司：" + response.BusinessMessage + ";该车是续保期外的车或者是投保我司对接外的其他保险公司的车辆，这种情况，只能返回该车的投保日期(ForceExpireDate,BusinessExpireDate),险种取不到，不再返回";
                        viewModel.BusinessStatus = 1;
                    }
                    if (response.BusinessStatus != 1)
                    {
                        viewModel.SaveQuote.Source = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            logInfo.Info("车辆续保信息记录:" + viewModel.ToJson());
            return viewModel.ResponseToJson();
        }
        #endregion
        /// <summary>
        /// 获取车型信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetVehicleInfo")]
        //[EnableThrottling()]
        public async Task<HttpResponseMessage> FetchVehicleInfo([FromUri]GetCarVehicleRequest request)
        {
            logInfo.Info(string.Format("获取车型信息接口请求串：{0}", Request.RequestUri));
            var viewModel = new CarVehicleInfoViewModel();
            viewModel.CustKey = request.CustKey;
            viewModel.Items = new List<ICarVehicleItem>();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson(HttpStatusCode.Accepted);
            }

            try
            {
                GetCarVehicleInfoResponse response = await _carInsuranceService.GetCarVehicle(request, Request.GetQueryNameValuePairs());

                if (response.Status == HttpStatusCode.PartialContent)
                {
                    viewModel.BusinessStatus = -10001;
                    viewModel.StatusMessage = response.BusinessMessage;
                    return viewModel.ResponseToJson(HttpStatusCode.Accepted);
                }
                if (response.Status == HttpStatusCode.BadRequest || response.Status == HttpStatusCode.Forbidden)
                {
                    viewModel.BusinessStatus = -10001;
                    if (!string.IsNullOrWhiteSpace(response.BusinessMessage))
                    {
                        viewModel.StatusMessage = response.BusinessMessage;
                    }
                    else
                    {
                        viewModel.StatusMessage = "参数校验错误，请检查您的校验码";
                    }

                    return viewModel.ResponseToJson(HttpStatusCode.Accepted);
                }
                if (response.Status == HttpStatusCode.ExpectationFailed)
                {
                    viewModel.BusinessStatus = -10003;
                    viewModel.StatusMessage = "服务发生异常";
                    return viewModel.ResponseToJson(HttpStatusCode.Accepted);
                }
                else
                {
                    viewModel.BusinessStatus = response.BusinessStatus;
                    viewModel.StatusMessage = response.BusinessMessage;
                    if (response.BusinessStatus == 1)
                    {
                        viewModel.Items = response.Vehicles.ConvertToCarVehicleItems(request.ResultFormat);
                        viewModel.StatusMessage = "获取成功";
                    }
                }
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            var result = viewModel.ResponseToJsonReplaceType();

            return result;
        }

        /// <summary>
        /// 获取车型信息第二步
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetNewVehicleInfo")]
        public async Task<HttpResponseMessage> FetchNewVehicleInfo([FromUri]GetVehicleRequest request)
        {
            logInfo.Info(string.Format("获取车型信息接口请求串：{0}", Request.RequestUri));
            var viewModel = new NewCarVehicleInfoViewModel();
            viewModel.CustKey = request.CustKey;
            viewModel.Items = new List<NewCarVehicleDetail>();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson(HttpStatusCode.Accepted);
            }
            if (string.IsNullOrWhiteSpace(request.MoldName) && string.IsNullOrWhiteSpace(request.CarVin))
            {
                viewModel.BusinessStatus = -10000;
                string msg = "品牌型号或者车架号不能都为空";
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson(HttpStatusCode.Accepted);
            }
            try
            {
                GetNewCarVehicleInfoResponse response = await _getNewVehicleInfoService.GetCarVehicle(request, Request.GetQueryNameValuePairs());

                if (response.Status == HttpStatusCode.PartialContent)
                {
                    viewModel.BusinessStatus = -10001;
                    viewModel.StatusMessage = response.BusinessMessage;
                    return viewModel.ResponseToJson(HttpStatusCode.Accepted);
                }
                if (response.Status == HttpStatusCode.BadRequest || response.Status == HttpStatusCode.Forbidden)
                {
                    viewModel.BusinessStatus = -10001;
                    if (!string.IsNullOrWhiteSpace(response.BusinessMessage))
                    {
                        viewModel.StatusMessage = response.BusinessMessage;
                    }
                    else
                    {
                        viewModel.StatusMessage = "参数校验错误，请检查您的校验码";
                    }
                    return viewModel.ResponseToJson(HttpStatusCode.Accepted);
                }
                if (response.Status == HttpStatusCode.ExpectationFailed)
                {
                    viewModel.BusinessStatus = -10003;
                    viewModel.StatusMessage = "服务发生异常";
                    return viewModel.ResponseToJson(HttpStatusCode.Accepted);
                }
                else
                {
                    viewModel.BusinessStatus = response.BusinessStatus;
                    viewModel.StatusMessage = response.BusinessMessage;
                    if (response.BusinessStatus == 1)
                    {
                        viewModel.Items = response.Vehicles.ConvertToCarVehicleItems();
                        viewModel.StatusMessage = "获取成功";
                    }
                }
            }
            catch (Exception ex)
            {
                MetricUtil.UnitReports("GetNewVehicleInfo_controller");
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException + ";请求串是：" + Request.RequestUri);
            }
            var result = viewModel.ResponseToJsonReplaceType();

            return result;
        }
        [HttpGet]
        [ActionName("GetSecondVehicleInfo")]
        //[EnableThrottling()]
        public async Task<HttpResponseMessage> FetchSecondVehicleInfo([FromUri]GetNewCarSecondVehicleRequest request)
        {
            logInfo.Info(string.Format("新车第二次获取新车车型信息接口请求串：{0}", Request.RequestUri));
            var viewModel = new CarVehicleInfoViewModel();
            viewModel.CustKey = request.CustKey;
            viewModel.Items = new List<ICarVehicleItem>();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson(HttpStatusCode.Accepted);
            }
            try
            {
                GetCarVehicleInfoResponse response = await _getSecondVehicleInfoService.GetSecondCarVehicle(request, Request.GetQueryNameValuePairs());
                if (response.Status == HttpStatusCode.PartialContent)
                {
                    viewModel.BusinessStatus = -10001;
                    viewModel.StatusMessage = response.BusinessMessage;
                    return viewModel.ResponseToJson(HttpStatusCode.Accepted);
                }
                if (response.Status == HttpStatusCode.BadRequest || response.Status == HttpStatusCode.Forbidden)
                {
                    viewModel.BusinessStatus = -10001;
                    if (!string.IsNullOrWhiteSpace(response.BusinessMessage))
                    {
                        viewModel.StatusMessage = response.BusinessMessage;
                    }
                    else
                    {
                        viewModel.StatusMessage = "参数校验错误，请检查您的校验码";
                    }
                    return viewModel.ResponseToJson(HttpStatusCode.Accepted);
                }
                if (response.Status == HttpStatusCode.ExpectationFailed)
                {
                    viewModel.BusinessStatus = -10004;
                    viewModel.StatusMessage = "服务发生异常";
                    return viewModel.ResponseToJson(HttpStatusCode.Accepted);
                }
                else
                {
                    viewModel.BusinessStatus = response.BusinessStatus;
                    viewModel.StatusMessage = response.BusinessMessage;
                    viewModel.Items = new List<ICarVehicleItem>();
                    if (response.BusinessStatus == 1)
                    {
                        viewModel.Items = response.Vehicles.ConvertToCarVehicleItems(1);
                        viewModel.StatusMessage = "获取成功";
                    }
                    else if (response.BusinessStatus == -10002)
                    {
                        viewModel.StatusMessage = "非新车,禁止按照新车报价";
                    }
                    else if (response.BusinessStatus == -10003)
                    {
                        viewModel.StatusMessage = "当前选择车型与平台返回信息不一致（输入参数可能不匹配）";
                    }
                    else if (response.BusinessStatus == -10004)
                    {
                        viewModel.StatusMessage = "保险公司服务发生异常";
                    }
                }
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            var result = viewModel.ResponseToJsonReplaceType();

            return result;
        }
        [HttpGet]
        [ActionName("GetFirstVehicleInfo")]
        //[EnableThrottling()]
        public async Task<HttpResponseMessage> FetchFirstVehicleInfo([FromUri]GetNewCarVehicleRequest request)
        {
            logInfo.Info(string.Format("获取新车车型信息接口请求串：{0}", Request.RequestUri));
            var viewModel = new CarVehicleInfoViewModel();
            viewModel.CustKey = request.CustKey;
            viewModel.Items = new List<ICarVehicleItem>();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson(HttpStatusCode.Accepted);
            }
            try
            {
                GetCarVehicleInfoResponse response = await _getFirstVehicleInfoService.GetNewCarVehicle(request, Request.GetQueryNameValuePairs());
                if (response.Status == HttpStatusCode.PartialContent)
                {
                    viewModel.BusinessStatus = -10001;
                    viewModel.StatusMessage = response.BusinessMessage;
                    return viewModel.ResponseToJson(HttpStatusCode.Accepted);
                }
                if (response.Status == HttpStatusCode.BadRequest || response.Status == HttpStatusCode.Forbidden)
                {
                    viewModel.BusinessStatus = -10001;
                    if (!string.IsNullOrWhiteSpace(response.BusinessMessage))
                    {
                        viewModel.StatusMessage = response.BusinessMessage;
                    }
                    else
                    {
                        viewModel.StatusMessage = "参数校验错误，请检查您的校验码";
                    }
                    return viewModel.ResponseToJson(HttpStatusCode.Accepted);
                }
                if (response.Status == HttpStatusCode.ExpectationFailed)
                {
                    viewModel.BusinessStatus = -10003;
                    viewModel.StatusMessage = "服务发生异常";
                    return viewModel.ResponseToJson(HttpStatusCode.Accepted);
                }
                else
                {
                    viewModel.BusinessStatus = response.BusinessStatus;
                    viewModel.StatusMessage = response.BusinessMessage;
                    viewModel.Items = new List<ICarVehicleItem>();
                    if (response.BusinessStatus == 1)
                    {
                        viewModel.Items = response.Vehicles.ConvertToCarVehicleItems(1);
                        viewModel.StatusMessage = "获取成功";
                    }
                    else if (response.BusinessStatus == -10002)
                    {
                        viewModel.StatusMessage = "非新车,禁止按照新车报价";
                    }
                    else if (response.BusinessStatus == -10003)
                    {
                        viewModel.StatusMessage = "当前选择车型与平台返回信息不一致（输入参数可能不匹配）";
                    }
                    else if (response.BusinessStatus == -10004)
                    {
                        viewModel.StatusMessage = "保险公司服务发生异常";
                    }
                }
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            var result = viewModel.ResponseToJsonReplaceType();
            return result;
        }
        [HttpGet]
        public async Task<HttpResponseMessage> GetMoldName([FromUri] GetMoldNameRequest request)
        {
            logInfo.Info(string.Format("车型报价校验请求串：{0}", Request.RequestUri));
            var viewModel = new GetMoldNameViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson(HttpStatusCode.Accepted);
            }
            try
            {
                GetMoldNameResponse response = await _carInsuranceService.GetMoldName(request, Request.GetQueryNameValuePairs());

                if (response.Status == HttpStatusCode.PartialContent)
                {
                    viewModel.BusinessStatus = -10001;
                    viewModel.StatusMessage = response.BusinessMessage;
                    return viewModel.ResponseToJson(HttpStatusCode.Accepted);
                }
                if (response.Status == HttpStatusCode.BadRequest || response.Status == HttpStatusCode.Forbidden)
                {
                    viewModel.BusinessStatus = -10001;
                    viewModel.StatusMessage = "参数校验错误，请检查您的校验码";
                    return viewModel.ResponseToJson(HttpStatusCode.Accepted);
                }
                if (response.Status == HttpStatusCode.ExpectationFailed)
                {
                    viewModel.BusinessStatus = -10004;
                    viewModel.StatusMessage = "服务发生异常";
                    return viewModel.ResponseToJson(HttpStatusCode.Accepted);
                }
                else
                {
                    viewModel.BusinessStatus = response.BusinessStatus;
                    viewModel.StatusMessage = response.BusinessMessage;
                    viewModel.MoldName = response.MoldName;
                }
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            var result = viewModel.ResponseToJsonReplaceType();

            return result;
        }
        [HttpGet]
        public async Task<HttpResponseMessage> CheckVehicle([FromUri] GetNewCarSecondVehicleRequest request)
        {
            logInfo.Info(string.Format("车型报价校验请求串：{0}", Request.RequestUri));
            var viewModel = new CheckCarVehicleInfoViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson(HttpStatusCode.Accepted);
            }
            try
            {
                request.IsCheckVehicleNo = 1;
                CheckCarVehicleInfoResponse response = await _carInsuranceService.CheckCarVehicle(request, Request.GetQueryNameValuePairs());
                viewModel.CheckCode = -1;
                viewModel.CheckMsg = string.Empty;
                if (response.Status == HttpStatusCode.PartialContent)
                {
                    viewModel.BusinessStatus = -10001;
                    viewModel.StatusMessage = response.BusinessMessage;
                    return viewModel.ResponseToJson(HttpStatusCode.Accepted);
                }
                if (response.Status == HttpStatusCode.BadRequest || response.Status == HttpStatusCode.Forbidden)
                {
                    viewModel.BusinessStatus = -10001;
                    if (!string.IsNullOrWhiteSpace(response.BusinessMessage))
                    {
                        viewModel.StatusMessage = response.BusinessMessage;
                    }
                    else
                    {
                        viewModel.StatusMessage = "参数校验错误，请检查您的校验码";
                    }
                    return viewModel.ResponseToJson(HttpStatusCode.Accepted);
                }
                viewModel.CarType = response.CarType ?? string.Empty;
                viewModel.TypeName = response.TypeName ?? string.Empty;
                if (response.Status == HttpStatusCode.ExpectationFailed)
                {
                    viewModel.BusinessStatus = -10004;
                    viewModel.StatusMessage = "服务发生异常";

                    return viewModel.ResponseToJson(HttpStatusCode.Accepted);
                }
                else
                {
                    viewModel.BusinessStatus = response.BusinessStatus;
                    viewModel.StatusMessage = response.BusinessMessage;
                    if (response.BusinessStatus == 1)
                    {
                        viewModel.CheckCode = response.CheckCode;
                        viewModel.CheckMsg = response.CheckMessage;
                        //viewModel.DriveLicenseCarTypeName = response.DriveLicenseCarTypeName;
                        //viewModel.DriveLicenseCarTypeValue = response.DriveLicenseCarTypeValue;
                        viewModel.StatusMessage = "获取成功";
                    }
                    else if (response.BusinessStatus == -1)
                    {
                        viewModel.BusinessStatus = -10004;
                        viewModel.StatusMessage = "请求超时";
                        viewModel.CheckCode = -1;
                        viewModel.CheckMsg = "超时了 ，请稍候重试";
                    }
                }

                if (request.ShowCarType == 0)
                {
                    viewModel.CarType = null;
                    viewModel.TypeName = null;
                }
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            var result = viewModel.ResponseToJsonReplaceType();
            return result;
        }
        private async Task<PostPrecisePriceResponse> PostPreciseFactory(PostPrecisePriceRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            PostPrecisePriceResponse response;
            if (!string.IsNullOrEmpty(request.CarVin) && !string.IsNullOrWhiteSpace(request.EngineNo) &&
                (!string.IsNullOrWhiteSpace(request.MoldName) || !string.IsNullOrWhiteSpace(request.MoldNameUrlEncode)) && !string.IsNullOrWhiteSpace(request.RegisterDate))
            {
                response = await _carInsuranceService.UpdateDrivingLicense(request, pairs);
            }
            else
            {
                response = await _carInsuranceService.InsertUserInfo(request, pairs);
            }
            return response;
        }
        #region test
        [HttpGet]
        public async Task<HttpResponseMessage> Test(string custkey, int agent, string licenseno)
        {
            try
            {
                string str = string.Empty;
                var rootId = System.Web.HttpUtility.UrlEncode(string.Format("{0}_{1}_{2}", custkey, agent, licenseno));
                using (HttpClient client = new HttpClient())
                {
                    // client.BaseAddress = new Uri("http://i.91bihu.com/");
                    //client.Timeout = TimeSpan.FromSeconds(1400);
                    var Dc = new DmContext
                    {
                        RootId = rootId,
                        ParentId = 1,
                        ChildId = 1,
                        //Tag =System.Web.HttpUtility.UrlEncode("京NR4923")
                    };
                    var Msg = new Message()
                    {
                        ActionName = "Test",
                        Body = "请求串",
                        ExecuteTime = DateTime.Now,
                        ServerIp = NetworkInterfaceManager.GetLocalHostAddress()
                    };
                    DmContainer cm = new DmContainer()
                    {
                        Dc = Dc,
                        Msg = Msg
                    };
                    client.DefaultRequestHeaders.Add("DM", Dc.ToJson());
                    IDmCacheOperator trace = new DmCacheOperator();
                    trace.AddTrace(cm);
                    logInfo.Info("进入请求：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:dd"));
                    var getUrl =
                        "http://localhost:42901/api/CarInsurance/getreinfo?LicenseNo=%E4%BA%ACLD2076&CityCode=1&Agent=102&IsPublic=0&CustKey=19900068199&RenewalType=3&SecCode=1a712c1bdfd80f277c402fe7c45b07d8";
                    var clientResult = client.GetAsync(getUrl).Result;
                    //DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                    //DateTime dtNow = DateTime.Parse(DateTime.Now.ToString());
                    //TimeSpan toNow = dtNow.Subtract(dtStart);
                    //string timeStamp = toNow.Ticks.ToString();
                    //timeStamp = timeStamp.Substring(0, timeStamp.Length - 7);
                    //var _from = "bihu";
                    //var _nonce = "abcdefgh";
                    //var _timestamp = timeStamp;
                    //var queryString = "data=sfdfdfdfdsfdfdfsf";
                    //var request = new
                    //{
                    //    _from = "bihu",
                    //    _nonce = "abcdefgh",
                    //    _timestamp = timeStamp,
                    //    _sign = (queryString.GetMd5() + _nonce + _timestamp + _from +
                    //            "NmQ0YzhmNzhlZmM1OWNk").GetMd5().ToUpper(),
                    //    data = "sfdfdfdfdsfdfdfsf"
                    //};
                    //var data = CommonHelper.ReverseEachProperties(request);
                    //var postData = new System.Net.Http.FormUrlEncodedContent(data);
                    //var clientResult =
                    //    await client.PostAsync("http://210.13.242.24:7001/api/insurance/updatePrice", postData);
                    ////var rr2 = response2.Content.ReadAsStringAsync().Result;
                    //if (clientResult.IsSuccessStatusCode)
                    //{
                    //    var raw_response = await clientResult.Content.ReadAsByteArrayAsync();
                    //    str = Encoding.Default.GetString(await clientResult.Content.ReadAsByteArrayAsync(), 0, raw_response.Length);
                    //}
                }
                var t = str;
            }
            catch (Exception exception)
            {
                logInfo.Info("请求超时：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:dd"));

            }

            return new HttpResponseMessage();
        }
        #endregion

        /// <summary>
        /// 校验车牌号和发动机号
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private GetReInfoRequest CheckLicensenoOrCarvin(GetReInfoRequest request)
        {
            bool isCarLicenseno = true;
            if (!string.IsNullOrWhiteSpace(request.LicenseNo))
            {
                request.LicenseNo = request.LicenseNo.ToUpper();
                isCarLicenseno = request.LicenseNo.IsValidLicenseno();
            }
            else
            {
                isCarLicenseno = false;
            }
            if (isCarLicenseno == false)
            {
                request.IsLastYearNewCar = 2;
                request.LicenseNo = request.CarVin.ToUpper();
            }
            else
            {
                request.IsLastYearNewCar = 1;
            }
            return request;
        }

        [HttpGet]
        public string Ts([FromUri]int num)
        {
            IMySqlMonitorService service = new MySqlMonitorService();
            service.GetDate();
            try
            {
                _carInsuranceService.Test(num);
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }

            return "ok";
        }

        /// <summary>
        /// 获取折扣价格
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [EnableCommonThrottling(PerSecond = 1, PerMinute = 15, PerHour = 50)]
        public async Task<HttpResponseMessage> GetDepreciationPrice([FromUri]GetDepreciationPriceRequest request)
        {
            logInfo.Info(string.Format("获取折扣价格请求串：{0}", Request.RequestUri));
            DepreciationPriceViewModel model = new DepreciationPriceViewModel();
            if (!ModelState.IsValid)
            {
                model.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                model.StatusMessage = "输入参数错误，" + msg;
                return model.ResponseToJson(HttpStatusCode.Accepted);
            }
            if (request.PurchasePrice <= 0 || string.IsNullOrWhiteSpace(request.Bizstartdate) ||
                string.IsNullOrWhiteSpace(request.Registerdate))
            {
                model.BusinessStatus = -10000;
                model.StatusMessage = "输入的参数是否有空或者长度不符合要求";
                model.Item = new DepreciationPriceItem();
            }
            else
            {
                model = await GetPrice(request.Bizstartdate, request.Registerdate, request.PurchasePrice, request.CarType);
            }
            return model.ResponseToJson();
        }
        private async Task<DepreciationPriceViewModel> GetPrice(string bizstartdate, string registerdate, decimal purchasePrice, int? cartype = 0)
        {
            decimal ufp = decimal.Parse(string.IsNullOrWhiteSpace(System.Configuration.ConfigurationManager.AppSettings["upFloatPoint"]) ? "0.3" : System.Configuration.ConfigurationManager.AppSettings["upFloatPoint"]);
            decimal dfp = decimal.Parse(string.IsNullOrWhiteSpace(System.Configuration.ConfigurationManager.AppSettings["downFloatPoint"]) ? "0.3" : System.Configuration.ConfigurationManager.AppSettings["downFloatPoint"]);
            var model = new DepreciationPriceViewModel { Item = new DepreciationPriceItem() };

            if (!string.IsNullOrWhiteSpace(bizstartdate))
            {
                bizstartdate = bizstartdate.UnixTimeToDateTime().ToString("yyyy-MM-dd HH") + ":00:00";
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baoxianCenter);
                string requestStr =
                    string.Format("api/carmodel/GetDepreciationPrice?price={0}&registerdate={1}&startdate={2}&CarType={3}",
                        purchasePrice, registerdate, bizstartdate, cartype);
                HttpResponseMessage res = await client.GetAsync(requestStr);
                try
                {
                    if (res.IsSuccessStatusCode)
                    {
                        var response = await res.Content.ReadAsStringAsync();
                        var item = response.FromJson<DepriciatePriceModel>();
                        if (item.BusinessStatus == 0)
                        {
                            model.BusinessStatus = 1;
                            model.Item.DepreciationPrice = item.DepreciationPrice;
                            model.Item.UpPrice = item.DepreciationPrice * (1 + ufp);
                            model.Item.DownPrice = item.DepreciationPrice * (1 - dfp);
                            model.StatusMessage = "成功";
                        }
                        else
                        {
                            model.BusinessStatus = -10001;
                            model.StatusMessage = "中心计算失败";
                        }
                    }
                    else
                    {
                        model.BusinessStatus = -10001;
                        model.StatusMessage = "返回超时";
                        logError.Info("获取折扣价格返回超时:" + requestStr);
                    }
                }
                catch (Exception ex)
                {
                    model.BusinessStatus = -10001;
                    model.StatusMessage = "发生异常";
                    logError.Info("获取折扣价格返回超时:" + requestStr + ",异常消息：" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
                }
            }
            return model;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet, ActionName("PostSubmitInfo")]
        [EnableThrottling()]
        public async Task<HttpResponseMessage> PostSubmitInfo([FromUri]PostSubmitInfoRequest request)
        {
            BaseViewModel viewModel = new BaseViewModel();
            //基础校验
            logInfo.Info(string.Format("重新请求核保信息接口请求串：{0}", Request.RequestUri));
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson(HttpStatusCode.Accepted);
            }
            if (request.ChildAgent == 0)
            {
                request.ChildAgent = request.Agent;
            }
            //主体函数
            PostSubmitInfoResponse response = _postSubmitInfoService.PostSubmitInfo(request, Request.GetQueryNameValuePairs());
            //返回值判断
            if (response.Status == HttpStatusCode.Forbidden)
            {
                viewModel.BusinessStatus = -10001;
                viewModel.StatusMessage = !string.IsNullOrWhiteSpace(response.ErrMsg) ? response.ErrMsg : "参数校验错误，请检查您的校验码";
                return viewModel.ResponseToJson(HttpStatusCode.Accepted);
            }
            if (response.Status == HttpStatusCode.NotAcceptable)
            {
                viewModel.BusinessStatus = -10005;
                viewModel.StatusMessage = "参数校验错误，请确认您之前在该渠道报价/核保过";
                return viewModel.ResponseToJson(HttpStatusCode.Accepted);
            }
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


        /// <summary>
        /// 获取核保信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet, ActionName("GetRepeatSubmitDetail")]
        public async Task<HttpResponseMessage> GetRepeatSubmitDetail([FromUri]GetRepeatSubmitRequest request)
        {
            logInfo.Info(string.Format("获取重复投保请求串：{0}", Request.RequestUri));
            GetRepeatSubmitViewModel viewModel = new GetRepeatSubmitViewModel();
            viewModel.RepeatInfo = new GetRepeatSubmitInfo()
            {
                BusinessExpireDate = string.Empty,
                ForceExpireDate = string.Empty,
                RepeatSubmitResult = -1,
                RepeatSubmitMessage = string.Empty
            };

            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }
            GetRepeatSubmitResponse response = await _carInsuranceService.GetRepeatSubmitInfo(request, Request.GetQueryNameValuePairs());
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

            viewModel = response.RepeatSubmitConvertToViewModel();

            return viewModel.ResponseToJson();
        }

        /// <summary>
        /// 保存新车备案
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> RecordNewCar([FromBody]RecordNewCarRequest request)
        {
            logInfo.Info(string.Format("新增新车备案请求串：{0}", request.ToJson()));
            BaseViewModel viewModel = new BaseViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson(HttpStatusCode.Accepted);
            }
            viewModel = await _recordNewCarService.RecordNewCar(request);
            return viewModel.ResponseToJson();
        }
        [HttpGet]
        public async Task<HttpResponseMessage> RecordNewCarGet([FromUri]RecordNewCarRequest request)
        {
            logInfo.Info(string.Format("新增新车备案请求串：{0}", Request.RequestUri));
            BaseViewModel viewModel = new BaseViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson(HttpStatusCode.Accepted);
            }
            viewModel = await _recordNewCarService.RecordNewCar(request);
            return viewModel.ResponseToJson();
        }

        /// <summary>
        /// 获取是否新车备案
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<HttpResponseMessage> GetRecordNewCar([FromUri]GetRecordNewCarRequest request)
        {
            logInfo.Info(string.Format("获取是否新车备案请求串：{0}", Request.RequestUri));
            GetRecordNewCarViewModel viewModel = new GetRecordNewCarViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson(HttpStatusCode.Accepted);
            }
            viewModel = await _recordNewCarService.GetRecordNewCar(request);
            return viewModel.ResponseToJson();
        }

        /// <summary>
        /// 获取简易续保信息，包括车五项和推荐险种
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetIntelligentReInfo")]
        [EnableThrottling()]
        public async Task<HttpResponseMessage> GetIntelligentReInfo([FromUri]GetIntelligentReInfoRequest request)
        {
            logInfo.Info(string.Format("获取简易续保信息请求串：{0}", Request.RequestUri));
            #region 初始化返回对象信息
            GetIntelligentReInfoViewModel viewModel = new GetIntelligentReInfoViewModel();
            viewModel.VehicleInfo = new LicenseViewModel
            {
                CarVin = string.Empty,
                EngineNo = string.Empty,
                LicenseNo = request.LicenseNo,
                MoldName = string.Empty,
                RegisterDate = string.Empty,
                CarOwnersName = null
            };
            viewModel.SaveQuote = new SaveQuoteViewModel()
            {
                HcXiuLiChang = "0",
                HcXiuLiChangType = "-1",
                Fybc = "0",
                FybcDays = "0",
                SheBeis = new List<SheBei>(),
                SheBeiSunShi = "0",
                BjmSheBeiSunShi = "0",
                SanZheJieJiaRi = "0",
            };
            #endregion
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson(HttpStatusCode.Accepted);
            }
            try
            {
                var response = await _getIntelligentReInfoService.GetIntelligentReInfo(request, Request.GetQueryNameValuePairs());
                if (response.Status == HttpStatusCode.BadRequest || response.Status == HttpStatusCode.Forbidden)
                {
                    viewModel.BusinessStatus = -10001;
                    if (!string.IsNullOrWhiteSpace(response.ErrMsg))
                    {
                        viewModel.StatusMessage = response.ErrMsg;
                    }
                    else
                    {
                        viewModel.StatusMessage = "参数校验错误，请检查您的校验码";
                    }

                    return viewModel.ResponseToJson(HttpStatusCode.Accepted);
                }
                if (response.Status == HttpStatusCode.ExpectationFailed)
                {
                    viewModel.BusinessStatus = -10003;
                    viewModel.StatusMessage = "服务发生异常";
                    return viewModel.ResponseToJson(HttpStatusCode.Accepted);
                }
                else
                {
                    viewModel.BusinessStatus = response.ErrCode;
                    viewModel.StatusMessage = response.ErrMsg;
                    if (response.ErrCode == 1 || response.ErrCode == 2)
                    {
                        viewModel.VehicleInfo = new LicenseViewModel
                        {
                            CarVin = response.CarInfo.vin_no,
                            EngineNo = response.CarInfo.engine_no,
                            LicenseNo = response.CarInfo.license_no,
                            MoldName = response.CarInfo.mold_name,
                            RegisterDate = response.CarInfo.register_date.HasValue ? response.CarInfo.register_date.Value.ToString("yyyy-MM-dd") : string.Empty,
                            CarOwnersName = null,
                            CarType = response.CarInfo.car_type ?? 0,
                            CarUsedType = response.CarInfo.car_used_type ?? 0,
                            SeatCount = response.CarInfo.seat_count ?? 0,
                            RenewalCarType = response.CarInfo.RenewalCarType ?? 0,
                            ExhaustScale = (response.CarInfo.exhaust_scale ?? 0).ToString()
                        };
                        if (response.SaveQuote != null)
                        {
                            viewModel.SaveQuote = response.SaveQuote;
                            if (viewModel.SaveQuote != null)
                            {
                                viewModel.SaveQuote.Source = 0;
                            }
                        }
                    }
                }
                //独立增加排量信息
                if (request.CanShowExhaustScale == 0)
                {
                    if (viewModel.VehicleInfo != null)
                    {
                        viewModel.VehicleInfo.ExhaustScale = null;
                    }
                }
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
                viewModel.BusinessStatus = -10003;
                viewModel.StatusMessage = "服务发生异常";
                return viewModel.ResponseToJson(HttpStatusCode.Accepted);
            }
            logInfo.Info("车辆续保信息记录:" + viewModel.ToJson());
            return viewModel.ResponseToJson();
        }
    }
}