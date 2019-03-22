using System.ComponentModel.DataAnnotations;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class GetDriverLicenseCarTypeRequest
    {
        [Range(1, 1000000)]
        public int Agent { set; get; }
        [Required]
        [StringLength(32, MinimumLength = 32)]
        public string SecCode { set; get; }
      
    }
}
