using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BiHuManBu.ExternalInterfaces.Infrastructure;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Mapper;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using log4net;
using ServiceStack.Text;

namespace BiHuManBu.ExternalInterfaces.API.Controllers
{
    public class ClaimController : ApiController
    {
        private ILog logInfo;
        private static IUserClaimService _userClaimService;

        public ClaimController(IUserClaimService userClaimService)
        {
            logInfo = LogManager.GetLogger("INFO");
            _userClaimService = userClaimService;
        }

        /// <summary>
        /// 车辆出险信息新
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet, ActionName("GetCreditDetailInfo")]
        public async Task<HttpResponseMessage> FetchCreditDetailInfo([FromUri]GetEscapedInfoRequest request)
        {
            logInfo.Info(string.Format("车辆出险信息接口请求串：{0}", Request.RequestUri));

            var vm = new GetCreaditDetailInfoViewModel();

            if (!ModelState.IsValid)
            {
                vm.BusinessStatus = -10000;
                vm.StatusMessage = "输入参数错误，请检查您输入的参数是否有空或者长度不符合要求等";
                return vm.ResponseToJson();
            }

            GetEscapedInfoResponse response = await _userClaimService.GetList(request, Request.GetQueryNameValuePairs());
            if (response.Status == HttpStatusCode.Forbidden)
            {
                vm.BusinessStatus = -10001;
                vm.StatusMessage = "参数校验错误，请检查您的校验码";
                return vm.ResponseToJson();
            }
            if (response.Status == HttpStatusCode.ExpectationFailed)
            {
                vm.BusinessStatus = -10003;
                vm.StatusMessage = "服务器发生异常";
                return vm.ResponseToJson();
            }
            if (response.List != null)
            {
                vm.BusinessStatus = 1;
                vm.List = response.List.ConvertToDetailViewModelList();
            }
            else
            {
                vm.BusinessStatus = -10002;
                vm.StatusMessage = "获取车辆出险信息失败";
            }
            if (response.Lastinfo != null)
            {
                vm.BizClaimCount = (response.Lastinfo.LastYearBizClaimTimes ?? 0).ToString();
                vm.ForceCliamCount = (response.Lastinfo.LastYearForceClaimTimes ?? 0).ToString();
                vm.ClaimCount = (response.Lastinfo.last_year_claimtimes ?? 0).ToString();
            }
            else {
                vm.BizClaimCount = "";
                vm.ForceCliamCount = "";
                vm.ClaimCount = "";
            }
            if (request.ShowClaimCount == 0)
            {
                vm.BizClaimCount = "";
                vm.ForceCliamCount = "";
                vm.ClaimCount = "";
            }

            logInfo.Info("车辆：" + request.LicenseNo + "的出险信息:" + vm.ToJson());
            return vm.ResponseToJson();
        }
        /// <summary>
        /// 车辆出险信息(废弃)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet, ActionName("GetCreditInfo")]
        public async Task<HttpResponseMessage> FetchCreditInfo([FromUri]GetEscapedInfoRequest request)
        {
            logInfo.Info(string.Format("车辆出险信息接口请求串：{0}", Request.RequestUri));

            var vm = new GetCreaditInfoViewModel();

            if (!ModelState.IsValid)
            {
                vm.BusinessStatus = -10000;
                vm.StatusMessage = "输入参数错误，请检查您输入的参数是否有空或者长度不符合要求等";
                return vm.ResponseToJson();
            }

            GetEscapedInfoResponse response = await _userClaimService.GetList(request, Request.GetQueryNameValuePairs());
            if (response.Status == HttpStatusCode.Forbidden)
            {
                vm.BusinessStatus = -10001;
                vm.StatusMessage = "参数校验错误，请检查您的校验码";
            }
            if (response.Status == HttpStatusCode.ExpectationFailed)
            {
                vm.BusinessStatus = -10003;
                vm.StatusMessage = "服务器发生异常";
                return vm.ResponseToJson();
            }

            if (response.List != null)
            {
                vm.BusinessStatus = 1;
                vm.List = response.List.ConvertToViewModelList();
            }
            else
            {
                vm.BusinessStatus = -10002;
                vm.StatusMessage = "获取车辆出险信息失败";
            }

            logInfo.Info("车辆：" + request.LicenseNo + "的出险信息:" + vm.ToJson());
            return vm.ResponseToJson();
        }

        public async Task<HttpResponseMessage> GetViolationInfo([FromUri]GetViolationInfoRequest request)
        {
            logInfo.Info(string.Format("车辆出险信息接口请求串：{0}", Request.RequestUri));

            var vm = new GetViolationInfoViewModel();

            if (!ModelState.IsValid)
            {
                vm.BusinessStatus = -10000;
                vm.StatusMessage = "输入参数错误，请检查您输入的参数是否有空或者长度不符合要求等";
                return vm.ResponseToJson();
            }

            var response = await _userClaimService.GetViolationList(request, Request.GetQueryNameValuePairs());
            if (response.Status == HttpStatusCode.Forbidden)
            {
                vm.BusinessStatus = -10001;
                vm.StatusMessage = "参数校验错误，请检查您的校验码";
            }
            if (response.Status == HttpStatusCode.ExpectationFailed)
            {
                vm.BusinessStatus = -10003;
                vm.StatusMessage = "服务器发生异常";
                return vm.ResponseToJson();
            }

            if (response.List != null)
            {
                vm.BusinessStatus = 1;
                vm.List = response.List.ConverToViewModelList();
            }
            else
            {
                vm.BusinessStatus = -10002;
                vm.StatusMessage = "获取车辆出险信息失败";
            }

            logInfo.Info("车辆：" + request.LicenseNo + "的违章信息:" + vm.ToJson());
            return vm.ResponseToJson();
        }

    }
}