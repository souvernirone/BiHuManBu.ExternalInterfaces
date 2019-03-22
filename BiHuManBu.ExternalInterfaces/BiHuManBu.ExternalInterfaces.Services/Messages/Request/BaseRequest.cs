using System.ComponentModel.DataAnnotations;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public  class BaseRequest
    {
        [Range(1, 1000000)]
        public int Agent { get; set; }
        [Required]
        [StringLength(32, MinimumLength = 32)]
        public string SecCode { get; set; }
    }

    public class BaseRequest2
    {
        [Range(1, 1000000)]
        public int Agent { get; set; }
        [Range(1, 1000000)]
        public int ChildAgent { get; set; }
        [Required]
        [StringLength(32, MinimumLength = 32)]
        public string SecCode { get; set; }
    }
}
