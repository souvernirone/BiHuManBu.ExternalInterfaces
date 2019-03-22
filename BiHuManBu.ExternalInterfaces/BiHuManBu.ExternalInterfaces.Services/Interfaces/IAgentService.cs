using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.Interfaces
{
    public interface IAgentService
    {
        GetAgentResponse AddAgent(PostAddAgentRequest request, IEnumerable<KeyValuePair<string, string>> pairs);
        bx_agent GetAgent(int agentId);
        GetAgentResponse GetAgentByOpenId(GetByOpenIdRequest request, IEnumerable<KeyValuePair<string, string>> pairs);
        GetAgentResponse GetAgentByOpenIdOld(GetByOpenIdRequest request, IEnumerable<KeyValuePair<string, string>> pairs);
        GetAgentIdentityAndRateResponse GetAgent(GetAgentIdentityAndRateRequest request, IEnumerable<KeyValuePair<string, string>> pairs);
        GetAgentSourceResponse GetAgentSource(GetAgentResourceRequest request, IEnumerable<KeyValuePair<string, string>> pairs);
        GetAgentSourceResponse GetAgentNewSource(GetAgentResourceRequest request, IEnumerable<KeyValuePair<string, string>> pairs);
        GetAgentListResponse GetAgentList(GetAgentRequest request,
            IEnumerable<KeyValuePair<string, string>> pairs);
        ApproveAgentResponse ApproveAgent(ApproveAgentRequest request,
            IEnumerable<KeyValuePair<string, string>> pairs);

        bool IsTopAgentId(int agentId);

        /// <summary>
        /// 根据顶级代理Id获取渠道列表
        /// </summary>
        /// <param name="url"></param>
        /// <param name="agentId">顶级代理Id</param>
        /// <returns></returns>
        List<AgentCity> GetSourceList(string url, int agentId);

        List<bx_agent> GetAgentExpireList(int status);
    }
}
