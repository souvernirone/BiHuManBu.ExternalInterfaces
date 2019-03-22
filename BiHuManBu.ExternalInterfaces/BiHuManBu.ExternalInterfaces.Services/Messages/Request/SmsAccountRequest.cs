
using System.ComponentModel.DataAnnotations;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class SmsAccountRequest:BaseRequest
    {
        [Range(1,2100000000)]
        public int CurAgent { get; set; }
    }
}
