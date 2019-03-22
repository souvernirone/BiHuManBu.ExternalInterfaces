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
using System;
using BiHuManBu.ExternalInterfaces.Models.ReportModel;

namespace BiHuManBu.ExternalInterfaces.UnitTests.BjdServicesTests
{
    [TestFixture]
    public class SetQuoteReqServiceTests
    {
        private static IQuoteReqCarinfoRepository _quoteReqCarinfoRepository = Substitute.For<IQuoteReqCarinfoRepository>();
        private static SetQuoteReqService _setQuoteReqService = new SetQuoteReqService(_quoteReqCarinfoRepository);
        [Test]
        public void SetQuoteReq_QuoteReqNull()
        {
            _quoteReqCarinfoRepository.Find(Arg.Any<long>()).Returns(x => null);
            string postBizStartDate = "2018-1-1";
            string postForceStartDate = "2017-1-1";
            var result = _setQuoteReqService.SetQuoteReq(new MyBaoJiaViewModel(), new bx_userinfo(), ref postBizStartDate, ref postForceStartDate);
            Assert.AreEqual(0, result.IsPublic);
            Assert.AreEqual(0, result.CoRealValue);
            Assert.AreEqual("", result.ReqInfo.AutoMoldCode);
            Assert.AreEqual(2, result.ReqInfo.IsNewCar);
            Assert.AreEqual(0, result.ReqInfo.NegotiatePrice);
            Assert.AreEqual(0, result.ReqInfo.IsPublic);
            Assert.AreEqual(0, result.ReqInfo.CarUsedType);
            Assert.AreEqual(-1, result.ReqInfo.AutoMoldCodeSource);
            Assert.AreEqual("", result.ReqInfo.DriveLicenseTypeName);
            Assert.AreEqual("", result.ReqInfo.DriveLicenseTypeValue);
            Assert.AreEqual("-1", result.ReqInfo.SeatUpdated);
            Assert.AreEqual("0", result.ReqInfo.RequestActualDiscounts);
            Assert.AreEqual("0", result.ReqInfo.RequestIsPaFloorPrice);

        }
        [Test]
        public void SetQuoteReq_QuoteReqNotNull_QuoteStatusNegitive1()
        {
            _quoteReqCarinfoRepository.Find(Arg.Any<long>()).Returns(x => new bx_quotereq_carinfo()
            {
                force_start_date = new DateTime(2016, 1, 1),
                biz_start_date = new DateTime(2016, 1, 2),
                is_public = 1,
                is_newcar = 1,
                auto_model_code = "auto_model_code",
                co_real_value = 0.1m,
                car_used_type = 1,
                seat_count = 5
            });
            string postBizStartDate = "2018-1-1";
            string postForceStartDate = "2017-1-1";
            var result = _setQuoteReqService.SetQuoteReq(new MyBaoJiaViewModel(), new bx_userinfo() { QuoteStatus = -1 }, ref postBizStartDate, ref postForceStartDate);
            Assert.AreEqual(1, result.IsPublic);
            Assert.AreEqual(0.1, result.CoRealValue);
            Assert.AreEqual("auto_model_code", result.ReqInfo.AutoMoldCode);
            Assert.AreEqual(1, result.ReqInfo.IsNewCar);
            Assert.AreEqual(0.1m, result.ReqInfo.NegotiatePrice);
            Assert.AreEqual(1, result.ReqInfo.IsPublic);
            Assert.AreEqual(1, result.ReqInfo.CarUsedType);
            Assert.AreEqual(-1, result.ReqInfo.AutoMoldCodeSource);
            Assert.AreEqual("", result.ReqInfo.DriveLicenseTypeName);
            Assert.AreEqual("", result.ReqInfo.DriveLicenseTypeValue);
            Assert.AreEqual("-1", result.ReqInfo.SeatUpdated);
            Assert.AreEqual("2016-01-02 00:00", postBizStartDate);
            Assert.AreEqual("2016-01-01 00:00", postForceStartDate);
            Assert.AreEqual("0", result.ReqInfo.RequestActualDiscounts);
            Assert.AreEqual("0", result.ReqInfo.RequestIsPaFloorPrice);
        }
        [Test]
        public void SetQuoteReq_QuoteReqNotNull_QuoteStatusGreter1()
        {
            _quoteReqCarinfoRepository.Find(Arg.Any<long>()).Returns(x => new bx_quotereq_carinfo()
            {
                force_start_date = new DateTime(2016, 1, 1),
                biz_start_date = new DateTime(2016, 1, 2),
                is_public = 1,
                is_newcar = 1,
                auto_model_code = "auto_model_code",
                co_real_value = 0.1m,
                car_used_type = 1,
                seat_count = 5
            });
            string postBizStartDate = "2018-1-1";
            string postForceStartDate = "2017-1-1";
            var result = _setQuoteReqService.SetQuoteReq(new MyBaoJiaViewModel(), new bx_userinfo() { QuoteStatus = 1 }, ref postBizStartDate, ref postForceStartDate);
            Assert.AreEqual(1, result.IsPublic);
            Assert.AreEqual(0.1, result.CoRealValue);
            Assert.AreEqual("auto_model_code", result.ReqInfo.AutoMoldCode);
            Assert.AreEqual(1, result.ReqInfo.IsNewCar);
            Assert.AreEqual(0.1m, result.ReqInfo.NegotiatePrice);
            Assert.AreEqual(1, result.ReqInfo.IsPublic);
            Assert.AreEqual(1, result.ReqInfo.CarUsedType);
            Assert.AreEqual(-1, result.ReqInfo.AutoMoldCodeSource);
            Assert.AreEqual("", result.ReqInfo.DriveLicenseTypeName);
            Assert.AreEqual("", result.ReqInfo.DriveLicenseTypeValue);
            Assert.AreEqual("-1", result.ReqInfo.SeatUpdated);
            Assert.AreEqual("2018-1-1", postBizStartDate);
            Assert.AreEqual("2017-1-1", postForceStartDate);
            Assert.AreEqual("0", result.ReqInfo.RequestActualDiscounts);
            Assert.AreEqual("0", result.ReqInfo.RequestIsPaFloorPrice);
        }
    }
}
