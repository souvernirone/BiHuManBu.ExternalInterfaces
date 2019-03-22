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
    public class SetDateServiceTests
    {
        private static ILastInfoRepository _lastInfoRepository = Substitute.For<ILastInfoRepository>();
        private static SetDateService _setDateService = new SetDateService(_lastInfoRepository);
        [Test]
        public void SetDateService_EndDateNull()
        {
            _lastInfoRepository.GetEndDate(Arg.Any<long>()).Returns(x => null);
            var result = _setDateService.SetDate(new MyBaoJiaViewModel(), new bx_userinfo() { Id = 1 }, "2018-1-1", "2017-1-1");
            Assert.AreEqual("2018-1-1", result.BizStartDate);
            Assert.AreEqual("2017-1-1", result.ForceStartDate);
            Assert.AreEqual("", result.LastBusinessEndDdate);
            Assert.AreEqual("", result.LastEndDate);
        }
        [Test]
        public void SetDateService_EndDateNotNull()
        {
            _lastInfoRepository.GetEndDate(Arg.Any<long>()).Returns(x => new InsuranceEndDate
                           {
                               LastBusinessEndDdate = "2015-1-1",
                               LastForceEndDdate = "2016-1-1"
                           });
            var result = _setDateService.SetDate(new MyBaoJiaViewModel(), new bx_userinfo() { Id = 1 }, "2018-1-1", "2017-1-1");
            Assert.AreEqual("2018-1-1", result.BizStartDate);
            Assert.AreEqual("2017-1-1", result.ForceStartDate);
            Assert.AreEqual("2015-1-1", result.LastBusinessEndDdate);
            Assert.AreEqual("2016-1-1", result.LastEndDate);
        }
    }
}
