using System;
using System.Collections.Generic;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Models.IRepository;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class AgentSelectRepository : IAgentSelectRepository
    {
        public int AddMulti(string strAddSql)
        {
            int count = DataContextFactory.GetDataContext().Database.ExecuteSqlCommand(strAddSql);
            return count;
        }

        public int DelMulti(long buid)
        {
            //STATUS=0，标识已删除，1未删除
            string strSql = string.Format("UPDATE bx_agent_select SET STATUS=0,updatetime='{0}' WHERE b_uid={1}",DateTime.Now, buid);
            int count = DataContextFactory.GetDataContext().Database.ExecuteSqlCommand(strSql);
            return count;
        }

        public List<long> GetMulti(int agent,int topagent)
        {
            var agentSelect =
                DataContextFactory.GetDataContext()
                    .bx_quote_many_source.Where(x => x.child_agent == agent)
                    .ToList()
                    .Select(x => x.channel_id)
                    .ToList();
            if (agentSelect.Count == 0)
            {
                agentSelect =
                DataContextFactory.GetDataContext()
                    .bx_quote_many_source.Where(x => x.top_agent == topagent)
                    .ToList()
                    .Select(x => x.channel_id)
                    .ToList();
            }
            var list = new List<long>();
            foreach (var tint in agentSelect)
            {
                list.Add(tint);
            }
            return list;
        }
    }
}
