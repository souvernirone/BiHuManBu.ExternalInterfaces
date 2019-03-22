using BiHuManBu.ExternalInterfaces.Infrastructure;
using BiHuManBu.ExternalInterfaces.Services.GetReInfoService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.RemoteMessage;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.PrecisePriceService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.SubmitInfoService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using log4net;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebApiThrottle;


namespace BiHuManBu.ExternalInterfaces.API.Controllers
{
    [DisableThrotting()]
    public class QuoteSpecialController : ApiController
    {
        private readonly ILog logInfo;
        private readonly ILog logError;
        private static readonly string _baoxianCenter = ConfigurationManager.AppSettings["baoxianCenterApi"];
        private static readonly string _gzcbAgentId = ConfigurationManager.AppSettings["gzcbAgentId"];
        private static readonly string _reInfoFailedSource = ConfigurationManager.AppSettings["ReInfoFailedSource"];
        private static readonly string _reInfoNoRelationSource = ConfigurationManager.AppSettings["ReInfoNoRelationSource"];
        private IQuoteSpecialService _quoteSpecialService;
        private readonly IPostSubmitInfoService _postSubmitInfoService;
        private readonly ICheckRequestPostPrecisePrice _checkRequestPostPrecisePrice;
        private readonly IIsFalseReInfoService _isFalseReInfoService;
        private readonly IRenewalStatusService _renewalStatusService;
        private readonly IReWriteUserInfo _reWriteUserInfo;
        private readonly IGetAccidentListService _getAccidentListService;
        public QuoteSpecialController(IQuoteSpecialService quoteSpecialService, IPostSubmitInfoService postSubmitInfoService, ICheckRequestPostPrecisePrice checkRequestPostPrecisePrice, IIsFalseReInfoService isFalseReInfoService, IRenewalStatusService renewalStatusService, IReWriteUserInfo reWriteUserInfo, IGetAccidentListService getAccidentListService)
        {
            logInfo = LogManager.GetLogger("INFO");
            logError = LogManager.GetLogger("ERROR");
            _quoteSpecialService = quoteSpecialService;
            _postSubmitInfoService = postSubmitInfoService;
            _checkRequestPostPrecisePrice = checkRequestPostPrecisePrice;
            _isFalseReInfoService = isFalseReInfoService;
            _renewalStatusService = renewalStatusService;
            _reWriteUserInfo = reWriteUserInfo;
            _getAccidentListService = getAccidentListService;
        }
        [HttpGet]
        public async Task<HttpResponseMessage> GetSpeciaList([FromUri]GetSpecialListRequest request)
        {
            GetSpecialListViewModel model = new GetSpecialListViewModel();
            if (!new int[] { 1, 2, 4, 8 }.Contains(request.Source))
            {
                model.BusinessStatus = 0;
                model.StatusMessage = "该保险公司不支持特约";
                return model.ResponseToJson();
            }
            logInfo.Info(string.Format("获取特约列表：{0}", Request.RequestUri));

            SpecialListResponse response = await _quoteSpecialService.GetSpeciaList(request, Request.GetQueryNameValuePairs());
            if (response.Status == HttpStatusCode.BadRequest || response.Status == HttpStatusCode.Forbidden)
            {
                model.BusinessStatus = response.BusinessStatus;
                model.StatusMessage = response.BusinessMessage;
                model.Key = response.Key;
                return model.ResponseToJson();
            }
            else if (response.Status == HttpStatusCode.OK)
            {
                model.BusinessStatus = response.BusinessStatus;
                model.StatusMessage = response.BusinessMessage;
                model.Count = response.SpecialOptions.Count;
                model.SpecialList = response.SpecialOptions.Skip((request.PageIndex - 1) * (request.PageSize)).Take(request.PageSize).ToList(); ;
                model.Key = response.Key;
                return model.ResponseToJson();
            }
            else
            {
                model.BusinessStatus = response.BusinessStatus;
                model.StatusMessage = response.BusinessMessage;
                model.Key = response.Key;
                return model.ResponseToJson();
            }
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetAccidentList([FromUri]GetAccidentListRequest request)
        {
            GetAccidentListViewModel viewModel = new GetAccidentListViewModel();
            viewModel.CityCode = request.CityCode;

            if (!new long[] { 1, 2, 4 }.Contains(request.Source))
            {
                viewModel.BusinessStatus = 0;
                viewModel.StatusMessage = "该保险公司不支持特约";
                return viewModel.ResponseToJson();
            }
            logInfo.Info(string.Format("获取驾意险种列表：{0}", Request.RequestUri));
            WaBxSysJyxResponse response = await _getAccidentListService.GetAccidentList(request, Request.GetQueryNameValuePairs());

            if (response.ErrCode == 1)
            {
                if (request.Source == 2)
                {
                    viewModel.PingAnJyxInfo = response.PingAnJyxInfo;
                }
                else if (request.Source == 1)
                {
                    viewModel.TpyYwxProductInfo = response.TpyYwxProductInfo;
                }
                viewModel.BusinessStatus = 1;
                viewModel.StatusMessage = "获取信息成功";
            }
            else
            {
                viewModel.BusinessStatus = 0;
                viewModel.StatusMessage = "获取信息失败";
            }


            return viewModel.ResponseToJson();
        }

    }
}
