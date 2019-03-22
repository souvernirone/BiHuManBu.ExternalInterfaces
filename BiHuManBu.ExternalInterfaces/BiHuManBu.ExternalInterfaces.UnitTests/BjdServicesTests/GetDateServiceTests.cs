using System.Collections.Generic;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services;
using BiHuManBu.ExternalInterfaces.Services.AgentChannelService.Implementations;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using NSubstitute;
using NUnit.Framework;
using BiHuManBu.ExternalInterfaces.Models.ReportModel;
using BiHuManBu.ExternalInterfaces.Services.BjdServices.Achieves;

namespace BiHuManBu.ExternalInterfaces.UnitTests.BjdServicesTests
{
    [TestFixture]
    public class GetDateServiceTests
    {
        private static IQuoteResultRepository _quoteResultRepository = Substitute.For<IQuoteResultRepository>();
        GetDateService _getDateService = new GetDateService(_quoteResultRepository);
        [Test]
        public void GetDate_QuoteStatusGreaterNegative1()
        {

            _quoteResultRepository.GetStartDate(Arg.Any<long>()).Returns(x => new InsuranceStartDate() { BizStartDate = new System.DateTime(2018, 1, 1), ForceStartDate = new System.DateTime(2018, 2, 2) });

            var postBizStartDate = "";
            var result = _getDateService.GetDate(new bx_userinfo() { QuoteStatus = 1 }, out postBizStartDate);
            Assert.AreEqual("2018-02-02 00:00", result);
            Assert.AreEqual("2018-01-01 00:00", postBizStartDate);

        }
        [Test]
        public void GetDate_QuoteStatusLessNegative1()
        {
            _quoteResultRepository.GetStartDate(Arg.Any<long>()).Returns(x => null);
            var postBizStartDate = "";
            var result = _getDateService.GetDate(new bx_userinfo() { QuoteStatus = -1 }, out postBizStartDate);
            Assert.AreEqual("", result);
            Assert.AreEqual("", postBizStartDate);

        }

    }
}
