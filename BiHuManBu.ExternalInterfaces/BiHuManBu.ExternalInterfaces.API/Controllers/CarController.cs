using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using BiHuManBu.ExternalInterfaces.Infrastructure;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Mapper;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.API.Controllers
{
    public class CarController : ApiController
    {
        private readonly IDriverLicenseTypeService _driverLicenseTypeService;
     

        public CarController(IDriverLicenseTypeService driverLicenseTypeService)
        {
            _driverLicenseTypeService = driverLicenseTypeService;
         
        }

        [System.Web.Mvc.HttpGet]
        public HttpResponseMessage GetList([FromUri]GetDriverLicenseCarTypeRequest request)
        {
            DriverLicenseTypeViewModel viewModel = new DriverLicenseTypeViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }

            DriverLicenseTypeResponse response = _driverLicenseTypeService.GetList(request, Request.GetQueryNameValuePairs());
            viewModel.Items = new List<DriverLicenseType>();
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

            viewModel.Items = response.List.ConvertToList();
            viewModel.BusinessStatus = 1;
            viewModel.StatusMessage = "获取成功";

            return viewModel.ResponseToJson();
        }

    }
}
