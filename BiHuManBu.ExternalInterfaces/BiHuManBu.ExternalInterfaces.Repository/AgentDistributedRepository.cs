using System;
using System.Collections.Generic;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Models;
using log4net;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class AgentDistributedRepository : IAgentDistributedRepository
    {
        private ILog logError;
        public AgentDistributedRepository()
        {
            logError = LogManager.GetLogger("ERROR");
        }
        public List<bx_agent_distributed> FindByParentAgent(int parentAgentId)
        {
            var item = new List<bx_agent_distributed>();
            try
            {
                item = DataContextFactory.GetDataContext().bx_agent_distributed.Where(x => x.ParentAgentId == parentAgentId && x.AgentType == 1 && x.Deteled == false).OrderByDescending(o => o.Id).ToList();
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return item;
        }
    }
}
