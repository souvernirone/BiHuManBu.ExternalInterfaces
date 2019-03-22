using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using log4net;
using ServiceStack.Text;

namespace BiHuManBu.ExternalInterfaces.Services
{
    public class AppAchieveService : CommonBehaviorService, IAppAchieveService
    {
        private IAgentRepository _agentRepository;
        private IUserInfoRepository _userInfoRepository;
        private IAppVerifyService _appVerifyService;
        private IAgentService _agentService;
        private IWorkOrderService _workOrderService;
        private IBjdService _bjdService;
        private INoticexbService _noticexbService;
        private ILog logError;

        public AppAchieveService(IAgentService agentService, IUserInfoRepository userInfoRepository, IAppVerifyService appVerifyService, IAgentRepository agentRepository, ICacheHelper cacheHelper, IWorkOrderService workOrderService, IBjdService bjdService, INoticexbService noticexbService)
            : base(agentRepository, cacheHelper)
        {
            _agentService = agentService;
            _userInfoRepository = userInfoRepository;
            _appVerifyService = appVerifyService;
            _workOrderService = workOrderService;
            _bjdService = bjdService;
            _noticexbService = noticexbService;
            logError = LogManager.GetLogger("ERROR");
        }

        #region 续保、报价、核保 相关
        public async Task<GetReInfoNewViewModel> GetReInfo(GetReInfoRequest request, IEnumerable<KeyValuePair<string, string>> pairs, Uri uri)
        {
            var viewModel = new GetReInfoNewViewModel();
            #region 参数校验
            //校验请求串
            var baseRequest = new AppBaseRequest()
            {
                Agent = request.Agent,
                SecCode = request.SecCode,
                CustKey = request.CustKey,
                BhToken = request.BhToken,
                ChildAgent = request.ChildAgent
            };
            //校验返回值
            var baseResponse = _appVerifyService.Verify(baseRequest, pairs);
            if (baseResponse.ErrCode != 1)
            {
                viewModel.BusinessStatus = baseResponse.ErrCode;
                viewModel.StatusMessage = baseResponse.ErrMsg;
                return viewModel;
            }
            #endregion
            #region 业务逻辑
            //拼接请求串
            string strUrl = string.Format("http://{0}:{1}/api/CarInsurance/GetReInfo", uri.Host, uri.Port);
            //3,续保请求
            BaseResponse rep = await SimulateGet(strUrl, pairs);
            if (rep.ErrCode == 1)
            {
                viewModel = rep.ErrMsg.FromJson<GetReInfoNewViewModel>();
            }
            else
            {
                viewModel.BusinessStatus = rep.ErrCode;
                viewModel.StatusMessage = rep.ErrMsg;
            }
            bx_userinfo userinfo = _userInfoRepository.FindByOpenIdAndLicense(request.CustKey, request.LicenseNo, request.ChildAgent.ToString(),request.RenewalCarType);
            viewModel.Buid = userinfo != null ? userinfo.Id : 0;
            viewModel.Agent = baseResponse.Agent;
            viewModel.AgentName = baseResponse.AgentName;
            viewModel.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            viewModel.IsDistrib = userinfo != null ? userinfo.IsDistributed : 0;
            //添加续保通知，向bx_notice_xb插记录，商业险或交强险在90天范围之内
            //20170111产品说再改吧，又不需要app的通知了
            //if (userinfo != null)
            //{
            //    long noticeBuid = userinfo.Id;
            //    _noticexbService.AddNoticexb(viewModel.UserInfo.LicenseNo, viewModel.UserInfo.BusinessExpireDate,
            //        viewModel.UserInfo.ForceExpireDate, viewModel.UserInfo.NextBusinessStartDate,
            //        viewModel.UserInfo.NextForceStartDate, viewModel.SaveQuote.Source, int.Parse(userinfo.Agent), noticeBuid, 1);
            //}
            #endregion
            return viewModel;
        }

