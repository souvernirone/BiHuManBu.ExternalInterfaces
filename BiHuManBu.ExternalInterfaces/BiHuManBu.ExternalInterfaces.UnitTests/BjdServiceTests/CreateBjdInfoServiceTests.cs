
using System;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.BjdServices.Achieves;
using BiHuManBu.ExternalInterfaces.Services.BjdServices.Extends;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using NSubstitute;
using NUnit.Framework;

namespace BiHuManBu.ExternalInterfaces.UnitTests
{
    [TestFixture]
    public class CreateBjdInfoServiceTests
    {
        [Test]
        public void UpdateBjdInfo_CheckReturn0_Return0()
        {

            #region 配置  Substitute
            IBaodanXianZhongRepository _baodanXianZhongRepository = Substitute.For<IBaodanXianZhongRepository>();
            IBaodanxinxiRepository _baodanxinxiRepository = Substitute.For<IBaodanxinxiRepository>();
            IBxBjUnionRepository _bjxUnionRepository = Substitute.For<IBxBjUnionRepository>();
            ICreateActivity _createActivity = Substitute.For<ICreateActivity>();
            IMapBaoDanXinXiRecord _mapBaoDanXinXiRecord = Substitute.For<IMapBaoDanXinXiRecord>();
            IMapBaoDanXianZhongRecord _mapBaoDanXianZhongRecord = Substitute.For<IMapBaoDanXianZhongRecord>();
            IUpdateBjdCheck _bjdCheck = Substitute.For<IUpdateBjdCheck>();

            var request = new CreateOrUpdateBjdInfoRequest();

            _bjdCheck.Valid(request).Returns(x => new UpdateBjdCheckMessage() {State = 0});

            CreateBjdInfoService service = new CreateBjdInfoService(_createActivity,_mapBaoDanXinXiRecord,_mapBaoDanXianZhongRecord,_bjxUnionRepository,_baodanXianZhongRepository,_baodanxinxiRepository,_bjdCheck);
            #endregion

            #region 操作  Arg
            var result = service.UpdateBjdInfo(request, null);
            #endregion

            #region 断言  Assert
            Assert.AreEqual(0, result);
            #endregion
            

        }

        public void UpdateBjdInfo_BaoDanXinXiIsNull_Return0()
        {

            #region 配置  Substitute
            IBaodanXianZhongRepository _baodanXianZhongRepository = Substitute.For<IBaodanXianZhongRepository>();
            IBaodanxinxiRepository _baodanxinxiRepository = Substitute.For<IBaodanxinxiRepository>();
            IBxBjUnionRepository _bjxUnionRepository = Substitute.For<IBxBjUnionRepository>();
            ICreateActivity _createActivity = Substitute.For<ICreateActivity>();
            IMapBaoDanXinXiRecord _mapBaoDanXinXiRecord = Substitute.For<IMapBaoDanXinXiRecord>();
            IMapBaoDanXianZhongRecord _mapBaoDanXianZhongRecord = Substitute.For<IMapBaoDanXianZhongRecord>();
            IUpdateBjdCheck _bjdCheck = Substitute.For<IUpdateBjdCheck>();

            var request = new CreateOrUpdateBjdInfoRequest();
            int type = Arg.Any<int>();

            _bjdCheck.Valid(request).Returns(x => new UpdateBjdCheckMessage() { State = 0 });
            _createActivity.AddActivity(Arg.Any<CreateOrUpdateBjdInfoRequest>(), type)
                .Returns(x => new bx_preferential_activity());
            _baodanxinxiRepository.Add(Arg.Any<bj_baodanxinxi>()).Returns(x => null);

            CreateBjdInfoService service = new CreateBjdInfoService(_createActivity, _mapBaoDanXinXiRecord, _mapBaoDanXianZhongRecord, _bjxUnionRepository, _baodanXianZhongRepository, _baodanxinxiRepository, _bjdCheck);
            #endregion

            #region 操作  Arg
            var result = service.UpdateBjdInfo(request, null);
            #endregion

            #region 断言  Assert
            Assert.AreEqual(0, result);
            #endregion


        }

        [Test]
        public void UpdateBjdInfo_ThrowExecption_Return0()
        {

            #region 配置  Substitute
            IBaodanXianZhongRepository _baodanXianZhongRepository = Substitute.For<IBaodanXianZhongRepository>();
            IBaodanxinxiRepository _baodanxinxiRepository = Substitute.For<IBaodanxinxiRepository>();
            IBxBjUnionRepository _bjxUnionRepository = Substitute.For<IBxBjUnionRepository>();
            ICreateActivity _createActivity = Substitute.For<ICreateActivity>();
            IMapBaoDanXinXiRecord _mapBaoDanXinXiRecord = Substitute.For<IMapBaoDanXinXiRecord>();
            IMapBaoDanXianZhongRecord _mapBaoDanXianZhongRecord = Substitute.For<IMapBaoDanXianZhongRecord>();
            IUpdateBjdCheck _bjdCheck = Substitute.For<IUpdateBjdCheck>();

            _bjdCheck.Valid(new CreateOrUpdateBjdInfoRequest()).Returns(x => new UpdateBjdCheckMessage() { State = 1 });

            _createActivity.When(x => x.AddActivity(new CreateOrUpdateBjdInfoRequest(), Arg.Any<int>()))
                .Do(info => { throw new Exception(); });

            CreateBjdInfoService service = new CreateBjdInfoService(_createActivity, _mapBaoDanXinXiRecord, _mapBaoDanXianZhongRecord, _bjxUnionRepository, _baodanXianZhongRepository, _baodanxinxiRepository, _bjdCheck);
            #endregion

            #region 操作  Arg
            var result = service.UpdateBjdInfo(Arg.Any<CreateOrUpdateBjdInfoRequest>(), null);
            #endregion

            #region 断言  Assert
            Assert.AreEqual(0, result);
            #endregion


        }
    }
}
