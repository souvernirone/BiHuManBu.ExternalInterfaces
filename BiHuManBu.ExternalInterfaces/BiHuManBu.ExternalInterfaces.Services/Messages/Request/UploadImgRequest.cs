
using System.ComponentModel.DataAnnotations;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class UploadImgRequest
    {
        [Required]
        public string baseContent { get; set; }
    }
}
