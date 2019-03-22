using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class PostIndependentSubmitRequest : BaseRequest
    {
        /// <summary>
        /// 车牌号
        /// </summary>
        [Required]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "您输入的字符串长度应该在5-30个字符内")]
        public string LicenseNo { set; get; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(32, MinimumLength = 10, ErrorMessage = "custkey应该是10-32个字符范围内")]
        public string CustKey { get; set; }
        public int ChildAgent { get; set; }

        /// <summary>
        /// 0小车，1大车，默认0
        /// </summary>
        public int RenewalCarType { get; set; }
        /// <summary>
        /// 保司新渠道1,2,4,8... 等bx_userinfo改为long再将此值改过来
        /// </summary>
        [Range(1, 10000000000000000000)]        
        public int Source { get; set; }


        /// <summary>
        /// 签收人姓名
        /// </summary>
        public string SignerName { get; set; }

        /// <summary>
        /// 签收人手机号
        /// </summary>
        public string SignerTel { get; set; }

        /// <summary>
        ///  签收人地址
        /// </summary>
        public string SingerAddress { get; set; }

        /// <summary>
        /// 邮政编码
        /// </summary>
        public string ZipCode { get; set; }

        /// <summary>
        /// 商业保单形式 1:电子保单0:纸质保单
        /// </summary>
        public string BizPolicyType { get; set; }

        /// <summary>
        /// 商业电子发票形式 004 专票 007 普票026 电子发票 当联合单或者个单都电子发票时，配送信息可以为空
        /// </summary>
        public string BizElcInvoice { get; set; }

        /// <summary>
        /// 商业投保单号
        /// </summary>
        public string BizNo { get; set; }

        /// <summary>
        /// 交强投保单形式
        /// </summary>
        public string ForcePolicyType { get; set; }

        /// <summary>
        /// 交强电子发票类型
        /// </summary>
        public string ForceElcInvoice { get; set; }

        /// <summary>
        /// 交强投保单号
        /// </summary>
        public string ForceNO { get; set; }

        /// <summary>
        /// 产品代码
        /// </summary>
        public string ProductNo { get; set; }


        #region 新版安心核保新增参数 20180509
        /// <summary>
        /// 支付成功路径
        /// </summary>
        public string PayFinishUrl { get; set; }

        /// <summary>
        /// 取消支付路径
        /// </summary>
        public string PayCancelUrl { get; set; }

        /// <summary>
        /// 渠道返回路径
        /// </summary>
        public string BgRetUrl { get; set; }

        /// <summary>
        /// 错误支付路径
        /// </summary>
        public string PayErrorUrl { get; set; }
        /// <summary>
        /// 附加数据
        /// </summary>
        public string Attach { get; set; }
        #endregion

        ///// <summary>
        ///// 被保人手机号
        ///// </summary>
        //[StringLength(11, MinimumLength = 0)]
        //public string InsuredMobile { get; set; }

        ///// <summary>
        ///// 投保人手机号
        ///// </summary>
        //public string HolderMobile { get; set; }
        /// <summary>
        /// 车主手机号
        /// </summary>
        [StringLength(11, MinimumLength = 11)]
        public string Mobile { get; set; }
        /// <summary>
        /// 车主姓名
        /// </summary>
        [StringLength(30, MinimumLength = 0)]
        public string CarOwnersName { get; set; }
        /// <summary>
        /// 车主身份证号
        /// </summary>
        [StringLength(50, MinimumLength = 0)]
        public string IdCard { get; set; }
        /// <summary>
        /// 车主证件类型
        /// </summary>
        public int OwnerIdCardType { get; set; }
        #region 被保人信息

        /// <summary>
        /// 被保险人姓名
        /// </summary>
        [StringLength(40, MinimumLength = 0)]
        public string InsuredName { get; set; }

        /// <summary>
        /// 被保人身份证ID
        /// </summary>
        [StringLength(50, MinimumLength = 0)]
        public string InsuredIdCard { get; set; }

        /// <summary>
        ///  证件类型
        /// </summary>
        //[Range(0,50)]
        public int InsuredIdType { get; set; }

        /// <summary>
        /// 被保人手机号
        /// </summary>
        [StringLength(11, MinimumLength = 0)]
        public string InsuredMobile { get; set; }
        /// <summary>
        /// 被保险人地址
        /// </summary>
        public string InsuredAddress { get; set; }
        /// <summary>
        /// 被保险人邮箱地址
        /// </summary>
        [RegularExpression(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$")]
        public string InsuredEmail { get; set; }

        #endregion 

        #region 投保人信息
        /// <summary>
        /// 投保人身份证ID
        /// </summary>
        [StringLength(50, MinimumLength = 0)]
        public string HolderIdCard { get; set; }
        /// <summary>
        /// 投保人
        /// </summary>
        [StringLength(40, MinimumLength = 0)]
        public string HolderName { get; set; }
        /// <summary>
        ///  证件类型
        /// </summary>
        public int HolderIdType { get; set; }
        /// <summary>
        /// 投保人手机号
        /// </summary>
        public string HolderMobile { get; set; }
        /// <summary>
        /// 投保险人地址
        /// </summary>
        public string HolderAddress { get; set; }
        /// <summary>
        /// 投保人邮箱地址
        /// </summary>
        [RegularExpression(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$")]
        public string HolderEmail { get; set; }
        #endregion


    }
}
