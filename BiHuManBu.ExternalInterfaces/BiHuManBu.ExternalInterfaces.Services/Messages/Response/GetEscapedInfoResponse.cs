using System.Collections.Generic;
using System.Net;
using BiHuManBu.ExternalInterfaces.Models;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Response
{
    public  class GetEscapedInfoResponse
    {
        public List<bx_claim_detail> List { get; set; }
        public bx_lastinfo Lastinfo { get; set; }
        public HttpStatusCode Status { get; set; }
    }
}
