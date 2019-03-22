
using System.ComponentModel.DataAnnotations;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class GetAppVersionRequest//:BaseRequest
    {
        [Range(6,7)]
        public int RenewalType { get; set; }
    }
}
