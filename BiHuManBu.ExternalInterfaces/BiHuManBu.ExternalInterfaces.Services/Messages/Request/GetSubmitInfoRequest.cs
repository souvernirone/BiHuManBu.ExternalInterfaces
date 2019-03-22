using System.ComponentModel.DataAnnotations;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class GetSubmitInfoRequest : BaseRequest
    {
        /// <summary>
        /// 客户端标识
        /// </summary>
        private string _custKey = string.Empty;

        /// <summary>
        /// 车牌号
        /// </summary>
        [Required]
        [StringLength(30, MinimumLength = 5)]
        public string LicenseNo { get; set; }

        public int ChildAgent { get; set; }

        [Range(0, 5)]
        public int IntentionCompany { get; set; }

        [StringLength(32, MinimumLength = 10)]
        public string CustKey { get { return _custKey; } set { _custKey = value; } }

        public string OrderId { get; set; }

        public string CheckCode { get; set; }
        //[Range(1, 4095)]
        public int SubmitGroup { get; set; }

        /// <summary>
        /// 目前只对app做登录状态的校验使用
        /// addby20161020
        /// </summary>
        public string BhToken { get; set; }
        /// <summary>
        /// 0小车，1大车，默认0
        /// </summary>
        public int RenewalCarType { get; set; }

        public int ShowChannel { get; set; }
        /// <summary>
        /// 人保精算口径
        /// </summary>
        public int ShowJs { get; set; }
        /// <summary>
        /// 是否展示 支付预定单号 0不展示  >0展示
        /// </summary>
        public int ShowOrderNo { get; set; }
    }
}
