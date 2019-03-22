using System;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Models;
using log4net;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class CarOrderSubmitInfoRepository : ICarOrderSubmitInfoRepository
    {
        private ILog logInfo = LogManager.GetLogger("INFO");
        private ILog logError = LogManager.GetLogger("ERROR");

        public long Add(bx_order_submit_info submitinfo)
        {
            bx_order_submit_info item = new bx_order_submit_info();
            try
            {
                item = DataContextFactory.GetDataContext().bx_order_submit_info.Add(submitinfo);
                var returnResult = DataContextFactory.GetDataContext().SaveChanges();
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }

            return item.id;
        }
        public bx_order_submit_info GetSubmitInfo(long buid, int source)
        {
            bx_order_submit_info submitInfo = new bx_order_submit_info();
            try
            {
                submitInfo = DataContextFactory.GetDataContext().bx_order_submit_info.FirstOrDefault(x => x.b_uid == buid && x.source == source);
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return submitInfo;
        }

    }
}
