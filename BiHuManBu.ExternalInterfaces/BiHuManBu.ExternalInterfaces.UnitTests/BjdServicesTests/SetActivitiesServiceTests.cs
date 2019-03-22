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
    public class SetActivitiesServiceTests
    {
        private static IPreferentialActivityRepository _preferentialActivityRepository = Substitute.For<IPreferentialActivityRepository>();
        SetActivitiesService _setActivitiesService = new SetActivitiesService(_preferentialActivityRepository);
        [Test]
        public void SetActivities_ActivityNull()
        {

            _preferentialActivityRepository.GetActivityByIds(Arg.Any<string>()).Returns(x => new List<bx_preferential_activity>() { new bx_preferential_activity() { activity_name = "name", activity_content = "content" } });
            var result = _setActivitiesService.SetActivities(new MyBaoJiaViewModel() { }, new GetMyBjdDetailRequest() { Activity = "name" });
            Assert.AreEqual("name", result.Activity.FirstOrDefault().ActivityName);
            Assert.AreEqual("content", result.Activity.FirstOrDefault().ActivityContent);

        }
        [Test]
        public void SetActivities_ActivityNotNull_Return()
        {
            _preferentialActivityRepository.GetActivityByIds(Arg.Any<string>()).Returns(x => new List<bx_preferential_activity>() { new bx_preferential_activity() { activity_name = "name", activity_content = "content" } });
            var result = _setActivitiesService.SetActivities(new MyBaoJiaViewModel() { }, new GetMyBjdDetailRequest() { Activity = null });
            Assert.AreEqual(null, result.Activity.FirstOrDefault());

        }
    }
}
