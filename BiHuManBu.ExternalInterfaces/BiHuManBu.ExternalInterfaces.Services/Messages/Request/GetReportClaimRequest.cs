using System.ComponentModel.DataAnnotations;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class GetReportClaimRequest
    {
        [Required]
        public string LicenseNo { get; set; }
        public string EngineNo { get; set; }
        public string CarVin { get; set; }
        public string Type { get; set; }
        [Range(1, 1000000)]
        public int Agent { get; set; }
        [Required]
        [StringLength(32, MinimumLength = 10)]
        public string CustKey { get; set; }
        [Required]
        [StringLength(32, MinimumLength = 32)]
        public string SecCode { get; set; }
        /// <summary>
        /// bx_charge id
        /// </summary>
        public int BusyKey { get; set; }

    }
}