        public async Task<BaseViewModel> PostPrecisePrice(PostPrecisePriceRequest request, IEnumerable<KeyValuePair<string, string>> pairs, Uri uri)
        {
            //返回对象
            var viewModel = new BaseViewModel();
            #region 参数校验
            //校验请求串
            var baseRequest = new AppBaseRequest()
            {
                Agent = request.Agent,
                SecCode = request.SecCode,
                CustKey = request.CustKey,
                BhToken = request.BhToken,
                ChildAgent = request.ChildAgent
            };
            //校验返回值
            var baseResponse = _appVerifyService.Verify(baseRequest, pairs);
            if (baseResponse.ErrCode != 1)
            {
                viewModel.BusinessStatus = baseResponse.ErrCode;
                viewModel.StatusMessage = baseResponse.ErrMsg;
                return viewModel;
            }
            #endregion
            #region 业务逻辑
            //拼接请求串
            string strUrl = string.Format("http://{0}:{1}/api/CarInsurance/PostPrecisePrice", uri.Host, uri.Port);
            //3,请求报价/核保
            BaseResponse rep = await SimulateGet(strUrl, pairs);
            if (rep.ErrCode == 1)
            {
                viewModel = rep.ErrMsg.FromJson<BaseViewModel>();
            }
            else
            {
                viewModel.BusinessStatus = rep.ErrCode;
                viewModel.StatusMessage = rep.ErrMsg;
            }
            #endregion
            return viewModel;
        }

        public async Task<GetPrecisePriceNewViewModel> GetPrecisePrice(GetPrecisePriceRequest request, IEnumerable<KeyValuePair<string, string>> pairs, Uri uri)
        {
            var viewModel = new GetPrecisePriceNewViewModel();
            #region 参数校验
            //校验请求串
            var baseRequest = new AppBaseRequest()
            {
                Agent = request.Agent,
                SecCode = request.SecCode,
                CustKey = request.CustKey,
                BhToken = request.BhToken,
                ChildAgent = request.ChildAgent
            };
            //校验返回值
            var baseResponse = _appVerifyService.Verify(baseRequest, pairs);
            if (baseResponse.ErrCode != 1)
            {
                viewModel.BusinessStatus = baseResponse.ErrCode;
                viewModel.StatusMessage = baseResponse.ErrMsg;
                return viewModel;
            }
            #endregion
            #region 业务逻辑
            //拼接请求串
            string strUrl = string.Format("http://{0}:{1}/api/CarInsurance/GetPrecisePrice", uri.Host, uri.Port);
            //3,续保请求
            BaseResponse rep = await SimulateGet(strUrl, pairs);
            if (rep.ErrCode == 1)
            {
                viewModel = rep.ErrMsg.FromJson<GetPrecisePriceNewViewModel>();
            }
            else
            {
                viewModel.BusinessStatus = rep.ErrCode;
                viewModel.StatusMessage = rep.ErrMsg;
            }
            bx_userinfo userinfo = _userInfoRepository.FindByOpenIdAndLicense(request.CustKey, request.LicenseNo, request.ChildAgent.ToString(),request.RenewalCarType);
            viewModel.Buid = userinfo != null ? userinfo.Id : 0;
            viewModel.Agent = baseResponse.Agent;
            viewModel.AgentName = baseResponse.AgentName;
            #endregion
            return viewModel;
        }

        public async Task<SubmitInfoNewViewModel> GetSubmitInfo(GetSubmitInfoRequest request, IEnumerable<KeyValuePair<string, string>> pairs, Uri uri)
        {
            var viewModel = new SubmitInfoNewViewModel();
            #region 参数校验
            //校验请求串
            var baseRequest = new AppBaseRequest()
            {
                Agent = request.Agent,
                SecCode = request.SecCode,
                CustKey = request.CustKey,
                BhToken = request.BhToken,
                ChildAgent = request.ChildAgent
            };
            //校验返回值
            var baseResponse = _appVerifyService.Verify(baseRequest, pairs);
            if (baseResponse.ErrCode != 1)
            {
                viewModel.BusinessStatus = baseResponse.ErrCode;
                viewModel.StatusMessage = baseResponse.ErrMsg;
                return viewModel;
            }
            #endregion
            #region 业务逻辑
            //拼接请求串
            string strUrl = string.Format("http://{0}:{1}/api/CarInsurance/GetSubmitInfo", uri.Host, uri.Port);
            //3,续保请求
            BaseResponse rep = await SimulateGet(strUrl, pairs);
            if (rep.ErrCode == 1)
            {
                viewModel = rep.ErrMsg.FromJson<SubmitInfoNewViewModel>();
            }
            else
            {
                viewModel.BusinessStatus = rep.ErrCode;
                viewModel.StatusMessage = rep.ErrMsg;
            }
            bx_userinfo userinfo = _userInfoRepository.FindByOpenIdAndLicense(request.CustKey, request.LicenseNo, request.ChildAgent.ToString(),request.RenewalCarType);
            viewModel.Buid = userinfo != null ? userinfo.Id : 0;
            viewModel.Agent = baseResponse.Agent;
            viewModel.AgentName = baseResponse.AgentName;
            #endregion
            return viewModel;
        }

