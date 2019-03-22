using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Models;
using log4net;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class AgentPointRepository : IAgentPointRepository
    {
        private ILog logError = LogManager.GetLogger("ERROR");
        
        public List<bx_agentpoint> Find(int agentId)
        {
            List<bx_agentpoint> agentpoints = new List<bx_agentpoint>();
            try
            {
                agentpoints = DataContextFactory.GetDataContext().bx_agentpoint.Where(x => x.TopAgent == agentId && (!x.Deleted)).ToList();
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);

            }
            return agentpoints;
        }

        public bx_agentpoint FindById(int agentpointId)
        {
            bx_agentpoint agentpoint = new bx_agentpoint();
            try
            {
                agentpoint = DataContextFactory.GetDataContext().bx_agentpoint.FirstOrDefault(x => x.id == agentpointId && x.Deleted == false);
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return agentpoint;
        }


        public bx_agentpoint FindWorkAgentpoint(int agentId)
        {
            bx_agentpoint agentpoint = new bx_agentpoint();
            try
            {
                agentpoint = DataContextFactory.GetDataContext().bx_agentpoint.OrderByDescending(o => o.ModifiTime).FirstOrDefault(x => x.AgentId == agentId && x.IsShowForWebChat == true && x.Deleted == false);
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return agentpoint;
        }
    }
}
