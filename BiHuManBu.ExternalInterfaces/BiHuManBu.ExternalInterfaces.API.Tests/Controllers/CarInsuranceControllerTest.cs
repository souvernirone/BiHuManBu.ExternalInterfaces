using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Hosting;
using BiHuManBu.ExternalInterfaces.API.Controllers;
using BiHuManBu.ExternalInterfaces.API.ViewModels;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using Moq;
using NUnit.Framework;

namespace BiHuManBu.ExternalInterfaces.API.Tests.Controllers
{
    //[TestFixture]
//    public class CarInsuranceControllerTest
//    {
//        private Mock<ICarInsuranceService> _carInsuranceService;

//        [SetUp]
//        public void SetUp()
//        {
//            _carInsuranceService = new Mock<ICarInsuranceService>();
//        }

//        #region FetchReInsuranceInfo 
//        [Test]
//        public async void FetchReInsuranceInfo_InValidRequest_Returns_10000()
//        {
//            GetReInfoRequest request = new GetReInfoRequest
//            {
//                AgentId = 103,
//                CityCode = 1,
//                SecCode = "XSFSDSFD"
//            };
//            CarInsuranceController controller = new CarInsuranceController(_carInsuranceService.Object)
//            {
//                Request = new HttpRequestMessage
//                {
//                    Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }
//                }
//            };
//            controller.ModelState.AddModelError("车牌号","不能为空");

//            _carInsuranceService.Setup(x => x.GetReInfo(request, controller.Request.GetQueryNameValuePairs())).Returns(GetReInfo());
//            HttpResponseMessage message = await controller.FetchReInsuranceInfo(request);
//            NUnit.Framework.Assert.AreEqual(-10000,message.Content.ReadAsAsync<GetReInfoViewModel>().Result.BusinessStatus);
           
//        }

//        [Test]
//        public async void FetchReInsuranceInfo_InValid_Seccode_Returns_10001()
//        {
//            GetReInfoRequest request = new GetReInfoRequest
//            {
//                AgentId = 103,
//                CityCode = 1,
//                SecCode = "XSFSDSFD"
//            };
//            CarInsuranceController controller = new CarInsuranceController(_carInsuranceService.Object)
//            {
//                Request = new HttpRequestMessage
//                {
//                    Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }
//                }
//            };

//            _carInsuranceService.Setup(x => x.GetReInfo(request, controller.Request.GetQueryNameValuePairs())).Returns(GetReInfo_InValid_SecCode());
//            HttpResponseMessage message = await controller.FetchReInsuranceInfo(request);
//            NUnit.Framework.Assert.AreEqual(-10001, message.Content.ReadAsAsync<GetReInfoViewModel>().Result.BusinessStatus);

//        }

//        [Test]
//        public async void FetchReInsuranceInfo_Get_NoSaveQuote_Returns_10002()
//        {
//            GetReInfoRequest request = new GetReInfoRequest
//            {
//                AgentId = 103,
//                CityCode = 1,
//                SecCode = "XSFSDSFD"
//            };
//            CarInsuranceController controller = new CarInsuranceController(_carInsuranceService.Object)
//            {
//                Request = new HttpRequestMessage
//                {
//                    Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }
//                }
//            };

//            _carInsuranceService.Setup(x => x.GetReInfo(request, controller.Request.GetQueryNameValuePairs())).Returns(GetReInfo_SaveQuote_NOFound);
//            HttpResponseMessage message = await controller.FetchReInsuranceInfo(request);
//            NUnit.Framework.Assert.AreEqual(-10002, message.Content.ReadAsAsync<GetReInfoViewModel>().Result.BusinessStatus);
          
//        }
//        [Test]
//        public async void FetchReInsuranceInfo_Get_NoUserInfo_Returns_10002()
//        {
//            GetReInfoRequest request = new GetReInfoRequest
//            {
//                AgentId = 103,
//                CityCode = 1,
//                SecCode = "XSFSDSFD"
//            };
//            CarInsuranceController controller = new CarInsuranceController(_carInsuranceService.Object)
//            {
//                Request = new HttpRequestMessage
//                {
//                    Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }
//                }
//            };

//            _carInsuranceService.Setup(x => x.GetReInfo(request, controller.Request.GetQueryNameValuePairs())).Returns(GetReInfo_UserInfo_NOFound);
//            HttpResponseMessage message = await controller.FetchReInsuranceInfo(request);
//            NUnit.Framework.Assert.AreEqual(-10002, message.Content.ReadAsAsync<GetReInfoViewModel>().Result.BusinessStatus);
          
//        }
//        #endregion
       

//        private async Task<GetReInfoResponse>    GetReInfo()
//        {
//            GetReInfoResponse response = new GetReInfoResponse()
//            {
//                LastInfo = new bx_lastinfo(),
//                SaveQuote = new bx_savequote(),
//                Status = HttpStatusCode.OK,
//                UserInfo = new bx_userinfo()

//            };
           
//            return response;
//        }
//        private async Task<GetReInfoResponse> GetReInfo_SaveQuote_NOFound()
//        {
//            GetReInfoResponse response = new GetReInfoResponse()
//            {
//                LastInfo = new bx_lastinfo(),
//                Status = HttpStatusCode.OK,
//                UserInfo = new bx_userinfo()

//            };

//            return response;
//        }
//        private async Task<GetReInfoResponse> GetReInfo_UserInfo_NOFound()
//        {
//            GetReInfoResponse response = new GetReInfoResponse()
//            {
               

//            };

//            return response;
//        }

//        private async Task<GetReInfoResponse> GetReInfo_InValid_SecCode()
//        {
//            GetReInfoResponse response = new GetReInfoResponse()
//            {
//                LastInfo = new bx_lastinfo(),
//                SaveQuote = new bx_savequote(),
//                Status = HttpStatusCode.BadRequest,
//                UserInfo = new bx_userinfo()

//            };

//            return response;
//        }
//    }
}
