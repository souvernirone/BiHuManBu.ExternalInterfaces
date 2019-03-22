using System;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.BjdServices.Achieves;
using BiHuManBu.ExternalInterfaces.Services.BjdServices.Extends;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using NSubstitute;
using NUnit.Framework;

namespace BiHuManBu.ExternalInterfaces.UnitTests.SpecialoptionServiceTests
{
    [TestFixture]
    public class GetBjdInfoTests
    {
        [Test]
        public void GetBjdInfo_ThrowExecption_ReturnNegavite()
        {
            #region 配置 Substitute

            IBaodanxinxiRepository _baodanxinxiRepository = Substitute.For<IBaodanxinxiRepository>();
            IBaodanXianZhongRepository _baodanXianZhongRepository = Substitute.For<IBaodanXianZhongRepository>();
            IPreferentialActivityRepository _preferentialActivityRepository = Substitute.For<IPreferentialActivityRepository>();
            IBxBjUnionRepository _bxBjUnionRepository = Substitute.For<IBxBjUnionRepository>();
            IUserClaimRepository _userClaimRepository = Substitute.For<IUserClaimRepository>();
            ISaveQuoteRepository _saveQuoteRepository = Substitute.For<ISaveQuoteRepository>();
            IMapBjdInfoRecord _mapBjdInfoRecord = Substitute.For<IMapBjdInfoRecord>();
            IGetBjdCheck _getBjdCheck = Substitute.For<IGetBjdCheck>();

            var bjdItemRequest = Arg.Any<GetBjdItemRequest>();

            _baodanxinxiRepository.When(x=>x.Find(Arg.Any<int>())).Do(info =>
            {
                throw new Exception();
            });

            GetBjdInfoService getBjdInfoService = new GetBjdInfoService(_baodanxinxiRepository,_baodanXianZhongRepository,_preferentialActivityRepository,
                _bxBjUnionRepository, _userClaimRepository, _saveQuoteRepository, _mapBjdInfoRecord, _getBjdCheck);

            #endregion

            #region 操作 Arg

            var result = getBjdInfoService.GetBjdInfo(bjdItemRequest,null);

            #endregion

            #region 断言 Assert

            Assert.AreEqual(-1, result.BusinessStatus);

            #endregion
        }
    }
}
