using System;
using BiHuManBu.ExternalInterfaces.Models;
using log4net;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class ChargeHistoryRepository:IChargeHistoryRepository
    {
        private ILog logError = LogManager.GetLogger("ERROR");
        public long Add(bx_charge_history history)
        {
            long count = 0;
            try
            {
                var item = DataContextFactory.GetDataContext().bx_charge_history.Add(history);
                DataContextFactory.GetDataContext().SaveChanges();
                count= item.id;
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return count;
        }
    }
}
