using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BiHuManBu.ExternalInterfaces.Infrastructure;
using BiHuManBu.ExternalInterfaces.Services;
using BiHuManBu.ExternalInterfaces.Services.Mapper;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using log4net;
using ServiceStack.Text;

namespace BiHuManBu.ExternalInterfaces.API.Controllers
{
    public class CityController : ApiController
    {
        private ICityService _cityService;
        private ILog _logInfo;

        public CityController(ICityService cityService)
        {
            _cityService = cityService;
            _logInfo = LogManager.GetLogger("INFO");
        }
        [HttpGet]
        public async Task<HttpResponseMessage> GetCityList([FromUri]GetCityListRequest request)
        {
            _logInfo.Info(string.Format("获取城市接口请求串：{0}", Request.RequestUri));
            var viewModel = new CityViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                viewModel.StatusMessage = "输入参数错误，请检查您输入的参数是否有空或者长度不符合要求等";
                return viewModel.ResponseToJson();
            }

            var response = await _cityService.GetCityList(request, Request.GetQueryNameValuePairs());
            //添加日志
            _logInfo.Info(response.ToJson());

            if (response.Status == HttpStatusCode.BadRequest || response.Status == HttpStatusCode.Forbidden)
            {
                viewModel.BusinessStatus = -10001;
                viewModel.StatusMessage = "参数校验错误，请检查您的校验码";
                _logInfo.Info(viewModel.ResponseToJson());
                return viewModel.ResponseToJson();
            }
            if (response.Status == HttpStatusCode.ExpectationFailed)
            {
                viewModel.BusinessStatus = -10003;
                viewModel.StatusMessage = "服务发生异常";
            }
            else
            {
                viewModel = response.Cities.ConvertViewModel();
                viewModel.BusinessStatus = 1;
            }
            return viewModel.ResponseToJson();
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetContinuedPeriods([FromUri]GetCityContinuedPeriodRequest request)
        {
            var viewModel = new ContinuedPeriodViewModel();
            viewModel.StatusMessage = "获取成功";
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                viewModel.StatusMessage = "输入参数错误，请检查您输入的参数是否有空或者长度不符合要求等";

                if (request.ShowName == 0)
                {//不显示新增的城市名称对象
                    viewModel.ItemsAddCityName = null;
                    viewModel.Items = new List<ContinedPeriod>();
                }
                else
                {//展示原有列表对象
                    viewModel.ItemsAddCityName = new List<ContinedPeriodNew>();
                    viewModel.Items = null;
                }
                return viewModel.ResponseToJson();
            }

            var response = await _cityService.GetContinuedPeriod(request, Request.GetQueryNameValuePairs());

            if (response.Status == HttpStatusCode.BadRequest || response.Status == HttpStatusCode.Forbidden)
            {
                viewModel.BusinessStatus = -10001;
                viewModel.StatusMessage = "参数校验错误，请检查您的校验码";
                _logInfo.Info(viewModel.ResponseToJson());
                if (request.ShowName == 0)
                {//不显示新增的城市名称对象
                    viewModel.ItemsAddCityName = null;
                    viewModel.Items = new List<ContinedPeriod>();
                }
                else
                {//展示原有列表对象
                    viewModel.ItemsAddCityName = new List<ContinedPeriodNew>();
                    viewModel.Items = null;
                }
                return viewModel.ResponseToJson();
            }
            if (response.Status == HttpStatusCode.ExpectationFailed)
            {
                viewModel.BusinessStatus = -10003;
                viewModel.StatusMessage = "服务发生异常";
            }
            else
            {
                if (request.ShowName == 1)
                {//取新增城市名称对象
                    viewModel = response.Items.ConverToListAddCityName();
                }
                else
                {
                    viewModel = response.Items.ConverToList();
                }
                viewModel.BusinessStatus = 1;
            }

            if (request.ShowName == 0)
            {//不显示新增的城市名称对象
                viewModel.ItemsAddCityName = null;
            }
            else
            {//展示原有列表对象
                viewModel.Items = null;
                viewModel.ItemsAddCityName = viewModel.ItemsAddCityName ?? new List<ContinedPeriodNew>();
            }

            return viewModel.ResponseToJson();
        }
    }
}