using BiHuManBu.ExternalInterfaces.Models.IRepository;
using BiHuManBu.ExternalInterfaces.Services.PrecisePriceService.Interfaces;
using System;

namespace BiHuManBu.ExternalInterfaces.Services.PrecisePriceService.Implementations
{
    public class UpdateAgentSelect : IUpdateAgentSelect
    {
        private readonly IAgentSelectRepository _agentSelectRepository;
        public UpdateAgentSelect(IAgentSelectRepository agentSelectRepository)
        {
            _agentSelectRepository = agentSelectRepository;
        }
        public void UpdateAgentSelectMethod(string multiChannels, long buid)
        {
            if (!string.IsNullOrWhiteSpace(multiChannels))
            {
                //List<MultiChannels> models = multiChannels.FromJson<List<MultiChannels>>();
                //删除bx_agent_select记录
                _agentSelectRepository.DelMulti(buid);
                string strSqlInsert = string.Format(" INSERT INTO bx_agent_select (b_uid,channels,status,createtime,updatetime) values ({0},'{1}',1,'{2}','{2}'); ", buid, multiChannels, DateTime.Now);
                //执行插入操作
                _agentSelectRepository.AddMulti(strSqlInsert);
            }
        }
    }
}
