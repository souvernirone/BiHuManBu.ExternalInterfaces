using System;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.ReportModel;
using BiHuManBu.ExternalInterfaces.Services.BjdServices.Extends;
using NSubstitute;
using NUnit.Framework;

namespace BiHuManBu.ExternalInterfaces.UnitTests.SpecialoptionServiceTests
{
    [TestFixture]
    public class GetBjdCheckTests
    {
        [Test]
        public void BjdCheckMessage_BaoXianXinInIsNull_Return0()
        {
            #region  配置  Substitute

            IBaodanxinxiRepository _baodanxinxiRepository = Substitute.For<IBaodanxinxiRepository>();
            IBaodanXianZhongRepository _baodanXianZhongRepository = Substitute.For<IBaodanXianZhongRepository>();

            _baodanxinxiRepository.Finds(Arg.Any<long>())
                .Returns(
                    info =>
                        new BjdCorrelateViewModel()
                        {
                            Baodanxinxi = null,
                            Baodanxianzhong = new bj_baodanxianzhong()
                        });

            var bjdCheck = new GetBjdCheck(_baodanXianZhongRepository, _baodanxinxiRepository);

            #endregion

            #region 操作  Arg

            var result = bjdCheck.BjdCheckMessage(1);

            #endregion

            #region 断言  Assert

            Assert.AreEqual(0, result.State);

            #endregion
        }

        [Test]
        public void BjdCheckMessage_BaoXianXianZhongIsNull_Return0()
        {
            #region  配置  Substitute

            IBaodanxinxiRepository _baodanxinxiRepository = Substitute.For<IBaodanxinxiRepository>();
            IBaodanXianZhongRepository _baodanXianZhongRepository = Substitute.For<IBaodanXianZhongRepository>();

            _baodanxinxiRepository.Finds(Arg.Any<long>())
                .Returns(
                    info =>
                        new BjdCorrelateViewModel()
                        {
                            Baodanxinxi = new bj_baodanxinxi(),
                            Baodanxianzhong = null
                        });

            var bjdCheck = new GetBjdCheck(_baodanXianZhongRepository, _baodanxinxiRepository);

            #endregion

            #region 操作  Arg

            var result = bjdCheck.BjdCheckMessage(1);

            #endregion

            #region 断言  Assert

            Assert.AreEqual(0, result.State);

            #endregion
        }

        [Test]
        public void BjdCheckMessage_ThrowExecption_ReturnNegavite()
        {
            #region  配置  Substitute

            IBaodanxinxiRepository _baodanxinxiRepository = Substitute.For<IBaodanxinxiRepository>();
            IBaodanXianZhongRepository _baodanXianZhongRepository = Substitute.For<IBaodanXianZhongRepository>();

            _baodanxinxiRepository.When(x => x.Finds(Arg.Any<long>())).Do(x => { throw new Exception(); });

            var bjdCheck = new GetBjdCheck(_baodanXianZhongRepository, _baodanxinxiRepository);

            #endregion

            #region 操作  Arg

            var result = bjdCheck.BjdCheckMessage(1);

            #endregion

            #region 断言  Assert

            Assert.AreEqual(-1, result.State);

            #endregion
        }

        [Test]
        public void BjdCheckMessage_AllNotNull_Return1()
        {
            #region  配置  Substitute

            IBaodanxinxiRepository _baodanxinxiRepository = Substitute.For<IBaodanxinxiRepository>();
            IBaodanXianZhongRepository _baodanXianZhongRepository = Substitute.For<IBaodanXianZhongRepository>();

            _baodanxinxiRepository.Finds(Arg.Any<long>())
                .Returns(
                    info =>
                        new BjdCorrelateViewModel()
                        {
                            Baodanxinxi = new bj_baodanxinxi(),
                            Baodanxianzhong = new bj_baodanxianzhong()
                        });

            var bjdCheck = new GetBjdCheck(_baodanXianZhongRepository, _baodanxinxiRepository);

            #endregion

            #region 操作  Arg

            var result = bjdCheck.BjdCheckMessage(1);

            #endregion

            #region 断言  Assert

            Assert.AreEqual(1, result.State);

            #endregion
        }
    }
}
