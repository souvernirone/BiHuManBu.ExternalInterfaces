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
    public class SetAgentServiceTests
    {
        private static IAgentRepository _agentRepository = Substitute.For<IAgentRepository>();
        private static SetAgentService _setAgentService = new SetAgentService(_agentRepository);
        [Test]
        public void SetAgent_ChildAgent0()
        {
            _agentRepository.GetAgent(Arg.Any<int>()).Returns(x => new bx_agent { AgentName = "马小翠", Mobile = "18600463219" });

            var result = _setAgentService.SetAgent(new MyBaoJiaViewModel() { }, new bx_userinfo() { Agent = "1" }, new GetMyBjdDetailRequest() { ChildAgent = 0 });
            Assert.AreEqual("马小翠", result.CurAgentName);
            Assert.AreEqual("18600463219", result.CurAgentMobile);
            Assert.AreEqual(1, result.IsShowCalc);
        }
        [Test]
        public void SetAgent_ChildAgentNot0()
        {
            _agentRepository.GetAgent(Arg.Any<int>()).Returns(x => new bx_agent { AgentName = "马小翠", Mobile = "18600463219", IsShow = 0 });

            var result = _setAgentService.SetAgent(new MyBaoJiaViewModel() { }, new bx_userinfo() { Agent = "1" }, new GetMyBjdDetailRequest() { ChildAgent = 1 });
            Assert.AreEqual("马小翠", result.CurAgentName);
            Assert.AreEqual("18600463219", result.CurAgentMobile);
            Assert.AreEqual(0, result.IsShowCalc);
        }
    }
}
