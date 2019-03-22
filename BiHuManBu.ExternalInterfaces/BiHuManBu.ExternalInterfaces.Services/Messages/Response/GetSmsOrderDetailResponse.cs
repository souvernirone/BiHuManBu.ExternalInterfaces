
using BiHuManBu.ExternalInterfaces.Models;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Response
{
    public class GetSmsOrderDetailResponse:BaseResponse
    {
        public bx_sms_order BxSmsOrder { get; set; }
    }
}