        public async Task<GetCreaditInfoViewModel> GetCreditInfo(GetEscapedInfoRequest request, IEnumerable<KeyValuePair<string, string>> pairs, Uri uri)
        {
            var viewModel = new GetCreaditInfoViewModel();
            #region 参数校验
            //校验请求串
            var baseRequest = new AppBaseRequest()
            {
                Agent = request.Agent,
                SecCode = request.SecCode,
                CustKey = request.CustKey,
                BhToken = request.BhToken,
                ChildAgent = request.ChildAgent
            };
            //校验返回值
            var baseResponse = _appVerifyService.Verify(baseRequest, pairs);
            if (baseResponse.ErrCode != 1)
            {
                viewModel.BusinessStatus = baseResponse.ErrCode;
                viewModel.StatusMessage = baseResponse.ErrMsg;
                return viewModel;
            }
            #endregion
            #region 业务逻辑
            //拼接请求串
            string strUrl = string.Format("http://{0}:{1}/api/Claim/GetCreditInfo", uri.Host, uri.Port);
            //3,续保请求
            BaseResponse rep = await SimulateGet(strUrl, pairs);
            if (rep.ErrCode == 1)
            {
                viewModel = rep.ErrMsg.FromJson<GetCreaditInfoViewModel>();
            }
            else
            {
                viewModel.BusinessStatus = rep.ErrCode;
                viewModel.StatusMessage = rep.ErrMsg;
            }
            #endregion
            return viewModel;
        }

        public async Task<CarVehicleInfoNewViewModel> GetVehicleInfo(GetCarVehicleRequest request, IEnumerable<KeyValuePair<string, string>> pairs, Uri uri)
        {
            var viewModel = new CarVehicleInfoNewViewModel();
            #region 参数校验
            //校验请求串
            var baseRequest = new AppBaseRequest()
            {
                Agent = request.Agent,
                SecCode = request.SecCode,
                CustKey = request.CustKey,
                BhToken = request.BhToken,
                ChildAgent = request.ChildAgent
            };
            //校验返回值
            var baseResponse = _appVerifyService.Verify(baseRequest, pairs);
            if (baseResponse.ErrCode != 1)
            {
                viewModel.BusinessStatus = baseResponse.ErrCode;
                viewModel.StatusMessage = baseResponse.ErrMsg;
                return viewModel;
            }
            #endregion
            #region 业务逻辑
            //拼接请求串
            string strUrl = string.Format("http://{0}:{1}/api/CarInsurance/GetVehicleInfo", uri.Host, uri.Port);
            //3,续保请求
            BaseResponse rep = await SimulateGet(strUrl, pairs);
            if (rep.ErrCode == 1)
            {
                viewModel = rep.ErrMsg.FromJson<CarVehicleInfoNewViewModel>();
            }
            else
            {
                viewModel.BusinessStatus = rep.ErrCode;
                viewModel.StatusMessage = rep.ErrMsg;
            }
            #endregion
            return viewModel;
        }

