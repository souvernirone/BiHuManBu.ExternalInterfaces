using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.IRepository;
using log4net;
using System;
using System.Linq;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class QuotehistoryRelatedRepository : IQuotehistoryRelatedRepository
    {
        private ILog logError = LogManager.GetLogger("ERROR");
        public QuotehistoryRelatedRepository() { }

        public bx_quotehistory_related GetModel(long buid, long groupspan)
        {
            var item = new bx_quotehistory_related();
            try
            {
                item = DataContextFactory.GetDataContext().bx_quotehistory_related.FirstOrDefault(x => x.b_uid == buid && x.groupspan == groupspan);
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return item;
        }
    }
}
