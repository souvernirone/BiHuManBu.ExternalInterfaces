using System.Collections.Generic;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services;
using BiHuManBu.ExternalInterfaces.Services.AgentChannelService.Implementations;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using NSubstitute;
using NUnit.Framework;
using BiHuManBu.ExternalInterfaces.Services.ValidateService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using System.Net;
using BiHuManBu.ExternalInterfaces.Services.BjdServices.Achieves;
using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.UnitTests.BjdServicesTests
{
    [TestFixture]
    public class SetCarInfoServiceTests
    {
        private static IQuoteResultCarinfoRepository _quoteResultCarinfoRepository = Substitute.For<IQuoteResultCarinfoRepository>();
        private static SetCarInfoService setCarInfoService = new SetCarInfoService(_quoteResultCarinfoRepository);
        [Test]
        public void SetCarInfo_QuoteResultOne()
        {
            _quoteResultCarinfoRepository.FindList(Arg.Any<long>()).Returns(x => new List<bx_quoteresult_carinfo>() { new bx_quoteresult_carinfo() { source = 2, auto_model_code = "auto_model_code", seat_count=5,
            risk="risk",IsZhuanXubao="IsZhuanXubao",SyVehicleClaimType="SyVehicleClaimType",JqVehicleClaimType="JqVehicleClaimType",VehicleStyle="VehicleStyle"} });
            var quoteresultCarinfo = new List<bx_quoteresult_carinfo>() { };
            var result = setCarInfoService.SetCarInfo(new MyBaoJiaViewModel() { }, new bx_userinfo() { Id = 1 }, out quoteresultCarinfo);
            Assert.AreEqual(2, quoteresultCarinfo.FirstOrDefault().source);
            Assert.AreEqual("auto_model_code", quoteresultCarinfo.FirstOrDefault().auto_model_code);
            Assert.AreEqual(5, quoteresultCarinfo.FirstOrDefault().seat_count);
            Assert.AreEqual("risk", quoteresultCarinfo.FirstOrDefault().risk);
            Assert.AreEqual("IsZhuanXubao", quoteresultCarinfo.FirstOrDefault().IsZhuanXubao);
            Assert.AreEqual("SyVehicleClaimType", quoteresultCarinfo.FirstOrDefault().SyVehicleClaimType);
            Assert.AreEqual("JqVehicleClaimType", quoteresultCarinfo.FirstOrDefault().JqVehicleClaimType);
            Assert.AreEqual("VehicleStyle", quoteresultCarinfo.FirstOrDefault().VehicleStyle);
            Assert.AreEqual(4, result.CarInfos.FirstOrDefault().Source);
            Assert.AreEqual("auto_model_code", result.CarInfos.FirstOrDefault().AutoMoldCode);
            Assert.AreEqual(5, result.CarInfos.FirstOrDefault().CarSeat);
            Assert.AreEqual("", result.CarInfos.FirstOrDefault().VehicleInfo);
            Assert.AreEqual("risk", result.CarInfos.FirstOrDefault().Risk);
            Assert.AreEqual("IsZhuanXubao", result.CarInfos.FirstOrDefault().XinZhuanXu);
            Assert.AreEqual("SyVehicleClaimType", result.CarInfos.FirstOrDefault().SyVehicleClaimType);
            Assert.AreEqual("JqVehicleClaimType", result.CarInfos.FirstOrDefault().JqVehicleClaimType);
            Assert.AreEqual("VehicleStyle", result.CarInfos.FirstOrDefault().VehicleStyle);
            Assert.AreEqual("VehicleAlias", result.CarInfos.FirstOrDefault().VehicleAlias);
            Assert.AreEqual("VehicleYear", result.CarInfos.FirstOrDefault().VehicleYear);
        }
        //[Test]
        //public void SetCarInfo_QuoteResultNull_Result()
        //{
        //    IQuoteResultCarinfoRepository _quoteResultCarinfoRepository = Substitute.For<IQuoteResultCarinfoRepository>();
        //    _quoteResultCarinfoRepository.FindList(Arg.Any<long>()).Returns(x => null);
        //    SetCarInfoService setCarInfoService = new SetCarInfoService(_quoteResultCarinfoRepository);
        //    var quoteresultCarinfo = new List<bx_quoteresult_carinfo>() { };
        //    var result = setCarInfoService.SetCarInfo(new MyBaoJiaViewModel() { }, new bx_userinfo() { Id = 1 }, out quoteresultCarinfo);
        //    Assert.AreEqual(null, quoteresultCarinfo.FirstOrDefault());
        //    Assert.AreEqual(null, result.CarInfos);
        //}
        [Test]
        public void SetCarInfo_QuoteResultMany()
        {
            _quoteResultCarinfoRepository.FindList(Arg.Any<long>()).Returns(x => new List<bx_quoteresult_carinfo>() { new bx_quoteresult_carinfo() { source = 2, auto_model_code = "auto_model_code", seat_count=5,
            risk="risk",IsZhuanXubao="IsZhuanXubao",SyVehicleClaimType="SyVehicleClaimType",JqVehicleClaimType="JqVehicleClaimType",VehicleStyle="VehicleStyle"} ,
            new bx_quoteresult_carinfo() { source = 4, auto_model_code = "auto_model_code1", seat_count=6,
            risk="risk1",IsZhuanXubao="IsZhuanXubao1",SyVehicleClaimType="SyVehicleClaimType1",JqVehicleClaimType="JqVehicleClaimType1",VehicleStyle="VehicleStyle1"} });
            var quoteresultCarinfo = new List<bx_quoteresult_carinfo>() { };
            var result = setCarInfoService.SetCarInfo(new MyBaoJiaViewModel() { }, new bx_userinfo() { Id = 1 }, out quoteresultCarinfo);
            Assert.AreEqual(2, quoteresultCarinfo.FirstOrDefault().source);
            Assert.AreEqual("auto_model_code", quoteresultCarinfo.FirstOrDefault().auto_model_code);
            Assert.AreEqual(5, quoteresultCarinfo.FirstOrDefault().seat_count);
            Assert.AreEqual("risk", quoteresultCarinfo.FirstOrDefault().risk);
            Assert.AreEqual("IsZhuanXubao", quoteresultCarinfo.FirstOrDefault().IsZhuanXubao);
            Assert.AreEqual("SyVehicleClaimType", quoteresultCarinfo.FirstOrDefault().SyVehicleClaimType);
            Assert.AreEqual("JqVehicleClaimType", quoteresultCarinfo.FirstOrDefault().JqVehicleClaimType);
            Assert.AreEqual("VehicleStyle", quoteresultCarinfo.FirstOrDefault().VehicleStyle);
            Assert.AreEqual(4, result.CarInfos.FirstOrDefault().Source);
            Assert.AreEqual("auto_model_code", result.CarInfos.FirstOrDefault().AutoMoldCode);
            Assert.AreEqual(5, result.CarInfos.FirstOrDefault().CarSeat);
            Assert.AreEqual("", result.CarInfos.FirstOrDefault().VehicleInfo);
            Assert.AreEqual("risk", result.CarInfos.FirstOrDefault().Risk);
            Assert.AreEqual("IsZhuanXubao", result.CarInfos.FirstOrDefault().XinZhuanXu);
            Assert.AreEqual("SyVehicleClaimType", result.CarInfos.FirstOrDefault().SyVehicleClaimType);
            Assert.AreEqual("JqVehicleClaimType", result.CarInfos.FirstOrDefault().JqVehicleClaimType);
            Assert.AreEqual("VehicleStyle", result.CarInfos.FirstOrDefault().VehicleStyle);
            Assert.AreEqual("VehicleAlias", result.CarInfos.FirstOrDefault().VehicleAlias);
            Assert.AreEqual("VehicleYear", result.CarInfos.FirstOrDefault().VehicleYear);

            Assert.AreEqual(4, quoteresultCarinfo[1].source);
            Assert.AreEqual("auto_model_code1", quoteresultCarinfo[1].auto_model_code);
            Assert.AreEqual(6, quoteresultCarinfo[1].seat_count);
            Assert.AreEqual("risk1", quoteresultCarinfo[1].risk);
            Assert.AreEqual("IsZhuanXubao1", quoteresultCarinfo[1].IsZhuanXubao);
            Assert.AreEqual("SyVehicleClaimType1", quoteresultCarinfo[1].SyVehicleClaimType);
            Assert.AreEqual("JqVehicleClaimType1", quoteresultCarinfo[1].JqVehicleClaimType);
            Assert.AreEqual("VehicleStyle1", quoteresultCarinfo[1].VehicleStyle);
            Assert.AreEqual(16, result.CarInfos[1].Source);
            Assert.AreEqual("auto_model_code1", result.CarInfos[1].AutoMoldCode);
            Assert.AreEqual(6, result.CarInfos[1].CarSeat);
            Assert.AreEqual("", result.CarInfos[1].VehicleInfo);
            Assert.AreEqual("risk1", result.CarInfos[1].Risk);
            Assert.AreEqual("IsZhuanXubao1", result.CarInfos[1].XinZhuanXu);
            Assert.AreEqual("SyVehicleClaimType1", result.CarInfos[1].SyVehicleClaimType);
            Assert.AreEqual("JqVehicleClaimType1", result.CarInfos[1].JqVehicleClaimType);
            Assert.AreEqual("VehicleStyle1", result.CarInfos[1].VehicleStyle);
            Assert.AreEqual("VehicleAlias1", result.CarInfos[1].VehicleAlias);
            Assert.AreEqual("VehicleYear1", result.CarInfos[1].VehicleYear);
        }
    }
}
