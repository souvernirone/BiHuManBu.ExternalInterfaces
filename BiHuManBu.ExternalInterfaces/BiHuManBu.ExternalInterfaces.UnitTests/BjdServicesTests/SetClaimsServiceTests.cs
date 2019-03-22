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
using System;

namespace BiHuManBu.ExternalInterfaces.UnitTests.BjdServicesTests
{
    [TestFixture]
    public class SetClaimsServiceTests
    {
        private static IUserClaimRepository _userClaimRepository = Substitute.For<IUserClaimRepository>();
        private static SetClaimsService _setClaimsService = new SetClaimsService(_userClaimRepository);
        [Test]
        public void SetClaims_ClaimDetailEmpty_IsNewClaims1()
        {
            _userClaimRepository.FindList(Arg.Any<long>()).Returns(x => new List<bx_claim_detail>());
            var result = _setClaimsService.SetClaims(new MyBaoJiaViewModel(), new GetMyBjdDetailRequest() { IsNewClaim = 1 }, new bx_userinfo() { Id = 1 });
            Assert.AreEqual(null, result.ClaimItem.FirstOrDefault());
            Assert.AreEqual(null, result.ClaimDetail.FirstOrDefault());
            Assert.AreEqual(0, result.ClaimCount);
        }
        [Test]
        public void SetClaims_ClaimDetailEmpty_IsNewClaims0()
        {
            _userClaimRepository.FindList(Arg.Any<long>()).Returns(x => new List<bx_claim_detail>());
            var result = _setClaimsService.SetClaims(new MyBaoJiaViewModel(), new GetMyBjdDetailRequest() { IsNewClaim = 0 }, new bx_userinfo() { Id = 1 });
            Assert.AreEqual(null, result.ClaimItem.FirstOrDefault());
            Assert.AreEqual(null, result.ClaimDetail.FirstOrDefault());
            Assert.AreEqual(0, result.ClaimCount);
        }
        [Test]
        public void SetClaims_ClaimDetailNotEmpty_IsNewClaims0()
        {
            _userClaimRepository.FindList(Arg.Any<long>()).Returns(x => new List<bx_claim_detail>() { new bx_claim_detail() { endcase_time = new DateTime(2018, 1, 1), loss_time = new DateTime(2017, 1, 1), pay_amount = 1.22, pay_company_name = "pay_company_name", pay_type = 1 } });
            var result = _setClaimsService.SetClaims(new MyBaoJiaViewModel(), new GetMyBjdDetailRequest() { IsNewClaim = 0 }, new bx_userinfo() { Id = 1 });
            Assert.AreEqual(new DateTime(2018, 1, 1), result.ClaimDetail.FirstOrDefault().EndCaseTime);
            Assert.AreEqual(new DateTime(2017, 1, 1), result.ClaimDetail.FirstOrDefault().LossTime);
            Assert.AreEqual(1.22, result.ClaimDetail.FirstOrDefault().PayAmount);
            Assert.AreEqual("pay_company_name", result.ClaimDetail.FirstOrDefault().PayCompanyName);
            Assert.AreEqual(null, result.ClaimItem.FirstOrDefault());
            Assert.AreEqual(1, result.ClaimCount);
        }
        [Test]
        public void SetClaims_ClaimDetailNotEmpty_IsNewClaims1()
        {
            _userClaimRepository.FindList(Arg.Any<long>()).Returns(x => new List<bx_claim_detail>() { new bx_claim_detail() { endcase_time = new DateTime(2018, 1, 1), loss_time = new DateTime(2017, 1, 1), pay_amount = 1.22, pay_company_name = "pay_company_name", pay_type = 1 } });
            var result = _setClaimsService.SetClaims(new MyBaoJiaViewModel(), new GetMyBjdDetailRequest() { IsNewClaim = 1 }, new bx_userinfo() { Id = 1 });

            Assert.AreEqual("2017-01-01", result.ClaimItem.FirstOrDefault().LossTime);
            Assert.AreEqual(1.22, result.ClaimItem.FirstOrDefault().PayAmount);
            Assert.AreEqual("pay_company_name", result.ClaimItem.FirstOrDefault().PayCompanyName);
            Assert.AreEqual(null, result.ClaimDetail.FirstOrDefault());
            Assert.AreEqual(1, result.ClaimCount);
        }
    }
}
