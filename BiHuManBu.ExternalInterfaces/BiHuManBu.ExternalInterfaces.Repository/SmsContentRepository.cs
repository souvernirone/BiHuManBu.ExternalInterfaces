using System;
using System.Data.Entity.Migrations;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Models;
using log4net;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class SmsContentRepository : ISmsContentRepository
    {
        private ILog logError;

        public SmsContentRepository()
        {
            logError = LogManager.GetLogger("ERROR");
        }

        public bx_sms_account Find(int agent)
        {
            bx_sms_account smsAccount = new bx_sms_account();
            try
            {
                smsAccount = DataContextFactory.GetDataContext().bx_sms_account.FirstOrDefault(x => x.agent_id == agent);
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return smsAccount;
        }

        public int Add(bx_sms_account bxSmsAccount)
        {
            int smsAccountId = 0;
            try
            {
                var t = DataContextFactory.GetDataContext().bx_sms_account.Add(bxSmsAccount);
                DataContextFactory.GetDataContext().SaveChanges();
                smsAccountId = t.id;
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
                smsAccountId = 0;
            }
            return smsAccountId;
        }

        public int Update(bx_sms_account bxSmsAccount)
        {
            int count = 0;
            try
            {
                DataContextFactory.GetDataContext().bx_sms_account.AddOrUpdate(bxSmsAccount);
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
