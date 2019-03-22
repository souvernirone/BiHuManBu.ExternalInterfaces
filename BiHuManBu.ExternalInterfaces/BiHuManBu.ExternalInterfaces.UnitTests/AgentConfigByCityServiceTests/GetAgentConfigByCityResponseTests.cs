using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.AgentChannelService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.AgentConfigService.Implementations;
using BiHuManBu.ExternalInterfaces.Services.AgentConfigService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.GetAgentConfigByCityService.Implementations;
using NSubstitute;
using NSubstitute.Core;
using NUnit.Framework;

namespace BiHuManBu.ExternalInterfaces.UnitTests.AgentConfigByCityServiceTests
{
    [TestFixture]
    public class GetAgentConfigByCityResponseTests
    {
        [Test]
        public void GetAgentConfigByCityResponse_ThorwException_ReurnNegavite()
        {
            #region 配置 Substitute

            var agentConfigByCityService = Substitute.For<IAgentConfigByCityService>();
            var agentRepository = Substitute.For<IAgentRepository>();
            var channelModelMapRedisService = Substitute.For<IChannelModelMapRedisService>();

            agentConfigByCityService.When(x => x.GetAgentConfigByCity(Arg.Any<int>(), Arg.Any<int>())).Do(info =>
            {
                throw new Exception();
            });


            var agentConfigByCity = new GetAgentConfigByCitysService(agentConfigByCityService, agentRepository,
                channelModelMapRedisService);

            #endregion

            #region 操作  Arg

            var request = agentConfigByCity.GetAgentConfigByCityResponse(0, 0);

            #endregion

            #region 断言  Assert

            Assert.AreEqual(-10003, request.BusinessStatus);

            #endregion
        }

        [Test]
        public void GetAgentConfigByCityResponse_GetAgentConfigByCityIsNull_Reurn0()
        {
            #region 配置 Substitute

            var agentConfigByCityService = Substitute.For<IAgentConfigByCityService>();
            var agentRepository = Substitute.For<IAgentRepository>();
            var channelModelMapRedisService = Substitute.For<IChannelModelMapRedisService>();

            agentConfigByCityService.GetAgentConfigByCity(Arg.Any<int>(), Arg.Any<int>())
                .Returns(info => null);


            var agentConfigByCity = new GetAgentConfigByCitysService(agentConfigByCityService, agentRepository,
                channelModelMapRedisService);

            #endregion

            #region 操作  Arg

            var request = agentConfigByCity.GetAgentConfigByCityResponse(0, 0);

            #endregion

            #region 断言  Assert

            Assert.AreEqual(0, request.BusinessStatus);

            #endregion
        }
    }
}