        public async Task<CheckCarVehicleInfoViewModel> CheckVehicle(GetNewCarSecondVehicleRequest request, IEnumerable<KeyValuePair<string, string>> pairs, Uri uri)
        {
            var viewModel = new CheckCarVehicleInfoViewModel();
            #region 参数校验
            //校验请求串
            var baseRequest = new AppBaseRequest()
            {
                Agent = request.Agent,
                SecCode = request.SecCode,
                CustKey = request.CustKey,
                BhToken = request.BhToken,
                ChildAgent = request.ChildAgent
            };
            //校验返回值
            var baseResponse = _appVerifyService.Verify(baseRequest, pairs);
            if (baseResponse.ErrCode != 1)
            {
                viewModel.BusinessStatus = baseResponse.ErrCode;
                viewModel.StatusMessage = baseResponse.ErrMsg;
                return viewModel;
            }
            #endregion
            #region 业务逻辑
            //拼接请求串
            string strUrl = string.Format("http://{0}:{1}/api/CarInsurance/CheckVehicle", uri.Host, uri.Port);
            //3,续保请求
            BaseResponse rep = await SimulateGet(strUrl, pairs);
            if (rep.ErrCode == 1)
            {
                viewModel = rep.ErrMsg.FromJson<CheckCarVehicleInfoViewModel>();
            }
            else
            {
                viewModel.BusinessStatus = rep.ErrCode;
                viewModel.StatusMessage = rep.ErrMsg;
            }
            #endregion
            return viewModel;
        }
        #endregion

        #region 报价续保列表
        public MyListViewModel GetMyList(GetMyListRequest request,
            IEnumerable<KeyValuePair<string, string>> pairs)
        {
            var viewModel = new MyListViewModel();
            #region 参数校验
            //校验请求串
            var baseRequest = new AppBaseRequest()
            {
                Agent = request.Agent,
                SecCode = request.SecCode,
                CustKey = request.CustKey,
                BhToken = request.BhToken,
                ChildAgent = request.ChildAgent
            };
            //校验返回值
            var baseResponse = _appVerifyService.Verify(baseRequest, pairs);
            if (baseResponse.ErrCode != 1)
            {
                viewModel.BusinessStatus = baseResponse.ErrCode;
                viewModel.StatusMessage = baseResponse.ErrMsg;
                return viewModel;
            }
            #endregion

            #region 业务逻辑
            viewModel = _bjdService.GetMyList(request);
            #endregion
            return viewModel;
        }

        public MyBaoJiaViewModel GetPrecisePriceDetail(GetMyBjdDetailRequest request, IEnumerable<KeyValuePair<string, string>> pairs, Uri uri)
        {
            var viewModel = new MyBaoJiaViewModel();
            #region 参数校验
            //校验请求串
            var baseRequest = new AppBaseRequest()
            {
                Agent = request.Agent,
                SecCode = request.SecCode,
                CustKey = request.CustKey,
                BhToken = request.BhToken,
                ChildAgent = request.ChildAgent
            };
            //校验返回值
            var baseResponse = _appVerifyService.Verify(baseRequest, pairs);
            if (baseResponse.ErrCode != 1)
            {
                viewModel.BusinessStatus = baseResponse.ErrCode;
                viewModel.StatusMessage = baseResponse.ErrMsg;
                return viewModel;
            }
            #endregion
            #region 业务逻辑
            //拼接请求串
            string strUrl = string.Format("http://{0}:{1}/api/Bjd/GetMyBjdDetail", uri.Host, uri.Port);
            //3,续保请求
            BaseResponse rep = SimulateSyncGet(strUrl, pairs);
            if (rep.ErrCode == 1)
            {
                viewModel = rep.ErrMsg.FromJson<MyBaoJiaViewModel>();
            }
            else
            {
                viewModel.BusinessStatus = rep.ErrCode;
                viewModel.StatusMessage = rep.ErrMsg;
            }
            //bx_userinfo userinfo = _userInfoRepository.FindByOpenIdAndLicense(request.CustKey, request.LicenseNo, request.ChildAgent.ToString());
            //viewModel.Buid = userinfo != null ? userinfo.Id : 0;
            //viewModel.Agent = userinfo.Agent;
            //viewModel.AgentName = baseResponse.AgentName;
            #endregion
            return viewModel;
        }

