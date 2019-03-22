
using BiHuManBu.ExternalInterfaces.Models;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Response
{
    public class GetSmsAccountResponse : BaseResponse
    {
        public bx_sms_account SmsAccount { get; set; }
    }
}
