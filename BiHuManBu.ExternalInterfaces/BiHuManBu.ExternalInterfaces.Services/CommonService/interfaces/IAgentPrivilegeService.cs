using BiHuManBu.ExternalInterfaces.Services.CommonService.implementations;

namespace BiHuManBu.ExternalInterfaces.Services.CommonService.interfaces
{
    public interface IAgentPrivilegeService
    {
        AgentValidResult CanUse(int agentId);
    }
}