using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Services.ViewModels
{
    public class ExpireAgentViewModel
    {
        public List<ExpireAgent> List { get; set; }
    }

    public class ExpireAgent
    {
        public int AgentId { get; set; }
        public string AgentName { get; set; }
        public string EndDate { get; set; }
    }
}
