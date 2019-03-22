using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Infrastructure.MessageCenter;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.SubmitInfoService.Implementations;
using BiHuManBu.ExternalInterfaces.Services.SubmitInfoService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.ValidateService.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace BiHuManBu.ExternalInterfaces.UnitTests.SubmitInfoServiceTests
{
    [TestFixture]
    public class PostValidateTests
    {
        [Test]
        public void PostValidate_UserInfoIsNull_ReturnNotAcceptable()
        {
            //配置
            IUserInfoRepository userInfoRepository = Substitute.For<IUserInfoRepository>();
            ISubmitInfoRepository submitInfoRepository = Substitute.For<ISubmitInfoRepository>();
            userInfoRepository.FindByOpenIdAndLicense(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<int>(), Arg.Any<int>()).Returns(x => null);
            PostValidate postValidate = new PostValidate(userInfoRepository, submitInfoRepository);
            //操作
            var result = postValidate.SubmitInfoValidate(new PostSubmitInfoRequest());
            //断言
            Assert.AreEqual(HttpStatusCode.NotAcceptable, result.Item1.Status);
        }

        [Test]
        [TestCase(0, 0)]
        [TestCase(3, 4)]
        //source是和，requestsource是单个值
        public void PostValidate_SourceNotRule_ReturnNotAcceptable(int source, int requestSource)
        {
            //配置
            IUserInfoRepository userInfoRepository = Substitute.For<IUserInfoRepository>();
            ISubmitInfoRepository submitInfoRepository = Substitute.For<ISubmitInfoRepository>();
            userInfoRepository.FindByOpenIdAndLicense(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<int>(), Arg.Any<int>()).Returns(new bx_userinfo() { Source = source, IsSingleSubmit = source });
            PostValidate postValidate = new PostValidate(userInfoRepository, submitInfoRepository);
            //操作
            var result = postValidate.SubmitInfoValidate(new PostSubmitInfoRequest());
            //断言
            Assert.AreEqual(HttpStatusCode.NotAcceptable, result.Item1.Status);
        }

        [Test]
        public void PostValidate_SubmitInfoIsNull_ReturnNotAcceptable()
        {
            //配置
            IUserInfoRepository userInfoRepository = Substitute.For<IUserInfoRepository>();
            ISubmitInfoRepository submitInfoRepository = Substitute.For<ISubmitInfoRepository>();
            userInfoRepository.FindByOpenIdAndLicense(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<int>(), Arg.Any<int>()).Returns(new bx_userinfo() { Source = 1, IsSingleSubmit = 1 });
            submitInfoRepository.GetSubmitInfo(Arg.Any<int>(), Arg.Any<int>()).Returns(x => null);
            PostValidate postValidate = new PostValidate(userInfoRepository, submitInfoRepository);
            //操作
            var result = postValidate.SubmitInfoValidate(new PostSubmitInfoRequest());
            //断言
            Assert.AreEqual(HttpStatusCode.NotAcceptable, result.Item1.Status);
        }
    }
}
