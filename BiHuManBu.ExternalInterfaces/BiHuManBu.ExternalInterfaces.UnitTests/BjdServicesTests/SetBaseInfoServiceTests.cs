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
    public class SetBaseInfoServiceTests
    {
        SetBaseInfoService setBaseInfoService = new SetBaseInfoService();
        [Test]
        public void SetBaseInfo_IsSingleSubmitNotNull()
        {
            var userInfo = new bx_userinfo()
            {
                HolderName = "刘松年",
                HolderIdType = 1,
                HolderEmail = "songnian@123.com",
                OwnerCertiAddress = "ownerCertiAddress",
                OwnerNation = "中国",
                HolderAddress = "HolderAddress",
                CarVIN = "132456",
                SixDigitsAfterIdCard = "1234516",
                MoldName = "MoldName",
                LicenseOwner = "LicenseOwner",
                Id = 1,
                InsuredName = "InsuredName",
                CityCode = "CityCode",
                IsSingleSubmit = 1,
                UpdateTime = DateTime.Now
            };
            var result = setBaseInfoService.SetBaseInfo(new MyBaoJiaViewModel() { }, userInfo);
            Assert.AreEqual("刘松年", result.Holder.HolderName);
            Assert.AreEqual(1, result.Holder.HolderIdType);
            Assert.AreEqual("songnian@123.com", result.Holder.HolderEmail);
            Assert.AreEqual("ownerCertiAddress", result.CarOwnerPerson.Address);
            Assert.AreEqual("中国", result.CarOwnerPerson.Nation);
            Assert.AreEqual("HolderAddress", result.HolderPerson.Address);
            Assert.AreEqual("132456", result.CarVin);
            Assert.AreEqual("1234516", result.SixDigitsAfterIdCard);
            Assert.AreEqual("MoldName", result.MoldName);
            Assert.AreEqual("LicenseOwner", result.LicenseOwner);
            Assert.AreEqual(1, result.Buid);
            Assert.AreEqual("InsuredName", result.InsuredName);
            Assert.AreEqual("CityCode", result.CityCode);
            Assert.AreEqual(1, result.QuoteGroup);
            Assert.AreEqual(1, result.HasBaojia);
        }
        [Test]
        public void SetBaseInfo_IsSingleSubmitNull()
        {
            var userInfo = new bx_userinfo()
            {
                HolderName = "刘松年",
                HolderIdType = 1,
                HolderEmail = "songnian@123.com",
                OwnerCertiAddress = "ownerCertiAddress",
                OwnerNation = "中国",
                HolderAddress = "HolderAddress",
                CarVIN = "132456",
                SixDigitsAfterIdCard = "1234516",
                MoldName = "MoldName",
                LicenseOwner = "LicenseOwner",
                Id = 1,
                InsuredName = "InsuredName",
                CityCode = "CityCode",
            };
            var result = setBaseInfoService.SetBaseInfo(new MyBaoJiaViewModel() { }, userInfo);
            Assert.AreEqual("刘松年", result.Holder.HolderName);
            Assert.AreEqual(1, result.Holder.HolderIdType);
            Assert.AreEqual("songnian@123.com", result.Holder.HolderEmail);
            Assert.AreEqual("ownerCertiAddress", result.CarOwnerPerson.Address);
            Assert.AreEqual("中国", result.CarOwnerPerson.Nation);
            Assert.AreEqual("HolderAddress", result.HolderPerson.Address);
            Assert.AreEqual("132456", result.CarVin);
            Assert.AreEqual("1234516", result.SixDigitsAfterIdCard);
            Assert.AreEqual("MoldName", result.MoldName);
            Assert.AreEqual("LicenseOwner", result.LicenseOwner);
            Assert.AreEqual(1, result.Buid);
            Assert.AreEqual("InsuredName", result.InsuredName);
            Assert.AreEqual("CityCode", result.CityCode);
            Assert.AreEqual(0, result.QuoteGroup);
            Assert.AreEqual(0, result.HasBaojia);
        }
    }
}
