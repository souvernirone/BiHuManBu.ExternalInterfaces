using System;
using System.Data.Entity.Migrations;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Models;
using log4net;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class SmsOrderRepository : ISmsOrderRepository
    {
        private ILog logError;

        public SmsOrderRepository()
        {
            logError = LogManager.GetLogger("ERROR");
        }

        public bx_sms_order Add(bx_sms_order bxSmsOrder)
        {
            bx_sms_order model = new bx_sms_order();
            try
            {
                model = DataContextFactory.GetDataContext().bx_sms_order.Add(bxSmsOrder);
                DataContextFactory.GetDataContext().SaveChanges();
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }

            return model;
        }
        public int Update(bx_sms_order bxSmsOrder)
        {
            int count = 0;
            try
            {
                DataContextFactory.GetDataContext().bx_sms_order.AddOrUpdate(bxSmsOrder);
                count = DataContextFactory.GetDataContext().SaveChanges();
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return count;
        }

        public bx_sms_order Find(int orderId)
        {
            bx_sms_order smsOrder = new bx_sms_order();
            try
            {
                smsOrder = DataContextFactory.GetDataContext().bx_sms_order.FirstOrDefault(x => x.Id == orderId);
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return smsOrder;
        }

        /// <summary>
        /// 根据OrderNum获取对象
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public bx_sms_order Find(string orderNum)
        {
            bx_sms_order smsOrder = new bx_sms_order();
            try
            {
                smsOrder = DataContextFactory.GetDataContext().bx_sms_order.FirstOrDefault(x => x.OrderNum == orderNum);
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return smsOrder;
        }

    }
}
