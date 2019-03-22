using System.ComponentModel.DataAnnotations;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class AppBaseRequest : BaseRequest
    {
        [Required]
        [StringLength(32, MinimumLength = 10)]
        public string CustKey { get; set; }

        [Range(1, 2100000000)]
        public int ChildAgent { get; set; }

        public string ChildName { get; set; }

        [Required]
        public string BhToken { get; set; }

        public int? Buid { get; set; }
    }
}
