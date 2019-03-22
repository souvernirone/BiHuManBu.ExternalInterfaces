using System.ComponentModel.DataAnnotations;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class UserInfoValidateRequest
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
    }
}
