using BiHuManBu.ExternalInterfaces.Models;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Response
{
    public class GetSubmitInfoResponse:BaseResponse
    {
        public bx_submit_info SubmitInfo { get; set; }

        public string CustKey { get; set; }

        public string OrderId { get; set; }

        public int BusinessStatus { get; set; }

        public string CheckCode { get; set; }
        
    }
}
