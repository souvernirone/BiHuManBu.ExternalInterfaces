using System.Collections.Generic;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services;
using BiHuManBu.ExternalInterfaces.Services.AgentChannelService.Implementations;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using NSubstitute;
using NUnit.Framework;

namespace BiHuManBu.ExternalInterfaces.UnitTests.AgentChannelServiceTests
{
    [TestFixture]
    public class ChannelModelMapRedisServiceTests
    {
        [Test]
        public void GetCacheChannelList_HashIsNull_ReturnsFalse()
        {
            IHashOperator hashOperator = Substitute.For<IHashOperator>();

            hashOperator.Get<CacheChannelModel>(Arg.Any<string>(), Arg.Any<string>()).Returns(x => null);
            ChannelModelMapRedisService channelModelMapRedisService = new ChannelModelMapRedisService(hashOperator);

            var result = channelModelMapRedisService.GetCacheChannelList(new List<bx_agent_config>() { new bx_agent_config() { isurl = 1, bx_url = "aaa" } });

            Assert.AreEqual(false, result.Any());
        }
        [Test]
        public void GetCacheChannelList_HashNotNull_ReturnsTrue()
        {
            IHashOperator hashOperator = Substitute.For<IHashOperator>();

            hashOperator.Get<CacheChannelModel>(Arg.Any<string>(), Arg.Any<string>()).Returns(x => new CacheChannelModel());
            ChannelModelMapRedisService channelModelMapRedisService = new ChannelModelMapRedisService(hashOperator);

            var result = channelModelMapRedisService.GetCacheChannelList(new List<bx_agent_config>() { new bx_agent_config() { isurl = 1, bx_url = "aaa" } });

            Assert.AreEqual(true, result.Any());
        }
        [Test]
        public void GetCacheChannelList_ConfigsHasMany_ReturnsList()
        {
            IHashOperator hashOperator = Substitute.For<IHashOperator>();

            hashOperator.Get<CacheChannelModel>(Arg.Any<string>(), Arg.Any<string>()).Returns(x => new CacheChannelModel());
            ChannelModelMapRedisService channelModelMapRedisService = new ChannelModelMapRedisService(hashOperator);

            List<bx_agent_config> list = new List<bx_agent_config>()
            {
                new bx_agent_config() { isurl = 1, bx_url = "aaa" }, 
                new bx_agent_config() { isurl = 1, bx_url = "bbb" }, 
                new bx_agent_config() { isurl = 1, bx_url = "ccc" } 
            
            };
            var result = channelModelMapRedisService.GetCacheChannelList(list);
            hashOperator.Received(3).Get<CacheChannelModel>(Arg.Any<string>(), Arg.Any<string>());
        }
    }
}