        public GetReInfoNewViewModel GetReInfoDetail(GetReInfoRequest request, IEnumerable<KeyValuePair<string, string>> pairs, Uri uri)
        {
            var viewModel = new GetReInfoNewViewModel();
            #region 参数校验
            //校验请求串
            var baseRequest = new AppBaseRequest()
            {
                Agent = request.Agent,
                SecCode = request.SecCode,
                CustKey = request.CustKey,
                BhToken = request.BhToken,
                ChildAgent = request.ChildAgent
            };
            //校验返回值
            var baseResponse = _appVerifyService.Verify(baseRequest, pairs);
            if (baseResponse.ErrCode != 1)
            {
                viewModel.BusinessStatus = baseResponse.ErrCode;
                viewModel.StatusMessage = baseResponse.ErrMsg;
                return viewModel;
            }
            #endregion
            #region 业务逻辑
            //拼接请求串
            string strUrl = string.Format("http://{0}:{1}/api/Bjd/GetReInfoDetail", uri.Host, uri.Port);
            //3,续保请求
            BaseResponse rep = SimulateSyncGet(strUrl, pairs);
            var model = new AppReInfoViewModel();
            if (rep.ErrCode == 1)
            {
                model = rep.ErrMsg.FromJson<AppReInfoViewModel>();
                viewModel.BusinessStatus = model.BusinessStatus;
                viewModel.StatusMessage = model.StatusMessage;
                viewModel.UserInfo = model.UserInfo;
                viewModel.SaveQuote = model.SaveQuote;
                viewModel.Buid = model.Buid;
                viewModel.Agent = baseResponse.Agent;
                viewModel.AgentName = baseResponse.AgentName;
            }
            else
            {
                viewModel.BusinessStatus = rep.ErrCode;
                viewModel.StatusMessage = rep.ErrMsg;
                bx_userinfo userinfo = _userInfoRepository.FindByOpenIdAndLicense(request.CustKey, request.LicenseNo, request.ChildAgent.ToString(),request.RenewalCarType);
                viewModel.Buid = userinfo != null ? userinfo.Id : 0;
                viewModel.Agent = baseResponse.Agent;
                viewModel.AgentName = baseResponse.AgentName;
            }
            #endregion
            return viewModel;
        }
        #endregion

        #region 分享报价单
        public BaseViewModel SharePrecisePrice(CreateOrUpdateBjdInfoRequest request, IEnumerable<KeyValuePair<string, string>> pairs, Uri uri)
        {
            var viewModel = new BaseViewModel();
            #region 参数校验
            //校验请求串
            var baseRequest = new AppBaseRequest()
            {
                Agent = request.Agent,
                SecCode = request.SecCode,
                CustKey = request.CustKey,
                BhToken = request.BhToken,
                ChildAgent = request.ChildAgent
            };
            //校验返回值
            var baseResponse = _appVerifyService.Verify(baseRequest, pairs);
            if (baseResponse.ErrCode != 1)
            {
                viewModel.BusinessStatus = baseResponse.ErrCode;
                viewModel.StatusMessage = baseResponse.ErrMsg;
                return viewModel;
            }
            #endregion
            #region 业务逻辑
            //拼接请求串
            string strUrl = string.Format("http://{0}:{1}/api/Bjd/UpdateBjdInfo", uri.Host, uri.Port);
            //3,续保请求
            BaseResponse rep = SimulateSyncGet(strUrl, pairs);
            if (rep.ErrCode == 1)
            {
                viewModel = rep.ErrMsg.FromJson<BaseViewModel>();
            }
            else
            {
                viewModel.BusinessStatus = rep.ErrCode;
                viewModel.StatusMessage = rep.ErrMsg;
            }
            //bx_userinfo userinfo = _userInfoRepository.FindByOpenIdAndLicense(request.CustKey, request.LicenseNo, request.ChildAgent.ToString());
            //viewModel.Buid = userinfo != null ? userinfo.Id : 0;
            //viewModel.Agent = baseResponse.Agent;
            //viewModel.AgentName = baseResponse.AgentName;
            #endregion
            return viewModel;
        }
        public BaojiaItemViewModel GetShare(GetBjdItemRequest request, IEnumerable<KeyValuePair<string, string>> pairs, Uri uri)
        {
            var viewModel = new BaojiaItemViewModel();
            #region 参数校验
            //校验请求串
            var baseRequest = new AppBaseRequest()
            {
                Agent = request.Agent,
                SecCode = request.SecCode,
                CustKey = request.CustKey,
                BhToken = request.BhToken,
                ChildAgent = request.ChildAgent
            };
            //校验返回值
            var baseResponse = _appVerifyService.Verify(baseRequest, pairs);
            if (baseResponse.ErrCode != 1)
            {
                viewModel.BusinessStatus = baseResponse.ErrCode;
                viewModel.StatusMessage = baseResponse.ErrMsg;
                return viewModel;
            }
            #endregion
            #region 业务逻辑
            //拼接请求串
            string strUrl = string.Format("http://{0}:{1}/api/Bjd/Get", uri.Host, uri.Port);
            //3,续保请求
            BaseResponse rep = SimulateSyncGet(strUrl, pairs);
            if (rep.ErrCode == 1)
            {
                viewModel = rep.ErrMsg.FromJson<BaojiaItemViewModel>();
            }
            else
            {
                viewModel.BusinessStatus = rep.ErrCode;
                viewModel.StatusMessage = rep.ErrMsg;
            }
            //bx_userinfo userinfo = _userInfoRepository.FindByOpenIdAndLicense(request.CustKey, request.LicenseNo, request.ChildAgent.ToString());
            //viewModel.Buid = userinfo != null ? userinfo.Id : 0;
            //viewModel.Agent = baseResponse.Agent;
            //viewModel.AgentName = baseResponse.AgentName;
            #endregion
            return viewModel;
        }
        #endregion

