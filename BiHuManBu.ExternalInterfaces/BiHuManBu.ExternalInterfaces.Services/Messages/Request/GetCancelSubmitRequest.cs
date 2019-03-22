using System.ComponentModel.DataAnnotations;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class GetCancelSubmitRequest
    {
        [Required]
        [StringLength(30, MinimumLength = 5)]
        public string LicenseNo { set; get; }
        [Range(1,4096)]
        public int SubmitGroup { set; get; }
        [Range(1, 1000000)]
        public int Agent { set; get; }
        public int ChildAgent { get; set; }
        [StringLength(32, MinimumLength = 10)]
        public string CustKey { get; set; }
        [Required]
        [StringLength(32, MinimumLength = 32)]
        public string SecCode { set; get; }
        [StringLength(30,MinimumLength = 10)]
        public string BizNo { get; set; }
        [StringLength(30, MinimumLength = 10)]
        public string ForceNo { get; set; }
        /// <summary>
        /// 0小车，1大车，默认0
        /// </summary>
        public int RenewalCarType { get; set; }
    }
}
