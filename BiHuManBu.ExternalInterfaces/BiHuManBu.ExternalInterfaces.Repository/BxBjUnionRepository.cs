using System;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Models;
using log4net;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class BxBjUnionRepository : IBxBjUnionRepository
    {
        private ILog logError;
        public BxBjUnionRepository()
        {
            logError = LogManager.GetLogger("ERROR");
        }
        public int Add(long buid, long bxid)
        {
            int count = 0;
            try
            {
                count = DataContextFactory.GetDataContext().Database.ExecuteSqlCommand(
                "insert into  bx_bj_union(b_uid,bx_id) values(" + buid + "," + bxid + ") on duplicate key update update_time=now() ");
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return count;
        }

        public bx_bj_union Find(long bxid)
        {
            bx_bj_union union = new bx_bj_union();
            try
            {
                union = DataContextFactory.GetDataContext().bx_bj_union.Where(i => i.bx_id == bxid).FirstOrDefault();
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return union;
        }
    }
}
