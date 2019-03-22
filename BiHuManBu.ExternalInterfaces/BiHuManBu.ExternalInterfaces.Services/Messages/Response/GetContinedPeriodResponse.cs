using BiHuManBu.ExternalInterfaces.Models;
using System.Collections.Generic;
using System.Net;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Response
{
    public class GetContinedPeriodResponse
    {
        public HttpStatusCode Status { get; set; }
        public List<bx_cityquoteday> Items { get; set; } 
    }
}
