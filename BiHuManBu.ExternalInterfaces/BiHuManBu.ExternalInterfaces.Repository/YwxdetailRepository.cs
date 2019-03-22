using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.IRepository;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class YwxdetailRepository: IYwxdetailRepository
    {
        private ILog logError = LogManager.GetLogger("ERROR");
        public YwxdetailRepository() { }

        public int DelList(long buid)
        {
            int countDel = 0;
            try
            {
                string strSql = string.Format("DELETE FROM bx_ywxdetail WHERE b_uid={0}", buid);
                int count = new EntityContext().Database.ExecuteSqlCommand(strSql);
                return count;
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return countDel;
        }

        public int AddList(List<bx_ywxdetail> jiayiList)
        {
            int countAdd = 0;
            try
            {
                EntityContext db = new EntityContext();
                var t = db.bx_ywxdetail.AddRange(jiayiList);
                db.SaveChanges();
                countAdd = t.Count();
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return countAdd;
        }

        public List<bx_ywxdetail> GetList(long buid) {
            return DataContextFactory.GetDataContext().bx_ywxdetail.Where(x => x.b_uid == buid).ToList();
        }
    }
}
