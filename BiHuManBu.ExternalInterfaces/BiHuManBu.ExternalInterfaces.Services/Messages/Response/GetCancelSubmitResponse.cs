
using System.Net;
using BiHuManBu.ExternalInterfaces.Models.ReportModel;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Response
{
    public class GetCancelSubmitResponse
    {
        public HttpStatusCode Status { get; set; }
        public CancelSubmitResult Result { get; set; }
        public int BusinessStatus { get; set; }
        public string BusinessMessage { get; set; }
    }
}
