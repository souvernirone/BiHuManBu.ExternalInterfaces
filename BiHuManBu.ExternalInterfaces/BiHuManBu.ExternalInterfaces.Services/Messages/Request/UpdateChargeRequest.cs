using System.ComponentModel.DataAnnotations;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class UpdateChargeRequest
    {
        [Range(1, 1000000)]
        public int Agent { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        [Required]
        public string LicenseNo { get; set; }
        [Required]
        [StringLength(32, MinimumLength = 32)]
        public string SecCode { get; set; }
        /// <summary>
        /// bx_charge id
        /// </summary>
        public int BusyKey { get; set; }
        
    }
}
