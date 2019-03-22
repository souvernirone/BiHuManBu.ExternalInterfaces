using System;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.ReportModel;
using log4net;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class LastInfoRepository : ILastInfoRepository
    {
        private ILog logInfo = LogManager.GetLogger("INFO");
        private ILog logError = LogManager.GetLogger("ERROR");
        public bx_lastinfo GetByBuid(long buid)
        {
            bx_lastinfo lastinfo = new bx_lastinfo();
            try
            {
                lastinfo = DataContextFactory.GetDataContext().bx_lastinfo.FirstOrDefault(x => x.b_uid == buid);
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return lastinfo;
        }

        /// <summary>
        /// 根据buid获取上一年商业险和交强险到期时间
        /// </summary>
        /// <param name="buid"></param>
        /// <returns></returns>
        public InsuranceEndDate GetEndDate(long buid)
        {
            var model = new InsuranceEndDate();
            try
            {
                var list = from li in DataContextFactory.GetDataContext().bx_lastinfo
                           where li.b_uid == buid
                           select new InsuranceEndDate
                           {
                               LastBusinessEndDdate = li.last_business_end_date,
                               LastForceEndDdate = li.last_end_date
                           };
                model = list.FirstOrDefault();
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return model;
        }

        /// <summary>
        /// 根据buid获取上一年商业险和交强险到期时间
        /// </summary>
        /// <param name="buid"></param>
        /// <returns></returns>
        public InsuranceEndDateAndClaim GetEndDateAndClaim(long buid)
        {
            var model = new InsuranceEndDateAndClaim();
            try
            {
                var list = from li in DataContextFactory.GetDataContext().bx_lastinfo
                           where li.b_uid == buid
                           select new InsuranceEndDateAndClaim
                           {
                               LastBusinessEndDdate = li.last_business_end_date,
                               LastForceEndDdate = li.last_end_date,
                               ClaimCount = li.last_year_claimtimes ?? 0,
                               BizClaimCount = li.LastYearBizClaimTimes ?? 0,
                               ForceClaimCount = li.LastYearForceClaimTimes ?? 0
                           };
                model = list.FirstOrDefault();
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return model;
        }

    }
}
