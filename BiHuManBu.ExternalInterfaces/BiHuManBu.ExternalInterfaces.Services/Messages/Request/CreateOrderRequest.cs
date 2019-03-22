using System;
using System.ComponentModel.DataAnnotations;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class CreateOrderRequest
    {
        /// <summary>
        /// 定单号 
        /// </summary>
        [RegularExpression("^[0-9a-zA-Z]{15,32}$")]
        public string OrderNum { get; set; }


        /// <summary>
        /// 被保险人
        /// </summary>
        public string InsuredName { get; set; }

        /// <summary>
        /// 证件类型
        /// </summary>
        public int IdType { get; set; }

        /// <summary>
        /// 证件号码
        /// </summary>
        //[RegularExpression("^[0-9a-zA-Z]{10,32}$")]
        public string IdNum { get; set; }

        /// <summary>
        /// 报价ID
        /// </summary>
        [Range(1, 21000000000)]
        public long Buid { get; set; }

        /// <summary>
        /// 保险公司
        /// </summary>
        [Range(0, 9223372036854775807)]
        public long Source { get; set; }

        /// <summary>
        /// 发票抬头
        /// </summary>
        [StringLength(30, MinimumLength = 0)]
        public string Receipt { get; set; }

        /// <summary>
        /// 发票类型
        /// </summary>
        public int ReceiptHead { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public int PayType { get; set; }
        /// <summary>
        /// 配送方式
        /// </summary>
        public int DistributionType { get; set; }
        /// <summary>
        /// 配送地址
        /// </summary>
        public string DistributionAddress { get; set; }
        /// <summary>
        /// 配送联系人
        /// </summary>
        public string DistributionName { get; set; }
        /// <summary>
        /// 配送联系电话
        /// </summary>
        public string DistributionPhone { get; set; }
        /// <summary>
        /// 送单时间
        /// </summary>
        public DateTime? DistributionTime { get; set; }
        /// <summary>
        /// 保费总额
        /// </summary>
        public double InsurancePrice { get; set; }
        /// <summary>
        /// 运费
        /// </summary>
        public double CarriagePrice { get; set; }
        /// <summary>
        /// 总额
        /// </summary>
        public double TotalPrice { get; set; }

        public int UserId { get; set; }

        /// <summary>
        /// 当前经纪人id
        /// </summary>
        [Range(1, 100000000)]
        public int Agent { get; set; }

        /// <summary>
        /// 校验码
        /// </summary>
        [StringLength(32, MinimumLength = 32)]
        public string SecCode { get; set; }

        public string IdImgFirst { get; set; }

        public string IdImgSecond { get; set; }
        [Range(1, 100000000)]
        public int Topagent { get; set; }

        //[Required]
        public string OpenId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [RegularExpression(@"^[1][3-8]+\d{9}")]
        public string Mobile { get; set; }

        public int? AddressId { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string ContactsName { get; set; }

        /// <summary>
        /// 商业险费率
        /// </summary>
        //[DataMember(IsRequired = true)]
        public double BizRate { get; set; }


        /// <summary>
        /// 是否是用新版的source提交数据，1是0否
        /// 微信传1，pc未传此值
        /// </summary>
        public int? IsNewSource { get; set; }

        #region 增加省市县Id
        public int? ProvinceId { get; set; }
        public int? CityId { get; set; }
        public int? AreaId { get; set; }

        public string ProvinceName { get; set; }
        public string CityName { get; set; }
        public string AreaName { get; set; }

        #endregion

        /// <summary>
        /// 数据来源 目前未使用 微信传7
        /// </summary>
        public int Fountain { get; set; }

        /// <summary>
        /// 当前代理人Id。微信新增字段
        /// 20170228账号统一，openid查询改为childagent查询
        /// </summary>
        //[Range(1, 1000000)]
        public int ChildAgent { get; set; }
        /// <summary>
        /// 多图片地址
        /// </summary>
        public string ImageUrls { get; set; }
    }
}
