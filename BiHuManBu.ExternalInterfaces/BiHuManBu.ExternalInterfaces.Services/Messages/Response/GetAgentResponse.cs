using System.Net;
using BiHuManBu.ExternalInterfaces.Models;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Response
{
    public class GetAgentResponse : BaseResponse
    {
        public AgentModel agent { get; set; }
        public HttpStatusCode Status { get; set; }
    }
}
