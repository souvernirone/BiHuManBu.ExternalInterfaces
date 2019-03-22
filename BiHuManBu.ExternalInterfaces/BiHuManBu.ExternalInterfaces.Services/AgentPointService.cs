using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;

namespace BiHuManBu.ExternalInterfaces.Services
{
    public class AgentPointService : IAgentPointService
    {
        public IAgentPointRepository _AgentPointRepository;

        public AgentPointService(IAgentPointRepository agentPointRepository)
        {
            _AgentPointRepository = agentPointRepository; 
        }

        public List<bx_agentpoint> GetAgentpoints(int agentId)
        {
            return _AgentPointRepository.Find(agentId);
        }
        
        public bx_agentpoint GetAgentpoint(int agentpointId)
        {
            return _AgentPointRepository.FindById(agentpointId);
        }

        public bx_agentpoint GetWorkAgentpoint(int agentId)
        {
            return _AgentPointRepository.FindWorkAgentpoint(agentId);
        }
    }
}
