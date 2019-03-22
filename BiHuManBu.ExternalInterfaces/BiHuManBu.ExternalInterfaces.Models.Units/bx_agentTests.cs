using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace BiHuManBu.ExternalInterfaces.Models.Units
{
    [TestFixture]
    public class bx_agentTests
    {
        [Test]
        public void AgentCanUse_IsUsedEqaula1_ReturnTrue()
        {
            bx_agent agent = new bx_agent(){IsUsed = 1};
            var result = agent.AgentCanUse();
            Assert.IsTrue(result);
        }

        [Test]
        public void AgentCanUse_IsUsedEquals0_ReturnFalse()
        {
            bx_agent agent=new bx_agent()
            {
                IsUsed = 0
            };
            var result = agent.AgentCanUse();

            Assert.IsFalse(result);
        }
        [Test]
        public void AgentCanUse_IsUsedEquals2_ReturnFalse()
        {
            bx_agent agent = new bx_agent()
            {
                IsUsed = 0
            };
            var result = agent.AgentCanUse();

            Assert.IsFalse(result);
        }



        [Test]
        public void AgentCanQuote_IsUsedEquals1AndQuoteEquals0ReturnFalse()
        {
            bx_agent agent = new bx_agent()
            {
                IsUsed = 1,
                IsQuote = 0
            };

            var result = agent.AgentCanQuote();

            Assert.IsFalse(result);
        }

        [Test]
        public void AgentCanQuote_IsUsedEquals0AndQuoteEquals0ReturnFalse()
        {
            bx_agent agent = new bx_agent()
            {
                IsUsed = 0,
                IsQuote = 0
            };

            var result = agent.AgentCanQuote();

            Assert.IsFalse(result);
        }
        [Test]
        public void AgentCanQuote_IsUsedEquals0AndQuoteEquals1ReturnFalse()
        {
            bx_agent agent = new bx_agent()
            {
                IsUsed = 0,
                IsQuote = 1
            };

            var result = agent.AgentCanQuote();

            Assert.IsFalse(result);
        }
        [Test]
        public void AgentCanQuote_IsUsedEquals1AndQuoteEquals1ReturnTrue()
        {
            bx_agent agent = new bx_agent()
            {
                IsUsed = 1,
                IsQuote = 1
            };

            var result = agent.AgentCanQuote();
            Assert.IsTrue(result);
        }

        [Test]
        public void AgentCanSubmit_IsUsedEquals0AndQuoteEquals2AndSubmitEquals2ReturnFalse()
        {
            bx_agent agent = new bx_agent()
            {
                IsUsed = 0,
                IsQuote = 2,
                IsSubmit = 2
            };

            var result = agent.AgentCanSubmit();

            Assert.IsFalse(result);
        }

        [Test]
        public void AgentCanSubmit_IsUsedEquals1AndQuoteEquals2AndSubmitEquals2ReturnFalse()
        {
            bx_agent agent = new bx_agent()
            {
                IsUsed = 1,
                IsQuote = 2,
                IsSubmit = 2
            };

            var result = agent.AgentCanSubmit();

            Assert.IsFalse(result);
        }

        [Test]
        public void AgentCanSubmit_IsUsedEquals1AndQuoteEquals1AndSubmitEquals2ReturnFalse()
        {
            bx_agent agent = new bx_agent()
            {
                IsUsed = 1,
                IsQuote = 1,
                IsSubmit = 2
            };

            var result = agent.AgentCanSubmit();

            Assert.IsFalse(result);
        }

        [Test]
        public void AgentCanSubmit_IsUsedEquals1AndQuoteEquals1AndSubmitEquals1ReturnTrue()
        {
            bx_agent agent = new bx_agent()
            {
                IsUsed = 1,
                IsQuote = 1,
                IsSubmit =1
            };

            var result = agent.AgentCanSubmit();

            Assert.IsTrue(result);
        }

    }
}
