
using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Models.ReportModel;

namespace BiHuManBu.ExternalInterfaces.Models
{
    public interface IAgentUKeyRepository
    {
        List<CityUKeyModel> GetList(int agentId);
        bx_agent_ukey GetModel(int uId);
        int UpdateModel(bx_agent_ukey model);
        bx_agent_ukey GetAgentUKeyModel(int Id);
    }
}
