
using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Models
{
    public interface IAgentDistributedRepository
    {
        List<bx_agent_distributed> FindByParentAgent(int parentAgentId);
    }
}
