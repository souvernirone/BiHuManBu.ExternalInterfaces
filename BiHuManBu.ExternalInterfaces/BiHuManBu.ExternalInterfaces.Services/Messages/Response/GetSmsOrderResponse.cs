using System.ComponentModel.DataAnnotations;
using BiHuManBu.ExternalInterfaces.Models;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Response
{
    public class GetSmsOrderResponse:BaseResponse
    {
        public bx_sms_order SmsOrder { get; set; }
    }
}
