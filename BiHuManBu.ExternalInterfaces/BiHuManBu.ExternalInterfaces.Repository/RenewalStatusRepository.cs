using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Models;
using log4net;
namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class RenewalStatusRepository : IRenewalStatusRepository
    {
        private ILog logError = LogManager.GetLogger("ERROR");
        public long Add(bx_renewalstatus renewalStatus)
        {
            long id = 0;
            try
            {
                EntityContext db = new EntityContext();
                var item = db.bx_renewalstatus.Add(renewalStatus);
                db.SaveChanges();
                id = item.Id;
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return id;
        }
    }
}
