
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.BjdServices.Extends;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using NSubstitute;
using NUnit.Framework;

namespace BiHuManBu.ExternalInterfaces.UnitTests
{
    [TestFixture]
    public class CreateActivityTests
    {
        IPreferentialActivityRepository _preferentialActivityRepository = Substitute.For<IPreferentialActivityRepository>();
        IAgentRepository _agentRepository = Substitute.For<IAgentRepository>();

       
        [Test]
        public void AddActivity_SelectRowNull_Return0()
        {
            #region 配置  Substitute

            _preferentialActivityRepository.GetListByType(Arg.Any<int>(),Arg.Any<string>()).Returns(x => null);

            CreateActivity createActivity = new CreateActivity(_agentRepository, _preferentialActivityRepository);

            #endregion

            #region 操作  Arg

            CreateOrUpdateBjdInfoRequest createOrUpdate = new CreateOrUpdateBjdInfoRequest();
            createOrUpdate.ActivityContent = "123";
            var result = createActivity.AddActivity(createOrUpdate, Arg.Any<int>());

            #endregion

            #region 断言

            Assert.AreNotEqual(0, result.activity_status);

            #endregion
        }

        [Test]
        public void AddActivity_ThorwException_Return0()
        {
            #region 配置  Substitute
            _agentRepository.When(x=>x.GetAgent(Arg.Any<int>())).Do(info =>
            {
                throw new Exception();
            });

            CreateActivity createActivity = new CreateActivity(_agentRepository, _preferentialActivityRepository);
            #endregion

            #region 操作 Arg

            CreateOrUpdateBjdInfoRequest createOrUpdate = new CreateOrUpdateBjdInfoRequest();
            createOrUpdate.ActivityContent = "123";
            var result = createActivity.AddActivity(createOrUpdate, Arg.Any<int>());

            #endregion

            #region 断言

            Assert.AreEqual(0, result.id);

            #endregion

        }
    }
}
