using System;
using System.Data.Entity.Migrations;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Models;
using log4net;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class QuoteReqCarinfoRepository : IQuoteReqCarinfoRepository
    {
        //private EntityContext _context = new EntityContext();
       
        public bx_quotereq_carinfo Find(long buid)
        {
            return DataContextFactory.GetDataContext().bx_quotereq_carinfo.FirstOrDefault(x => x.b_uid == buid);
        }

        public bx_quotereq_carinfo Add(bx_quotereq_carinfo item)
        {
            bx_quotereq_carinfo model = new bx_quotereq_carinfo();
            try
            {
                model = DataContextFactory.GetDataContext().bx_quotereq_carinfo.Add(item);
                DataContextFactory.GetDataContext().SaveChanges();
            }
            catch (Exception ex)
            {
               ILog logError = LogManager.GetLogger("ERROR");
               logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
           
            return model;
        }

        public int Update(bx_quotereq_carinfo item)
        {
            int count = 0;
            try
            {
                DataContextFactory.GetDataContext().bx_quotereq_carinfo.AddOrUpdate(item);
                count = DataContextFactory.GetDataContext().SaveChanges();
            }
            catch (Exception ex)
            {
                ILog logError = LogManager.GetLogger("ERROR");
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return count;
        }

        //public void Dispose()
        //{
        //    _context.Dispose();
        //}
    }
}
