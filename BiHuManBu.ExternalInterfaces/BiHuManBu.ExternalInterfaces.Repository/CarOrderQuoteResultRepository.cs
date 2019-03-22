using System;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Models;
using log4net;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class CarOrderQuoteResultRepository : ICarOrderQuoteResultRepository
    {
        private ILog logInfo = LogManager.GetLogger("INFO");
        private ILog logError = LogManager.GetLogger("ERROR");

        public long Add(bx_order_quoteresult quoteresult)
        {
            bx_order_quoteresult item=new bx_order_quoteresult();
            try
            {
                item = DataContextFactory.GetDataContext().bx_order_quoteresult.Add(quoteresult);
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return item.Id;
        }

        public bx_order_quoteresult GetQuoteResultByBuid(long buid, int source)
        {
            bx_order_quoteresult quoteresult = new bx_order_quoteresult();
            try
            {
                quoteresult = DataContextFactory.GetDataContext().bx_order_quoteresult.FirstOrDefault(x => x.B_Uid == buid && x.Source == source);
                
            }
            catch (Exception ex)
            {
               logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return quoteresult;
        }
    }
}