        #region 回访记录
        public BaseViewModel AddReVisited(AddReVisitedRequest request,
            IEnumerable<KeyValuePair<string, string>> pairs)
        {
            var viewModel = new BaseViewModel();
            #region 参数校验
            //校验请求串
            var baseRequest = new AppBaseRequest()
            {
                Agent = request.Agent,
                SecCode = request.SecCode,
                CustKey = request.CustKey,
                BhToken = request.BhToken,
                ChildAgent = request.ChildAgent
            };
            //校验返回值
            var baseResponse = _appVerifyService.Verify(baseRequest, pairs);
            if (baseResponse.ErrCode != 1)
            {
                viewModel.BusinessStatus = baseResponse.ErrCode;
                viewModel.StatusMessage = baseResponse.ErrMsg;
                return viewModel;
            }
            #endregion

            #region 业务逻辑
            if (request.Buid.HasValue)
            {
                viewModel = _workOrderService.AddReVisited(request);
            }
            else
            {
                viewModel.BusinessStatus = -10000;
                viewModel.StatusMessage = "输入参数错误，Buid不能为空";
            }
            #endregion
            return viewModel;
        }

        public ReVisitedListViewModel ReVisitedList(ReVisitedListRequest request,
            IEnumerable<KeyValuePair<string, string>> pairs)
        {
            var viewModel = new ReVisitedListViewModel();
            #region 参数校验
            //校验请求串
            var baseRequest = new AppBaseRequest()
            {
                Agent = request.Agent,
                SecCode = request.SecCode,
                CustKey = request.CustKey,
                BhToken = request.BhToken,
                ChildAgent = request.ChildAgent
            };
            //校验返回值
            var baseResponse = _appVerifyService.Verify(baseRequest, pairs);
            if (baseResponse.ErrCode != 1)
            {
                viewModel.BusinessStatus = baseResponse.ErrCode;
                viewModel.StatusMessage = baseResponse.ErrMsg;
                return viewModel;
            }
            #endregion

            #region 业务逻辑
            if (request.Buid.HasValue)
            {
                viewModel = _workOrderService.WorkOrderList(request.Buid.Value);
            }
            else
            {
                viewModel.BusinessStatus = -10000;
                viewModel.StatusMessage = "输入参数错误，Buid不能为空";
            }

            #endregion
            return viewModel;
        }
        #endregion

