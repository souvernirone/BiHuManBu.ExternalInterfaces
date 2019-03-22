using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Models;
using log4net;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class SaveQuoteRepository : ISaveQuoteRepository
    {
        private ILog logInfo = LogManager.GetLogger("INFO");
        private ILog logError = LogManager.GetLogger("ERROR");
        public bx_savequote GetSavequoteByBuid(long buid)
        {
            bx_savequote savequote = new bx_savequote();
            try
            {
                savequote = DataContextFactory.GetDataContext().bx_savequote.FirstOrDefault(x => x.B_Uid == buid);
            }
            catch (Exception ex)
            {

                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return savequote;
        }

        public async Task<bx_savequote> GetSavequoteByBuidAsync(long buid)
        {
            bx_savequote savequote = new bx_savequote();
            try
            {
                savequote = DataContextFactory.GetDataContext().bx_savequote.FirstOrDefault(x => x.B_Uid == buid);
            }
            catch (Exception ex)
            {

                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return savequote;
        }

        public long Add(bx_savequote savequote)
        {
            long count = 0;
            try
            {
                var item = DataContextFactory.GetDataContext().bx_savequote.Add(savequote);
                var returnResult = DataContextFactory.GetDataContext().SaveChanges();
                count = item.Id;
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }

            return count;
        }
        public int Update(bx_savequote savequote)
        {
            int count = 0;
            try
            {
                DataContextFactory.GetDataContext().bx_savequote.AddOrUpdate(savequote);
                count = DataContextFactory.GetDataContext().SaveChanges();
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return count;
        }
    }
}
