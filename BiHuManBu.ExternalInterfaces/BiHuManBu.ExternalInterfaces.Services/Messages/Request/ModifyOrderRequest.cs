using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class ModifyOrderRequest:BaseRequest
    {
        /// <summary>
        /// 订单ID
        /// </summary>
        [DataMember(IsRequired = true)]
        public long OrderID { get; set; }

        /// <summary>
        /// 被保险人
        /// </summary>
        //[Required]
        //[StringLength(20, MinimumLength = 1)]
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
        /// 保险公司
        /// </summary>
        //[DataMember(IsRequired = true)]
        public int Source { get; set; }

        /// <summary>
        /// 发票内容
        /// </summary>
        [StringLength(100, MinimumLength = 0)]
        public string Receipt { get; set; }

        /// <summary>
        /// 发票抬头，0-个人，1-单位
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
        /// 配送联系人
        /// </summary>
        [StringLength(20, MinimumLength = 0)]
        public string DistributionName { get; set; }

        /// <summary>
        /// 配送联系电话
        /// </summary>
        [StringLength(40, MinimumLength = 0)]
        public string DistributionPhone { get; set; }
        
        /// <summary>
        /// 配送地址
        /// </summary>
        [StringLength(200, MinimumLength = 0)]
        public string DistributionAddress { get; set; }

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
                
        /// <summary>
        /// 身份证正面
        /// </summary>
        public string IdImgFirst { get; set; }

        /// <summary>
        /// 身份证反面
        /// </summary>
        public string IdImgSecond { get; set; }
        
        public string OpenId { get; set; }

        public int AddressId { get; set; }

        /// <summary>
        /// 代理人手机号
        /// </summary>
        //[Required]
        [RegularExpression(@"^[1][3-8]+\d{9}")]
        public string Mobile { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        //[Required]
        [StringLength(50, MinimumLength = 1)]
        public string ContactsName { get; set; }

        /// <summary>
        /// 是否是用新版的source提交数据，1是0否
        /// </summary>
        public int? IsNewSource { get; set; }

        /// <summary>
        /// 多图片地址
        /// </summary>
        public string ImageUrls { get; set; }
    }
}
