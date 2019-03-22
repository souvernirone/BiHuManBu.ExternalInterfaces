using System.Net;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.ValidateService.Achieves;
using BiHuManBu.ExternalInterfaces.Services.ValidateService.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace BiHuManBu.ExternalInterfaces.UnitTests.ValidateServiceTests
{
    [TestFixture]
    public class ValidateServiceTests
    {
        [Test]
        [TestCase(0, HttpStatusCode.Forbidden)]
        [TestCase(2, HttpStatusCode.Forbidden)]
        [TestCase(3, HttpStatusCode.Forbidden)]
        public void Validate_AgentNoUse_ReturnForbidden(int useState, HttpStatusCode expectedCode)
        {
            //配置
            IGetAgentInfoService getAgentInfoService = Substitute.For<IGetAgentInfoService>();
            ICheckGetSecCode checkGetSecCode = Substitute.For<ICheckGetSecCode>();

            getAgentInfoService.GetAgentModelFactory(Arg.Any<int>()).Returns(new bx_agent() { IsUsed = useState });
            ValidateService validateService = new ValidateService(getAgentInfoService, checkGetSecCode);

            //操作
            var result = validateService.Validate(new BaseRequest() { Agent = 1 }, null);
            //断言
            Assert.AreEqual(expectedCode, result.Status);
        }

        [Test]
        public void Validate_AgentCanUse_NextCalled()
        {
            //配置
            IGetAgentInfoService getAgentInfoService = Substitute.For<IGetAgentInfoService>();
            ICheckGetSecCode checkGetSecCode = Substitute.For<ICheckGetSecCode>();

            getAgentInfoService.GetAgentModelFactory(Arg.Any<int>()).Returns(new bx_agent() { IsUsed = 1 });
            ValidateService validateService = new ValidateService(getAgentInfoService, checkGetSecCode);

            //操作
            var result = validateService.Validate(new BaseRequest() { Agent = 1 }, null);

            //断言
            checkGetSecCode.Received().ValidateReqest(null, Arg.Any<string>(), Arg.Any<string>());
        }

        [Test]
        [TestCase(false, HttpStatusCode.Forbidden)]
        public void Validate_SecCodeNoPass_ReturnFalse(bool useState, HttpStatusCode expectedCode)
        {
            //配置
            IGetAgentInfoService getAgentInfoService = Substitute.For<IGetAgentInfoService>();
            ICheckGetSecCode checkGetSecCode = Substitute.For<ICheckGetSecCode>();

            getAgentInfoService.GetAgentModelFactory(Arg.Any<int>()).Returns(new bx_agent() { IsUsed = 1 });
            checkGetSecCode.ValidateReqest(null, Arg.Any<string>(), Arg.Any<string>()).Returns(useState);

            ValidateService validateService = new ValidateService(getAgentInfoService, checkGetSecCode);

            //操作
            var result = validateService.Validate(new BaseRequest() { Agent = 1 }, null);
            //断言
            Assert.AreEqual(expectedCode, result.Status);
        }
    }
}
