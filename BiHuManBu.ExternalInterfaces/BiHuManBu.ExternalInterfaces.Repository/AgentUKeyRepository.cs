using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.ReportModel;
using log4net;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class AgentUKeyRepository : IAgentUKeyRepository
    {
        private ILog logError = LogManager.GetLogger("ERROR");
        public List<CityUKeyModel> GetList(int agentId)
        {
            List<CityUKeyModel> agentukey = new List<CityUKeyModel>();
            try
            {
                string sqlList = string.Format(@" SELECT uk.id AS Uid,uk.name AS Uname,c.id AS Cid,c.city_name AS Cname,uk.source AS Source FROM bx_city c 
 RIGHT JOIN bx_agent_ukey uk ON uk.city_id=c.id WHERE uk.agent_Id={0} AND uk.isusedeploy=1 ORDER BY uk.create_time DESC", agentId);
                //查询列表
                agentukey = DataContextFactory.GetDataContext().Database.SqlQuery<CityUKeyModel>(sqlList).ToList();
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return agentukey;
        }

        public bx_agent_ukey GetModel(int uId)
        {
            var item = new bx_agent_ukey();
            try
            {
                item = DataContextFactory.GetDataContext().bx_agent_ukey.FirstOrDefault(x => x.id == uId);
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return item;
        }
        public bx_agent_ukey GetAgentUKeyModel(int Id)
        {
            var item = new bx_agent_ukey();
            try
            {
                item = DataContextFactory.GetDataContext().bx_agent_ukey.FirstOrDefault(x => x.id == Id&&x.deleted==0);
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return item;
        }

        public int UpdateModel(bx_agent_ukey model)
        {
            int count = 0;
            try
            {
                DataContextFactory.GetDataContext().bx_agent_ukey.AddOrUpdate(model);
                count = DataContextFactory.GetDataContext().SaveChanges();
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return count;
        }
    }
}
