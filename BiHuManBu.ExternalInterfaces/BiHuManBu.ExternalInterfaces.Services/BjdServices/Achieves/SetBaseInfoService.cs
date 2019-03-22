using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.BjdServices.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using System;

namespace BiHuManBu.ExternalInterfaces.Services.BjdServices.Achieves
{
    public class SetBaseInfoService : ISetBaseInfoService
    {
        public MyBaoJiaViewModel SetBaseInfo(MyBaoJiaViewModel my, bx_userinfo userInfo)
        {
            my.Holder = new Holder
            {
                HolderName = userInfo.HolderName ?? string.Empty,
                HolderIdType = userInfo.HolderIdType.HasValue ? userInfo.HolderIdType.Value : 0,
                HolderIdCard = userInfo.HolderIdCard ?? string.Empty,
                HolderMobile = userInfo.HolderMobile ?? string.Empty,
                HolderAddress = userInfo.HolderAddress ?? string.Empty,
                HolderEmail = userInfo.HolderEmail ?? string.Empty
            };
            my.CarOwnerPerson = new RelationPerson
            {
                Address = userInfo.OwnerCertiAddress ?? string.Empty,
                Email = userInfo.Email ?? string.Empty,
                IdCard = userInfo.IdCard ?? string.Empty,
                IdType = userInfo.OwnerIdCardType,
                Mobile = userInfo.Mobile ?? string.Empty,
                Name = userInfo.LicenseOwner ?? string.Empty,
                Sex = userInfo.OwnerSex ?? 1,
                Authority = userInfo.OwnerIssuer ?? string.Empty,
                Birthday = string.IsNullOrEmpty(userInfo.OwnerBirthday) ? "" : Convert.ToDateTime(userInfo.OwnerBirthday).Date == DateTime.MinValue.Date ? "" : userInfo.OwnerBirthday,
                CertiEndDate = string.IsNullOrEmpty(userInfo.OwnerCertiEnddate) ? "" : Convert.ToDateTime(userInfo.OwnerCertiEnddate).Date == DateTime.MinValue.Date ? "" : userInfo.OwnerCertiEnddate,
                CertiStartDate = string.IsNullOrEmpty(userInfo.OwnerCertiStartdate) ? "" : Convert.ToDateTime(userInfo.OwnerCertiStartdate).Date == DateTime.MinValue.Date ? "" : userInfo.OwnerCertiStartdate,
                Nation = userInfo.OwnerNation ?? string.Empty
            };
            my.HolderPerson = new RelationPerson()
            {
                Address = userInfo.HolderAddress ?? string.Empty,
                Email = string.IsNullOrWhiteSpace(userInfo.HolderEmail) ? my.CarOwnerPerson.Email : userInfo.HolderEmail,
                IdCard = userInfo.HolderIdCard ?? string.Empty,
                IdType = userInfo.HolderIdType ?? 0,
                Mobile = userInfo.HolderMobile ?? string.Empty,
                Name = userInfo.HolderName ?? string.Empty,
                Sex = userInfo.HolderSex ?? 1,
                Authority = userInfo.HolderIssuer ?? string.Empty,
                Birthday = string.IsNullOrEmpty(userInfo.HolderBirthday) ? "" : Convert.ToDateTime(userInfo.HolderBirthday).Date == DateTime.MinValue.Date ? "" : userInfo.HolderBirthday,
                CertiEndDate = string.IsNullOrEmpty(userInfo.HolderCertiEnddate) ? "" : Convert.ToDateTime(userInfo.HolderCertiEnddate).Date == DateTime.MinValue.Date ? "" : userInfo.HolderCertiEnddate,
                CertiStartDate = string.IsNullOrEmpty(userInfo.HolderCertiStartdate) ? "" : Convert.ToDateTime(userInfo.HolderCertiStartdate).Date == DateTime.MinValue.Date ? "" : userInfo.HolderCertiStartdate,
                Nation = userInfo.HolderNation ?? string.Empty
            };
            my.InsuredPerson = new RelationPerson
            {
                Address = userInfo.InsuredAddress ?? string.Empty,
                Email = string.IsNullOrWhiteSpace(userInfo.InsuredEmail) ? my.CarOwnerPerson.Email : userInfo.InsuredEmail,
                IdCard = userInfo.InsuredIdCard ?? string.Empty,
                IdType = userInfo.InsuredIdType ?? 0,
                Mobile = userInfo.InsuredMobile ?? string.Empty,
                Name = userInfo.InsuredName ?? string.Empty,
                Sex = userInfo.InsuredSex ?? 1,
                Authority = userInfo.InsuredIssuer ?? string.Empty,
                Birthday = string.IsNullOrEmpty(userInfo.InsuredBirthday) ? "" : Convert.ToDateTime(userInfo.InsuredBirthday).Date == DateTime.MinValue.Date ? "" : userInfo.InsuredBirthday,
                CertiEndDate = string.IsNullOrEmpty(userInfo.InsuredCertiEnddate) ? "" : Convert.ToDateTime(userInfo.InsuredCertiEnddate).Date == DateTime.MinValue.Date ? "" : userInfo.InsuredCertiEnddate,
                CertiStartDate = string.IsNullOrEmpty(userInfo.InsuredCertiStartdate) ? "" : Convert.ToDateTime(userInfo.InsuredCertiStartdate).Date == DateTime.MinValue.Date ? "" : userInfo.InsuredCertiStartdate,
                Nation = userInfo.InsuredNation ?? string.Empty
            };
            my.SubmitGroup = userInfo.Source.HasValue ? (userInfo.Source.Value > 0 ? userInfo.Source.Value : 0) : 0;//核保类型
            my.CurOpenId = userInfo.OpenId;
            my.CurAgent = userInfo.Agent;
            my.MoldName = !string.IsNullOrEmpty(userInfo.MoldName) ? userInfo.MoldName : string.Empty;
            my.UserName = !string.IsNullOrEmpty(userInfo.UserName) ? userInfo.UserName : string.Empty;
            my.LicenseOwner = !string.IsNullOrEmpty(userInfo.LicenseOwner) ? userInfo.LicenseOwner : string.Empty;
            my.Buid = userInfo.Id;
            my.RegisterDate = !string.IsNullOrEmpty(userInfo.RegisterDate) ? userInfo.RegisterDate : string.Empty;
            //InsuredName和PostedName是一个字段,被保险人姓名
            my.InsuredName = !string.IsNullOrEmpty(userInfo.InsuredName) ? userInfo.InsuredName : string.Empty;
            my.PostedName = !string.IsNullOrEmpty(userInfo.InsuredName) ? userInfo.InsuredName : string.Empty;
            //被保险人电话
            my.InsuredMobile = !string.IsNullOrEmpty(userInfo.InsuredMobile) ? userInfo.InsuredMobile : string.Empty;
            //被保险人地址
            my.InsuredAddress = !string.IsNullOrEmpty(userInfo.InsuredAddress) ? userInfo.InsuredAddress : string.Empty;
            //被保险人证件类型
            my.InsuredIdType = userInfo.InsuredIdType.HasValue ? userInfo.InsuredIdType.Value : 0;
            //车主证件类型
            my.IdType = userInfo.OwnerIdCardType;//_quoteResultCarinfoRepository.GetOwnerIdType(userInfo.Id);
            my.IdCard = !string.IsNullOrEmpty(userInfo.IdCard) ? userInfo.IdCard : string.Empty;
            //my.IdType = userInfo.InsuredIdType.HasValue ? userInfo.InsuredIdType.Value : 0;
            my.CredentislasNum = !string.IsNullOrEmpty(userInfo.IdCard) ? userInfo.IdCard : string.Empty;
            my.InsuredIdCard = !string.IsNullOrEmpty(userInfo.InsuredIdCard) ? userInfo.InsuredIdCard : string.Empty;
            my.CityCode = !string.IsNullOrEmpty(userInfo.CityCode) ? userInfo.CityCode : string.Empty;
            my.EngineNo = !string.IsNullOrEmpty(userInfo.EngineNo) ? userInfo.EngineNo : string.Empty;
            my.CarVin = !string.IsNullOrEmpty(userInfo.CarVIN) ? userInfo.CarVIN : string.Empty;
            //获取电子保单的邮箱地址
            //my.Email = !string.IsNullOrEmpty(userInfo.Email) ? userInfo.Email : string.Empty;
            my.Email = !string.IsNullOrEmpty(userInfo.InsuredEmail) ? userInfo.InsuredEmail : string.Empty;
            my.Email = !string.IsNullOrEmpty(my.Email) ? my.Email : userInfo.Email;
            my.QuoteGroup = userInfo.IsSingleSubmit.HasValue ? userInfo.IsSingleSubmit.Value : 0;//报价类型
            my.HasBaojia = my.QuoteGroup == 0 ? 0 : 1;
            if (my.QuoteGroup > 0)//(userinfo.QuoteStatus > -1)
            {
                my.HasBaojia = 1; //是否报过价
                my.QuoteTime = userInfo.UpdateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                my.HasBaojia = 0;
                my.QuoteTime = "";
            }
            if (string.IsNullOrWhiteSpace(my.QuoteTime))
            {
                my.IsTheDay = 2;
            }
            else
            {
                my.IsTheDay = Convert.ToDateTime(my.QuoteTime).ToShortDateString() == DateTime.Now.ToShortDateString() ? 1 : 2;
            }
            my.SixDigitsAfterIdCard = userInfo.SixDigitsAfterIdCard ?? "";
            return my;
        }
    }
}
