using System;
using System.Data.Entity.Migrations;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Models;
using log4net;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class CarRenewalRepository : ICarRenewalRepository
    {
        private ILog logError;
        public bx_car_renewal FindByLicenseno(string licenseno)
        {
            return
                DataContextFactory.GetDataContext().bx_car_renewal.Where(x => x.LicenseNo == licenseno)
                    .OrderByDescending(x => x.LastBizEndDate)
                    .FirstOrDefault();
        }

        public bx_car_renewal FindCarRenewal(long buid)
        {
            try
            {
                var list = from c in DataContextFactory.GetDataContext().bx_car_renewal
                           join p in DataContextFactory.GetDataContext().bx_userinfo_renewal_index
                               on c.Id equals p.car_renewal_id
                           where p.b_uid == buid
                           select c;
                return list.FirstOrDefault();
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return new bx_car_renewal();
        }

        public int Add(bx_car_renewal carRenewal)
        {
            int num = 0;
            try
            {
                DataContextFactory.GetDataContext().bx_car_renewal.AddOrUpdate(carRenewal);
                num = DataContextFactory.GetDataContext().SaveChanges();
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return num;
        }

        /// <summary>
        /// 获取车辆使用性质
        /// </summary>
        /// <param name="buid"></param>
        /// <returns></returns>
        public int GetCarUsedType(long buid)
        {
            int usedType = 0;
            try
            {
                string strsql = string.Format("SELECT renewal.carusedtype FROM bx_car_renewal AS renewal RIGHT JOIN bx_userinfo_renewal_index AS reindex ON reindex.car_renewal_id = renewal.id WHERE reindex.b_uid = {0}", buid);
                int? usedTypeTemp = DataContextFactory.GetDataContext().Database.SqlQuery<int?>(strsql).FirstOrDefault();
                usedType = usedTypeTemp ?? 0;
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return usedType;
        }

    }
}
