using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.BjdServices.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using System;

namespace BiHuManBu.ExternalInterfaces.Services.BjdServices.Extends
{
    public class SetBaseInfoHistoryService: ISetBaseInfoHistoryService
    {
        /// <summary>
        /// 百得利从报价历史获取关系人信息
        /// </summary>
        /// <param name="my"></param>
        /// <param name="userInfo"></param>
        /// <param name="relatedhistory"></param>
        /// <returns></returns>
        public MyBaoJiaViewModel SetBaseInfoHistory(MyBaoJiaViewModel my, bx_userinfo userInfo,bx_quotehistory_related relatedhistory)
        {
            my.Holder = new Holder
            {
                HolderName = relatedhistory.holder_name ?? string.Empty,
                HolderIdType = relatedhistory.holder_id_type.HasValue ? relatedhistory.holder_id_type.Value : 0,
                HolderIdCard = relatedhistory.holder_id_card ?? string.Empty,
                HolderMobile = relatedhistory.holder_mobile ?? string.Empty,
                HolderAddress = relatedhistory.holder_address ?? string.Empty,
                HolderEmail = relatedhistory.holder_email ?? string.Empty
            };
            my.CarOwnerPerson = new RelationPerson
            {
                Address = relatedhistory.ower_address ?? string.Empty,
                Email = relatedhistory.ower_email ?? string.Empty,
                IdCard = relatedhistory.ower_id_card ?? string.Empty,
                IdType = relatedhistory.ower_id_type??0,
                Mobile = relatedhistory.holder_mobile ?? string.Empty,
                Name = relatedhistory.ower_name ?? string.Empty,
                Sex = relatedhistory.ower_sex ?? 1,
                Authority = relatedhistory.ower_authority ?? string.Empty,
                Birthday = (!relatedhistory.ower_birthday.HasValue) ? "" : Convert.ToDateTime(relatedhistory.ower_birthday.Value).Year == DateTime.MinValue.Year ? "" : relatedhistory.ower_birthday.Value.ToString("yyyy-MM-dd"),
                CertiEndDate = (!relatedhistory.ower_certi_end_date.HasValue) ? "" : Convert.ToDateTime(relatedhistory.ower_certi_end_date).Year == DateTime.MinValue.Year ? "" : relatedhistory.ower_certi_end_date.Value.ToString("yyyy-MM-dd"),
                CertiStartDate = (!relatedhistory.ower_certi_start_date.HasValue) ? "" : Convert.ToDateTime(relatedhistory.ower_certi_start_date).Year == DateTime.MinValue.Year ? "" : relatedhistory.ower_certi_start_date.Value.ToString("yyyy-MM-dd"),
                Nation = relatedhistory.ower_nation ?? string.Empty
            };
            my.HolderPerson = new RelationPerson
            {
                Address = relatedhistory.holder_address ?? string.Empty,
                Email = relatedhistory.holder_email ?? string.Empty,
                IdCard = relatedhistory.holder_id_card ?? string.Empty,
                IdType = relatedhistory.holder_id_type ?? 0,
                Mobile = relatedhistory.holder_mobile ?? string.Empty,
                Name = relatedhistory.holder_name ?? string.Empty,
                Sex = relatedhistory.holder_sex ?? 1,
                Authority = relatedhistory.holder_authority ?? string.Empty,
                Birthday = (!relatedhistory.holder_birthday.HasValue) ? "" : Convert.ToDateTime(relatedhistory.holder_birthday.Value).Year == DateTime.MinValue.Year ? "" : relatedhistory.holder_birthday.Value.ToString("yyyy-MM-dd"),
                CertiEndDate = (!relatedhistory.holder_certi_end_date.HasValue) ? "" : Convert.ToDateTime(relatedhistory.holder_certi_end_date).Year == DateTime.MinValue.Year ? "" : relatedhistory.holder_certi_end_date.Value.ToString("yyyy-MM-dd"),
                CertiStartDate = (!relatedhistory.holder_certi_start_date.HasValue) ? "" : Convert.ToDateTime(relatedhistory.holder_certi_start_date).Year == DateTime.MinValue.Year ? "" : relatedhistory.holder_certi_start_date.Value.ToString("yyyy-MM-dd"),
                Nation = relatedhistory.holder_nation ?? string.Empty
            };
            my.InsuredPerson = new RelationPerson
            {
                Address = relatedhistory.insured_address ?? string.Empty,
                Email = relatedhistory.insured_email ?? string.Empty,
                IdCard = relatedhistory.insured_id_card ?? string.Empty,
                IdType = relatedhistory.insured_id_type ?? 0,
                Mobile = relatedhistory.insured_mobile ?? string.Empty,
                Name = relatedhistory.insured_name ?? string.Empty,
                Sex = relatedhistory.insured_sex ?? 1,
                Authority = relatedhistory.insured_authority ?? string.Empty,
                Birthday = (!relatedhistory.insured_birthday.HasValue) ? "" : Convert.ToDateTime(relatedhistory.insured_birthday.Value).Year == DateTime.MinValue.Year ? "" : relatedhistory.insured_birthday.Value.ToString("yyyy-MM-dd"),
                CertiEndDate = (!relatedhistory.insured_certi_end_date.HasValue) ? "" : Convert.ToDateTime(relatedhistory.insured_certi_end_date).Year == DateTime.MinValue.Year ? "" : relatedhistory.insured_certi_end_date.Value.ToString("yyyy-MM-dd"),
                CertiStartDate = (!relatedhistory.insured_certi_start_date.HasValue) ? "" : Convert.ToDateTime(relatedhistory.insured_certi_start_date).Year == DateTime.MinValue.Year ? "" : relatedhistory.insured_certi_start_date.Value.ToString("yyyy-MM-dd"),
                Nation = relatedhistory.insured_nation ?? string.Empty
            };
            my.SubmitGroup = userInfo.Source.HasValue ? (userInfo.Source.Value > 0 ? userInfo.Source.Value : 0) : 0;//核保类型
            my.CurOpenId = userInfo.OpenId;
            my.CurAgent = userInfo.Agent;
            my.MoldName = !string.IsNullOrEmpty(userInfo.MoldName) ? userInfo.MoldName : string.Empty;
            my.UserName = !string.IsNullOrEmpty(userInfo.UserName) ? userInfo.UserName : string.Empty;
            my.LicenseOwner = my.CarOwnerPerson.Name;
            my.Buid = userInfo.Id;
            my.RegisterDate = !string.IsNullOrEmpty(userInfo.RegisterDate) ? userInfo.RegisterDate : string.Empty;
            //InsuredName和PostedName是一个字段,被保险人姓名
            my.InsuredName = my.InsuredPerson.Name;
            my.PostedName = my.InsuredPerson.Name;
            //被保险人电话
            my.InsuredMobile = my.InsuredPerson.Mobile;
            //被保险人地址
            my.InsuredAddress = my.InsuredPerson.Address;
            //被保险人证件类型
            my.InsuredIdType = my.InsuredPerson.IdType;
            //车主证件类型
            my.IdType = my.CarOwnerPerson.IdType;//_quoteResultCarinfoRepository.GetOwnerIdType(userInfo.Id);
            my.IdCard = my.CarOwnerPerson.IdCard;
            //my.IdType = userInfo.InsuredIdType.HasValue ? userInfo.InsuredIdType.Value : 0;
            my.CredentislasNum = my.CarOwnerPerson.IdCard;
            my.InsuredIdCard = my.InsuredPerson.IdCard;
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
