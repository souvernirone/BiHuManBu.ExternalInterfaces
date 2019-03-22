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

namespace BiHuManBu.ExternalInterfaces.UnitTests.BjdServicesTests
{
    [TestFixture]
    public class CheckRequestServiceTests
    {
        private static IValidateService _validateService = Substitute.For<IValidateService>();
        CheckRequestService _checkRequestService = new CheckRequestService(_validateService);
        [Test]
        public void CheckRequest_UserInfoIsNull_ReturnsFalse()
        {
            IValidateService _validateService = Substitute.For<IValidateService>();
            _validateService.ValidateAgent(Arg.Any<GetMyBjdDetailRequest>()).Returns(x => new BaseResponse() { Status = HttpStatusCode.OK });
            var result = _checkRequestService.CheckRequest(null, new GetMyBjdDetailRequest() { });
            Assert.AreEqual(-10000, result.BusinessStatus);
        }
        [Test]
        public void CheckRequest_NewRateNotNull_ReturnsFalse()
        {
            _validateService.ValidateAgent(Arg.Any<GetMyBjdDetailRequest>()).Returns(x => new BaseResponse() { Status = HttpStatusCode.OK });
            var result = _checkRequestService.CheckRequest(new bx_userinfo() { Id = 100, IsSingleSubmit = 1 }, new GetMyBjdDetailRequest() { NewRate = "0.2" });
            Assert.AreEqual(0, result.BusinessStatus);
        }
        [Test]
        public void CheckRequest_NewRateNotNull_ReturnsTrue()
        {
            _validateService.ValidateAgent(Arg.Any<GetMyBjdDetailRequest>()).Returns(x => new BaseResponse() { Status = HttpStatusCode.OK });
            var result = _checkRequestService.CheckRequest(new bx_userinfo() { Id = 100, IsSingleSubmit = 8 }, new GetMyBjdDetailRequest() { NewRate = "0.2" });
            Assert.AreEqual(1, result.BusinessStatus);
        }
        [Test]
        public void CheckRequest_ValidateAgentFalse_ReturnsTrue()
        {
            _validateService.ValidateAgent(Arg.Any<GetMyBjdDetailRequest>()).Returns(x => new BaseResponse() { Status = HttpStatusCode.Forbidden });
            var result = _checkRequestService.CheckRequest(new bx_userinfo() { Id = 100, IsSingleSubmit = 8 }, new GetMyBjdDetailRequest() { NewRate = "0.2" });
            Assert.AreEqual(-10000, result.BusinessStatus);
        }
    }
}
