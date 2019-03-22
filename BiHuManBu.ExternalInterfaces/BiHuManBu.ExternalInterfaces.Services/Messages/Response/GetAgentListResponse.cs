
using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Response
{
    public class GetAgentListResponse:BaseResponse
    {
        public int totalCount { get; set; }
        public List<bx_agent> AgentList { get; set; }
    }
}
