using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BiHuManBu.ExternalInterfaces.Infrastructure;
using BiHuManBu.ExternalInterfaces.Services.ChannelService;
using BiHuManBu.ExternalInterfaces.Services.Mapper;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.API.Controllers
{
    public class ChannelController : ApiController
    {
        private readonly ICityChannelService _channelService;

        public ChannelController(ICityChannelService channelService)
        {
            _channelService = channelService;
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetCityChannels([FromUri]GetSourceOfCityRequest request)
        {
            CityChannelViewModel viewModel = new CityChannelViewModel()
            {
                Items = new List<CityChannelItem>(),
                BusinessStatus = 1,
                StatusMessage = "获取成功"
            };
            

          var response=  _channelService.GetSourceOfCity(request, Request.GetQueryNameValuePairs());

          if (response.Status == HttpStatusCode.Unauthorized)
          {
              viewModel.BusinessStatus = -10002;
              viewModel.StatusMessage = "账号不可用";
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


          viewModel.Items = response.Result;

          return viewModel.ResponseToJson();
        }
    }
}