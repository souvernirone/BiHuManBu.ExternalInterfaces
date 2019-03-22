using System;
using System.Collections.Generic;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Models;
using log4net;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class GscRenewalRepository : IGscRenewalRepository
    {
        private ILog logError = LogManager.GetLogger("ERROR");

        public List<bx_gsc_renewal> FindListByBuid(long buid)
        {
            var bxGscRenewal = new List<bx_gsc_renewal>();
            try
            {
                bxGscRenewal = DataContextFactory.GetDataContext().bx_gsc_renewal.Where(x => x.B_uid == buid).ToList();
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return bxGscRenewal;
        }
    }
}
