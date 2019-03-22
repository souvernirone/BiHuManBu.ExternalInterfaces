using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.IRepository;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class PictureRepository : EFRepositoryBase<bx_picture>, IPictureRepository
    {
        private ILog logError = LogManager.GetLogger("ERROR");
        public int UpdateState(long buid)
        {
            string strSQL = string.Format("update bx_picture set state=0,update_time='{0}' where b_uid={1} and source=12 and state>0", DateTime.Now, buid);
            int executeCount = 0;
            try
            {
                executeCount = DataContextFactory.GetDataContext().Database.ExecuteSqlCommand(strSQL);
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return executeCount;

        }
    }
}
