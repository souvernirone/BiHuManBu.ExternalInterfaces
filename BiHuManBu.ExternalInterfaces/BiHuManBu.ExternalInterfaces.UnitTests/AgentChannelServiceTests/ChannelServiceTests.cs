using System;
using System.Collections.Generic;
using System.Net;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services;
using BiHuManBu.ExternalInterfaces.Services.AgentChannelService.Implementations;
using BiHuManBu.ExternalInterfaces.Services.AgentChannelService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.AgentConfigService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ValidateService.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace BiHuManBu.ExternalInterfaces.UnitTests.AgentChannelServiceTests
{
    [TestFixture]
    public class ChannelServiceTests
    {
        [Test]
        public void GetChannelStatus_Expection_Throws()
        {
            //配置
            var agentConfigByCityService = Substitute.For<IAgentConfigByCityService>();
            var validateService = Substitute.For<IValidateService>();
            var channelModelMapRedisService = Substitute.For<IChannelModelMapRedisService>();

            validateService.Validate(Arg.Any<BaseRequest>(), null).Returns(new BaseResponse() { Status = HttpStatusCode.OK });
            agentConfigByCityService.GetAgentConfigByCity(Arg.Any<int>(), Arg.Any<int>()).Returns(x => { throw new Exception(); });
            var channelService = new ChannelService(channelModelMapRedisService, validateService, agentConfigByCityService);

            //操作
            var result = channelService.GetChannelStatus(new GetChannelStatusRequest() { Agent = 1 }, null);

            //Assert.Catch<Exception>(
            //    () => _getAgentInfoService.GetAgentModelFactory(Arg.Any<int>()));
            Assert.AreEqual(HttpStatusCode.ExpectationFailed, result.Status);
        }

        [Test]
        public void GetChannelStatus_AgentConfigIsNull_ReturnError()
        {
            var agentConfigByCityService = Substitute.For<IAgentConfigByCityService>();
            var validateService = Substitute.For<IValidateService>();
            var channelModelMapRedisService = Substitute.For<IChannelModelMapRedisService>();

            validateService.Validate(Arg.Any<BaseRequest>(), null).Returns(new BaseResponse() { Status = HttpStatusCode.OK });
            agentConfigByCityService.GetAgentConfigByCity(Arg.Any<int>(), Arg.Any<int>()).Returns(x => null);
            var channelService = new ChannelService(channelModelMapRedisService, validateService, agentConfigByCityService);

            //操作
            var result = channelService.GetChannelStatus(new GetChannelStatusRequest() { Agent = 1 }, null);
            //断言
            Assert.AreEqual(-1, result.ErrCode);
        }

        [Test]
        public void GetChannelStatus_CacheChannelListEmpty_ReturnError()
        {
            //配置
            var agentConfigByCityService = Substitute.For<IAgentConfigByCityService>();
            var validateService = Substitute.For<IValidateService>();
            var channelModelMapRedisService = Substitute.For<IChannelModelMapRedisService>();

            validateService.Validate(Arg.Any<BaseRequest>(), null).Returns(new BaseResponse() { Status = HttpStatusCode.OK });
            agentConfigByCityService.GetAgentConfigByCity(Arg.Any<int>(), Arg.Any<int>()).Returns(new List<bx_agent_config>());
            channelModelMapRedisService.GetCacheChannelList(Arg.Any<List<bx_agent_config>>()).Returns(new List<CacheChannelModel>());

            var channelService = new ChannelService(channelModelMapRedisService, validateService, agentConfigByCityService);

            //操作
            var result = channelService.GetChannelStatus(new GetChannelStatusRequest() { Agent = 1 }, null);
            //断言
            Assert.AreEqual(0, result.ErrCode);
        }
        
    }
}