        #region 系统基础信息
        public AppAgentSourceViewModel GetAgentSource(AppBaseRequest request,
            IEnumerable<KeyValuePair<string, string>> pairs, Uri uri)
        {
            var viewModel = new AppAgentSourceViewModel();
            #region 参数校验
            //校验返回值
            var baseResponse = _appVerifyService.Verify(request, pairs);
            if (baseResponse.ErrCode != 1)
            {
                viewModel.BusinessStatus = baseResponse.ErrCode;
                viewModel.StatusMessage = baseResponse.ErrMsg;
                return viewModel;
            }
            #endregion

            #region 业务逻辑
            List<AgentCity> agentCity = _agentService.GetSourceList(string.Format("http://{0}:{1}/", uri.Host, uri.Port), request.Agent);
            if (agentCity.Any())
            {
                viewModel.BusinessStatus = 1;
                viewModel.AgentCity = agentCity;
            }
            else
            {
                viewModel.BusinessStatus = -10002;
                viewModel.StatusMessage = "获取信息失败";
            }

            #endregion
            return viewModel;
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// Get请求接口数据
        /// </summary>
        /// <param name="strUrl"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        private async Task<BaseResponse> SimulateGet(string strUrl, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            //拼接请求串
            var sb = new StringBuilder();
            sb.Append("?");
            foreach (KeyValuePair<string, string> pair in pairs)
            {
                if (pair.Key.ToUpper().Equals("BHTOKEN"))
                    sb.Append(string.Format("{0}={1}&", pair.Key, HttpUtility.UrlEncode(pair.Value)));
                else
                    sb.Append(string.Format("{0}={1}&", pair.Key, pair.Value));
            }
            sb.Remove(sb.Length - 1, 1);
            string getUrl = string.Format(strUrl + sb);
            //模拟请求
            var response = new BaseResponse();
            try
            {
                using (var client = new HttpClient())
                {
                    var clientResult = client.GetAsync(getUrl).Result;
                    if (clientResult.StatusCode.ToString().Equals("429"))
                    {
                        response.ErrCode = -429;
                        response.ErrMsg = "请求客户端频繁，请稍候再试";
                        return response;
                    }
                    if (clientResult.IsSuccessStatusCode)
                    {
                        response.ErrCode = 1;
                        response.ErrMsg = await clientResult.Content.ReadAsStringAsync();
                    }
                    if (!string.IsNullOrWhiteSpace(response.ErrMsg))
                        return response;
                    response.ErrCode = -10002;
                    response.ErrMsg = "获取信息失败";
                }
                return response;
            }
            catch (Exception ex)
            {
                response.ErrCode = -10003;
                response.ErrMsg = "服务器发生异常";
                logError.Info("Get请求接口数据异常，请求串为：" + strUrl + "\n 异常信息:" + ex.StackTrace + " \n " + ex.Message);
                return response;
            }
        }

        private BaseResponse SimulateSyncGet(string strUrl, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            //拼接请求串
            var sb = new StringBuilder();
            sb.Append("?");
            foreach (KeyValuePair<string, string> pair in pairs)
            {
                if (pair.Key.ToUpper().Equals("BHTOKEN"))
                    sb.Append(string.Format("{0}={1}&", pair.Key, HttpUtility.UrlEncode(pair.Value)));
                else
                    sb.Append(string.Format("{0}={1}&", pair.Key, pair.Value));
            }
            sb.Remove(sb.Length - 1, 1);
            string getUrl = string.Format(strUrl + sb);
            //模拟请求
            var response = new BaseResponse();

            var myReq = (HttpWebRequest)WebRequest.Create(getUrl);
            try
            {
                WebResponse wr = myReq.GetResponse();
                Stream receiveStream = wr.GetResponseStream();
                if (receiveStream != null)
                {
                    var reader = new StreamReader(receiveStream, Encoding.UTF8);
                    string content = reader.ReadToEnd();
                    response.ErrCode = 1;
                    response.ErrMsg = content;
                }
                else
                {
                    response.ErrCode = -10002;
                    response.ErrMsg = "获取信息失败";
                }
                return response;
            }
            catch (Exception ex)
            {
                response.ErrCode = -10003;
                response.ErrMsg = "服务器发生异常";
                logError.Info("Get请求接口数据异常，请求串为：" + strUrl + "\n 异常信息:" + ex.StackTrace + " \n " + ex.Message);
                return response;
            }
        }

        /// <summary>
        /// Post请求接口数据
        /// </summary>
        /// <param name="strUrl"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        private async Task<BaseResponse> SimulatePost(string strUrl, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            //模拟请求
            var response = new BaseResponse();
            try
            {
                using (var client = new HttpClient())
                {
                }
                return response;
            }
            catch (Exception ex)
            {
                response.ErrCode = -1000;
                response.ErrMsg = "获取信息异常";
                logError.Info("Post请求接口数据异常，请求串为：" + strUrl + "\n 参数为：" + pairs.ToJson() + "\n 异常信息:" + ex.StackTrace + " \n " + ex.Message);
                return response;
            }
        }

        //private class RequestObj<T>:AppBaseRequest
        //{
        //    private T ObjectT;
        //    private RequestObj(T obj)
        //    {
        //        this.ObjectT = obj;
        //    }
        //}

        #endregion
    }
}
