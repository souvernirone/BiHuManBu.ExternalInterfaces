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
            IAgentRepository _agentRepository = Substitute.For<IAgentRepository>();
            IAddCrmStepsService _addCrmStepsService = Substitute.For<IAddCrmStepsService>();

            var request = new CreateOrUpdateBjdInfoRequest();

            _bjdCheck.Valid(request)
                .Returns(
                    x =>
                        new UpdateBjdCheckMessage()
                        {
                            State = 0,
                            SubmitInfo = new bx_submit_info(),
                            Quoteresult = new bx_quoteresult(),
                            Savequote = new bx_savequote(),
                            Userinfo = new bx_userinfo()
                        });

            CreateBjdInfoService service = new CreateBjdInfoService(_createActivity, _mapBaoDanXinXiRecord, _mapBaoDanXianZhongRecord, _bjxUnionRepository, _baodanXianZhongRepository, _baodanxinxiRepository, _bjdCheck, _agentRepository, _addCrmStepsService);
            #endregion

            #region 操作  Arg
            var result = service.UpdateBjdInfo(request, null);
            #endregion

            #region 断言  Assert
            Assert.AreEqual(0, result);
            #endregion
            

        }

        [Test]
        public void UpdateBjdInfo_ThrowExecption_ReturnNegavite()
        {

            #region 配置  Substitute
            IBaodanXianZhongRepository _baodanXianZhongRepository = Substitute.For<IBaodanXianZhongRepository>();
            IBaodanxinxiRepository _baodanxinxiRepository = Substitute.For<IBaodanxinxiRepository>();
            IBxBjUnionRepository _bjxUnionRepository = Substitute.For<IBxBjUnionRepository>();
            ICreateActivity _createActivity = Substitute.For<ICreateActivity>();
            IMapBaoDanXinXiRecord _mapBaoDanXinXiRecord = Substitute.For<IMapBaoDanXinXiRecord>();
            IMapBaoDanXianZhongRecord _mapBaoDanXianZhongRecord = Substitute.For<IMapBaoDanXianZhongRecord>();
            IUpdateBjdCheck _bjdCheck = Substitute.For<IUpdateBjdCheck>();
            IAgentRepository _agentRepository = Substitute.For<IAgentRepository>();
            IAddCrmStepsService _addCrmStepsService = Substitute.For<IAddCrmStepsService>();

            var request = new CreateOrUpdateBjdInfoRequest();
            int type = Arg.Any<int>();

            _bjdCheck.Valid(request)
                .Returns(
                    x =>
                        new UpdateBjdCheckMessage()
                        {
                            State = 1,
                            SubmitInfo = new bx_submit_info(),
                            Quoteresult = new bx_quoteresult(),
                            Savequote = new bx_savequote(),
                            Userinfo = new bx_userinfo()
                        });

            _createActivity.When(x => x.AddActivity(request, type))
                .Do(info => { throw new Exception(); });

            CreateBjdInfoService service = new CreateBjdInfoService(_createActivity, _mapBaoDanXinXiRecord, _mapBaoDanXianZhongRecord, _bjxUnionRepository, _baodanXianZhongRepository, _baodanxinxiRepository, _bjdCheck, _agentRepository, _addCrmStepsService);
            #endregion

            #region 操作  Arg
            var result = service.UpdateBjdInfo(request, null);
            #endregion

            #region 断言  Assert
            Assert.AreEqual(-1, result);
            #endregion


        }
    }
}
