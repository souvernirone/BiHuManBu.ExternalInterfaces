using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.AgentChannelService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.AgentConfigService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.GetAgentConfigByCityService.Implementations;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using NSubstitute;
using NUnit.Framework;

namespace BiHuManBu.ExternalInterfaces.UnitTests.AgentConfigByCityServiceTests
{
    [TestFixture]
    public class GetLastQuotedResponseTests
    {
        [Test]
        public void GetLastQuotedResponse_ThorwException_ReurnNegavite()
        {
            //配置  
            var agentConfigByCityService = Substitute.For<IAgentConfigByCityService>();
            var agentRepository = Substitute.For<IAgentRepository>();
            var channelModelMapRedisService = Substitute.For<IChannelModelMapRedisService>();
            agentRepository.When(x=>x.GetAgent(Arg.Any<int>())).Do(info => { throw new Exception(); });

            var getAgentConfigByCitysService =
                new GetAgentConfigByCitysService(agentConfigByCityService, agentRepository, channelModelMapRedisService);

            //操作
            var result = getAgentConfigByCitysService.GetLastQuotedResponse(new GetLastQuotedRequest());

            //断言
            Assert.AreEqual(-10003,result.BusinessStatus);
        }
    }
}
