using System.Collections.Generic;
using System.Net;
using BiHuManBu.ExternalInterfaces.Models;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Response
{
    public class GetViolationInfoResponse
    {
        public List<bx_violationlog> List { get; set; }
        public HttpStatusCode Status { get; set; }
    }
}
