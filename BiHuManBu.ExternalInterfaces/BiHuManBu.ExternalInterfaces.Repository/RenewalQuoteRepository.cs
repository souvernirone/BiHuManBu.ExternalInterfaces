using System;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Models;
using log4net;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class RenewalQuoteRepository : IRenewalQuoteRepository
    {
        private ILog logError = LogManager.GetLogger("Error");
        public bx_renewalquote FindByBuid(long buid)
        {
            var item = new bx_renewalquote();
            try
            {
                item = DataContextFactory.GetDataContext().bx_renewalquote.FirstOrDefault(x => x.B_Uid == buid);
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return item;
        }

    }
}
