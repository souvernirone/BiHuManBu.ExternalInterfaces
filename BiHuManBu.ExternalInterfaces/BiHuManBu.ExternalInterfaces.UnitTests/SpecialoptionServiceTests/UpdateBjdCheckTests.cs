using System;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.BjdServices.Extends;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using NSubstitute;
using NUnit.Framework;

namespace BiHuManBu.ExternalInterfaces.UnitTests.SpecialoptionServiceTests
{
    [TestFixture]
    public class UpdateBjdCheckTests
    {

        [Test]
        public void Valid_submitInfoIsNull_Return0()
        {
            #region 配置  Substitute
            ISaveQuoteRepository _saveQuoteRepository = Substitute.For<ISaveQuoteRepository>();
            ISubmitInfoRepository _submitInfoRepository = Substitute.For<ISubmitInfoRepository>();
            IQuoteResultRepository _quoteResultRepository = Substitute.For<IQuoteResultRepository>();
            IUserInfoRepository _userInfoRepository = Substitute.For<IUserInfoRepository>();

            _submitInfoRepository.GetSubmitInfo(Arg.Any<long>(), Arg.Any<int>()).Returns(x => null);

            UpdateBjdCheck updateBjdCheck = new UpdateBjdCheck(_saveQuoteRepository, _submitInfoRepository,
                _quoteResultRepository, _userInfoRepository);
            #endregion

            #region 操作  Arg

            bx_submit_info submitInfo = new bx_submit_info();
            bx_quoteresult quoteresult = new bx_quoteresult();
            bx_savequote savequote = new bx_savequote();
            bx_userinfo userinfo = new bx_userinfo();
            var result = updateBjdCheck.Valid(new CreateOrUpdateBjdInfoRequest());

            #endregion

            #region 断言  Assert

            Assert.AreEqual(0, result.State);

            #endregion
        }

        [Test]
        public void Valid_quoteresultIsNull_Return0()
        {
            #region 配置  Substitute
            ISaveQuoteRepository _saveQuoteRepository = Substitute.For<ISaveQuoteRepository>();
            ISubmitInfoRepository _submitInfoRepository = Substitute.For<ISubmitInfoRepository>();
            IQuoteResultRepository _quoteResultRepository = Substitute.For<IQuoteResultRepository>();
            IUserInfoRepository _userInfoRepository = Substitute.For<IUserInfoRepository>();
            _submitInfoRepository.GetSubmitInfo(Arg.Any<long>(), Arg.Any<int>()).Returns(x => new bx_submit_info());
            _quoteResultRepository.GetQuoteResultByBuid(Arg.Any<long>()).Returns(x => null);

            UpdateBjdCheck updateBjdCheck = new UpdateBjdCheck(_saveQuoteRepository, _submitInfoRepository,
                _quoteResultRepository, _userInfoRepository);
            #endregion

            #region 操作  Arg

            bx_submit_info submitInfo = new bx_submit_info();
            bx_quoteresult quoteresult = new bx_quoteresult();
            bx_savequote savequote = new bx_savequote();
            bx_userinfo userinfo = new bx_userinfo();
            var result = updateBjdCheck.Valid(new CreateOrUpdateBjdInfoRequest());

            #endregion

            #region 断言  Assert

            Assert.AreEqual(0, result.State);

            #endregion
        }

        [Test]
        public void Valid_savequoteIsNull_Return0()
        {
            #region 配置  Substitute
            ISaveQuoteRepository _saveQuoteRepository = Substitute.For<ISaveQuoteRepository>();
            ISubmitInfoRepository _submitInfoRepository = Substitute.For<ISubmitInfoRepository>();
            IQuoteResultRepository _quoteResultRepository = Substitute.For<IQuoteResultRepository>();
            IUserInfoRepository _userInfoRepository = Substitute.For<IUserInfoRepository>();

            _submitInfoRepository.GetSubmitInfo(Arg.Any<long>(), Arg.Any<int>()).Returns(x => new bx_submit_info());
            _quoteResultRepository.GetQuoteResultByBuid(Arg.Any<long>(), Arg.Any<int>()).Returns(x => new bx_quoteresult());
            _saveQuoteRepository.GetSavequoteByBuid(Arg.Any<long>()).Returns(x=>null);

            UpdateBjdCheck updateBjdCheck = new UpdateBjdCheck(_saveQuoteRepository, _submitInfoRepository,
                _quoteResultRepository, _userInfoRepository);
            #endregion

            #region 操作  Arg

            bx_submit_info submitInfo = new bx_submit_info();
            bx_quoteresult quoteresult = new bx_quoteresult();
            bx_savequote savequote = new bx_savequote();
            bx_userinfo userinfo = new bx_userinfo();
            var result = updateBjdCheck.Valid(new CreateOrUpdateBjdInfoRequest());

            #endregion

            #region 断言  Assert

            Assert.AreEqual(0, result.State);

            #endregion
        }

