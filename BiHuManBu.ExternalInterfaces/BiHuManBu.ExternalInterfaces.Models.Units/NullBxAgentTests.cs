using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Models.PartialModels.bx_agent;
using NUnit.Framework;

namespace BiHuManBu.ExternalInterfaces.Models.Units
{
    [TestFixture]
    public class NullBxAgentTests
    {
        [Test]
        public void AgentCanUse_NullObject_ReturnFalse()
        {
            IBxAgent agent = new NullBxAgent();
            var result = agent.AgentCanUse();
            Assert.IsFalse(result);
        }
    }
}
