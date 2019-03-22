using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.IRepository;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class SpecialOptionRepository : ISpecialOptionRepository
    {
        private ILog logError = LogManager.GetLogger("ERROR");
        public int AddList(List<bx_specialoption> specialOptionList)
        {
            int countAdd = 0;
            try
            {
                EntityContext db = new EntityContext();
                var t = db.bx_specialoption.AddRange(specialOptionList);
                db.SaveChanges();
                countAdd = t.Count();
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return countAdd;
        }
        public int DelList(long buid)
        {
            int countDel = 0;
            try
            {
                //STATUS=0，标识已删除，1未删除
                string strSql = string.Format("UPDATE bx_specialoption SET STATUS=0,UpdateTime='{0}' WHERE BUid={1}", DateTime.Now, buid);
                int count = new EntityContext().Database.ExecuteSqlCommand(strSql);
                return count;
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return countDel;
        }
    }
}