        [Test]
        public void Valid_userinfoIsNull_Return0()
        {
            #region 配置  Substitute
            ISaveQuoteRepository _saveQuoteRepository = Substitute.For<ISaveQuoteRepository>();
            ISubmitInfoRepository _submitInfoRepository = Substitute.For<ISubmitInfoRepository>();
            IQuoteResultRepository _quoteResultRepository = Substitute.For<IQuoteResultRepository>();
            IUserInfoRepository _userInfoRepository = Substitute.For<IUserInfoRepository>();

            UpdateBjdCheck updateBjdCheck = new UpdateBjdCheck(_saveQuoteRepository, _submitInfoRepository,
                _quoteResultRepository, _userInfoRepository);
            #endregion

            #region 操作  Arg

            bx_submit_info submitInfo = new bx_submit_info();
            bx_quoteresult quoteresult = new bx_quoteresult();
            bx_savequote savequote = new bx_savequote();
            bx_userinfo userinfo = new bx_userinfo();
            var result = updateBjdCheck.Valid(new CreateOrUpdateBjdInfoRequest());

            #endregion

            #region 断言  Assert

            Assert.AreEqual(0, result.State);

            #endregion
        }

        [Test]
        public void Valid_AllNotNull_Return1()
        {
            #region 配置  Substitute
            ISaveQuoteRepository _saveQuoteRepository = Substitute.For<ISaveQuoteRepository>();
            ISubmitInfoRepository _submitInfoRepository = Substitute.For<ISubmitInfoRepository>();
            IQuoteResultRepository _quoteResultRepository = Substitute.For<IQuoteResultRepository>();
            IUserInfoRepository _userInfoRepository = Substitute.For<IUserInfoRepository>();

            _submitInfoRepository.GetSubmitInfo(Arg.Any<long>(), Arg.Any<int>()).Returns(x => new bx_submit_info());
            _quoteResultRepository.GetQuoteResultByBuid(Arg.Any<long>(),Arg.Any<int>()).Returns(x => new bx_quoteresult());
            _saveQuoteRepository.GetSavequoteByBuid(Arg.Any<long>()).Returns(x => new bx_savequote());
            _userInfoRepository.FindByBuid(Arg.Any<long>()).Returns(x => new bx_userinfo());

            UpdateBjdCheck updateBjdCheck = new UpdateBjdCheck(_saveQuoteRepository, _submitInfoRepository,
                _quoteResultRepository, _userInfoRepository);
            #endregion

            #region 操作  Arg

            bx_submit_info submitInfo = new bx_submit_info();
            bx_quoteresult quoteresult = new bx_quoteresult();
            bx_savequote savequote = new bx_savequote();
            bx_userinfo userinfo = new bx_userinfo();
            var result = updateBjdCheck.Valid(new CreateOrUpdateBjdInfoRequest());

            #endregion

            #region 断言  Assert

            Assert.AreEqual(1, result.State);

            #endregion
        }

        [Test]
        public void Valid_ThrowException_ReturnNegative()
        {
            #region 配置  Substitute
            ISaveQuoteRepository _saveQuoteRepository = Substitute.For<ISaveQuoteRepository>();
            ISubmitInfoRepository _submitInfoRepository = Substitute.For<ISubmitInfoRepository>();
            IQuoteResultRepository _quoteResultRepository = Substitute.For<IQuoteResultRepository>();
            IUserInfoRepository _userInfoRepository = Substitute.For<IUserInfoRepository>();

            _submitInfoRepository.When(x => x.GetSubmitInfo(Arg.Any<long>(), Arg.Any<int>())).Do(info=>
            {
                throw  new Exception("");
            });
            _quoteResultRepository.GetQuoteResultByBuid(Arg.Any<long>(), Arg.Any<int>()).Returns(x => new bx_quoteresult());
            _saveQuoteRepository.GetSavequoteByBuid(Arg.Any<long>()).Returns(x => new bx_savequote());
            _submitInfoRepository.GetSubmitInfo(Arg.Any<long>(), Arg.Any<int>()).Returns(x => null);

            UpdateBjdCheck updateBjdCheck = new UpdateBjdCheck(_saveQuoteRepository, _submitInfoRepository,
                _quoteResultRepository, _userInfoRepository);
            #endregion

            #region 操作  Arg

            bx_submit_info submitInfo = new bx_submit_info();
            bx_quoteresult quoteresult = new bx_quoteresult();
            bx_savequote savequote = new bx_savequote();
            bx_userinfo userinfo = new bx_userinfo();
            var result = updateBjdCheck.Valid(new CreateOrUpdateBjdInfoRequest());

            #endregion

            #region 断言  Assert

            Assert.AreEqual(-1, result.State);

            #endregion
        }
    }
}
