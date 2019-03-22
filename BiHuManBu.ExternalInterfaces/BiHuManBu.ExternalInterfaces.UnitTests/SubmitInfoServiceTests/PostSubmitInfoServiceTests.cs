using System;
using System.Collections.Generic;
using System.Net;
using BiHuManBu.ExternalInterfaces.Infrastructure.MessageCenter;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services;
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
    public class PostSubmitInfoServiceTests
    {
        [Test]
        public void PostSubmitInfo_ValidateNoPass_ReturnForbidden()
        {
            //配置
            IValidateService validateService = Substitute.For<IValidateService>();
            IMessageCenter messageCenter = Substitute.For<IMessageCenter>();
            IRemoveHeBaoKey removeHeBaoKeyService = Substitute.For<IRemoveHeBaoKey>();
            IPostValidate postValidate=Substitute.For<IPostValidate>();
            validateService.Validate(Arg.Any<PostSubmitInfoRequest>(), Arg.Any<IEnumerable<KeyValuePair<string, string>>>()).Returns(new BaseResponse() { Status = HttpStatusCode.Forbidden });
            PostSubmitInfoService postSubmitInfoService = new PostSubmitInfoService(validateService, messageCenter, removeHeBaoKeyService, postValidate);
            //操作
            var result = postSubmitInfoService.PostSubmitInfo(new PostSubmitInfoRequest(), null);
            //断言
            Assert.AreEqual(HttpStatusCode.Forbidden, result.Status);
        }

        [Test]
        public void PostSubmitInfo_SubmitInfoValidateNoPass_ReturnNotAcceptable()
        {
            //配置
            IValidateService validateService = Substitute.For<IValidateService>();
            IMessageCenter messageCenter = Substitute.For<IMessageCenter>();
            IRemoveHeBaoKey removeHeBaoKeyService = Substitute.For<IRemoveHeBaoKey>();
            IPostValidate postValidate = Substitute.For<IPostValidate>();
            validateService.Validate(Arg.Any<PostSubmitInfoRequest>(), Arg.Any<IEnumerable<KeyValuePair<string, string>>>()).Returns(new BaseResponse() { Status = HttpStatusCode.OK });
            postValidate.SubmitInfoValidate(Arg.Any<PostSubmitInfoRequest>()).Returns(Tuple.Create<BaseResponse, bx_userinfo, bx_submit_info>(new BaseResponse(){Status = HttpStatusCode.NotAcceptable}, null, null));
            PostSubmitInfoService postSubmitInfoService = new PostSubmitInfoService(validateService, messageCenter, removeHeBaoKeyService, postValidate);
            //操作
            var result = postSubmitInfoService.PostSubmitInfo(new PostSubmitInfoRequest(), null);
            //断言
            Assert.AreEqual(HttpStatusCode.NotAcceptable, result.Status);
        }

        [Test]
        public void PostSubmitInfo_HeBaoKeyIsException_ReturnExpectationFailed()
        {
            //配置
            IValidateService validateService = Substitute.For<IValidateService>();
            IMessageCenter messageCenter = Substitute.For<IMessageCenter>();
            IRemoveHeBaoKey removeHeBaoKey = Substitute.For<IRemoveHeBaoKey>();
            IPostValidate postValidate = Substitute.For<IPostValidate>();
            validateService.Validate(Arg.Any<PostSubmitInfoRequest>(), Arg.Any<IEnumerable<KeyValuePair<string, string>>>()).Returns(new BaseResponse() { Status = HttpStatusCode.OK });
            postValidate.SubmitInfoValidate(Arg.Any<PostSubmitInfoRequest>()).Returns(Tuple.Create<BaseResponse, bx_userinfo, bx_submit_info>(new BaseResponse() { Status = HttpStatusCode.OK }, new bx_userinfo(), new bx_submit_info()));
            removeHeBaoKey.When(x => x.RemoveHeBao(Arg.Any<PostSubmitInfoRequest>())).Do(info =>
            {
                throw new RedisOperateException();
            });
            PostSubmitInfoService postSubmitInfoService = new PostSubmitInfoService(validateService, messageCenter, removeHeBaoKey, postValidate);
            //操作
            var result = postSubmitInfoService.PostSubmitInfo(new PostSubmitInfoRequest(){Source = 1,LicenseNo = "京",Agent = 102,CustKey = "aaaaa",RenewalCarType = 0}, null);
            //断言
            Assert.AreEqual(HttpStatusCode.ExpectationFailed, result.Status);
        }

        [Test]
        public void PostSubmitInfo_MessageCenterIsNull_ReturnOK()
        {
            //配置
            IValidateService validateService = Substitute.For<IValidateService>();
            IMessageCenter messageCenter = Substitute.For<IMessageCenter>();
            IRemoveHeBaoKey removeHeBaoKey = Substitute.For<IRemoveHeBaoKey>();
            IPostValidate postValidate = Substitute.For<IPostValidate>();
            validateService.Validate(Arg.Any<PostSubmitInfoRequest>(), Arg.Any<IEnumerable<KeyValuePair<string, string>>>()).Returns(new BaseResponse() { Status = HttpStatusCode.OK });
            postValidate.SubmitInfoValidate(Arg.Any<PostSubmitInfoRequest>()).Returns(Tuple.Create<BaseResponse, bx_userinfo, bx_submit_info>(new BaseResponse() { Status = HttpStatusCode.OK }, new bx_userinfo(), new bx_submit_info()));
            removeHeBaoKey.RemoveHeBao(Arg.Any<PostSubmitInfoRequest>()).Returns(x => "test-string");
            messageCenter.SendToMessageCenter(Arg.Any<string>(), Arg.Any<string>()).Returns(x => null);
            PostSubmitInfoService postSubmitInfoService = new PostSubmitInfoService(validateService, messageCenter, removeHeBaoKey, postValidate);
            //操作
            var result = postSubmitInfoService.PostSubmitInfo(new PostSubmitInfoRequest() { Source = 1 }, null);
            //断言
            Assert.AreEqual(HttpStatusCode.OK, result.Status);
        }

        [Test]
        public void PostSubmitInfo_MessageCenterIsNotNull_ReturnOK()
        {
            //配置
            IValidateService validateService = Substitute.For<IValidateService>();
            IMessageCenter messageCenter = Substitute.For<IMessageCenter>();
            IRemoveHeBaoKey removeHeBaoKey = Substitute.For<IRemoveHeBaoKey>();
            IPostValidate postValidate = Substitute.For<IPostValidate>();
            validateService.Validate(Arg.Any<PostSubmitInfoRequest>(), Arg.Any<IEnumerable<KeyValuePair<string, string>>>()).Returns(new BaseResponse() { Status = HttpStatusCode.OK });
            postValidate.SubmitInfoValidate(Arg.Any<PostSubmitInfoRequest>()).Returns(Tuple.Create<BaseResponse, bx_userinfo, bx_submit_info>(new BaseResponse() { Status = HttpStatusCode.OK }, new bx_userinfo(), new bx_submit_info()));
            removeHeBaoKey.RemoveHeBao(Arg.Any<PostSubmitInfoRequest>()).Returns(x => "test-string");
            messageCenter.SendToMessageCenter(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(x => new MessageResult());
            PostSubmitInfoService postSubmitInfoService = new PostSubmitInfoService(validateService, messageCenter, removeHeBaoKey, postValidate);
            //操作
            var result = postSubmitInfoService.PostSubmitInfo(new PostSubmitInfoRequest() { Source = 1 }, null);
            //断言
            Assert.AreEqual(HttpStatusCode.OK, result.Status);
        }

        [Test]
        public void PostSubmitInfo_MessageCenterIsException_ReturnExpectationFailed()
        {
            //配置
            IValidateService validateService = Substitute.For<IValidateService>();
            IMessageCenter messageCenter = Substitute.For<IMessageCenter>();
            IRemoveHeBaoKey removeHeBaoKey = Substitute.For<IRemoveHeBaoKey>();
            IPostValidate postValidate = Substitute.For<IPostValidate>();
            validateService.Validate(Arg.Any<PostSubmitInfoRequest>(), Arg.Any<IEnumerable<KeyValuePair<string, string>>>()).Returns(new BaseResponse() { Status = HttpStatusCode.OK });
            postValidate.SubmitInfoValidate(Arg.Any<PostSubmitInfoRequest>()).Returns(Tuple.Create<BaseResponse, bx_userinfo, bx_submit_info>(new BaseResponse() { Status = HttpStatusCode.OK }, new bx_userinfo(), new bx_submit_info()));
            removeHeBaoKey.RemoveHeBao(Arg.Any<PostSubmitInfoRequest>()).Returns(x => "test-string");
            messageCenter.When(x => x.SendToMessageCenter(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())).Do(info =>
            {
                throw new MessageException();
            });
            PostSubmitInfoService postSubmitInfoService = new PostSubmitInfoService(validateService, messageCenter, removeHeBaoKey, postValidate);
            //操作
            var result = postSubmitInfoService.PostSubmitInfo(new PostSubmitInfoRequest() { Source = 1 }, null);
            //断言
            Assert.AreEqual(HttpStatusCode.ExpectationFailed, result.Status);
        }
    }
}
