using System;
using System.Data.Entity.Migrations;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Models;
using log4net;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class UserinfoRenewalInfoRepository : IUserinfoRenewalInfoRepository
    {
        private ILog logError = LogManager.GetLogger("ERROR");

        public int Add(bx_userinfo_renewal_info bxWorkOrder)
        {
            int workOrderId = 0;
            try
            {
                var t = DataContextFactory.GetDataContext().bx_userinfo_renewal_info.Add(bxWorkOrder);
                DataContextFactory.GetDataContext().SaveChanges();
                workOrderId = t.id;
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
                workOrderId = 0;
            }
            return workOrderId;
        }
        public int Update(bx_userinfo_renewal_info bxWorkOrder)
        {
            int count = 0;
            try
            {
                DataContextFactory.GetDataContext().bx_userinfo_renewal_info.AddOrUpdate(bxWorkOrder);
                count = DataContextFactory.GetDataContext().SaveChanges();
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return count;
        }
        public bx_userinfo_renewal_info FindById(int workOrderId)
        {
            var bxWorkOrder = new bx_userinfo_renewal_info();
            try
            {
                bxWorkOrder = DataContextFactory.GetDataContext().bx_userinfo_renewal_info.FirstOrDefault(x => x.id == workOrderId);
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return bxWorkOrder;
        }
        public bx_userinfo_renewal_info FindByBuid(long buid)
        {
            var bxWorkOrder = new bx_userinfo_renewal_info();
            try
            {
                bxWorkOrder = DataContextFactory.GetDataContext().bx_userinfo_renewal_info.OrderByDescending(o => o.create_time).FirstOrDefault(x => x.b_uid == buid);
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return bxWorkOrder;
        }
        public bx_userinfo_renewal_info FindByBuidAsync(long buid)
        {
            var bxWorkOrder = new bx_userinfo_renewal_info();
            try
            {
                bxWorkOrder = new EntityContext().bx_userinfo_renewal_info.OrderByDescending(o => o.create_time).FirstOrDefault(x => x.b_uid == buid);
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return bxWorkOrder;
        }
    }
}
