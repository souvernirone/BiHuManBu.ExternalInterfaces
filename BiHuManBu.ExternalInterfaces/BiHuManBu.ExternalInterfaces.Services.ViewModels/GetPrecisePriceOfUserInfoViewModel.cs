﻿
namespace BiHuManBu.ExternalInterfaces.Services.ViewModels
{
    public class GetPrecisePriceOfUserInfoViewModel
    {
        /// <summary>
        /// 车牌号
        /// </summary>
        public string LicenseNo { get; set; }
        /// <summary>
        /// 交强险到期时间（续保）
        /// </summary>
        public string ForceExpireDate { get; set; }
        /// <summary>
        /// 商业险到期时间（续保）
        /// </summary>
        public string BusinessExpireDate { get; set; }
        /// <summary>
        /// 商业险起保日期（报价）
        /// </summary>
        public string BusinessStartDate { get; set; }
        /// <summary>
        /// 交强险起保日期（报价）
        /// </summary>
        public string ForceStartDate { get; set; }
        /// <summary>
        /// 商业险到期日期（报价）
        /// </summary>
        public string BusinessEndDate { get; set; }
        /// <summary>
        /// 交强险到期日期（报价）
        /// </summary>
        public string ForceEndDate { get; set; }
        public string InsuredName { get; set; }
        public string InsuredIdCard { get; set; }
        public int InsuredIdType { get; set; }
        public string InsuredMobile { get; set; }
        public string HolderName { get; set; }
        public string HolderIdCard { get; set; }
        public int HolderIdType { get; set; }
        public string HolderMobile { get; set; }
        /// <summary>
        /// 车主手机号
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 投保人/被保人邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 精友编码
        /// </summary>
        public string AutoMoldCode { get; set; }
        public string VehicleInfo { get; set; }

        public RelationPerson CarOwnerPerson { get; set; }
        public RelationPerson HolderPerson { get; set; }
        public RelationPerson InsuredPerson { get; set; }

        public PostStartDateTime PostStartDate { get; set; }

        /// <summary>
        /// 身份证后六位
        /// </summary>
        public string SixDigitsAfterIdCard { get; set; }
    }

    public class RelationPerson
    {
        public int IdType { get; set; }
        public string IdCard { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        /// <summary>
        /// 性别 1男2女
        /// </summary>
        public int Sex { get; set; }
        /// <summary>
        /// 民族
        /// </summary>
        public string Nation { get; set; }
        /// <summary>
        /// 出生年月日
        /// </summary>
        public string Birthday { get; set; }
        /// <summary>
        /// 证件有效开始时间
        /// </summary>
        public string CertiStartDate { get; set; }
        /// <summary>
        /// 证件有效结束时间
        /// </summary>
        public string CertiEndDate { get; set; }
        /// <summary>
        /// 签发机关
        /// </summary>
        public string Authority { get; set; }
    }

    public class PostStartDateTime
    {
        public string BusinessStartDate { get; set; }
        public string ForceStartDate { get; set; }
    }
}
