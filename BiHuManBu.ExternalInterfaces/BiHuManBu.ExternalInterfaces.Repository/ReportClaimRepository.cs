using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Models;
using log4net;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public  class ReportClaimRepository:IReportClaimRepository
    {
        //private ILog logInfo = LogManager.GetLogger("INFO");
        private ILog logError = LogManager.GetLogger("ERROR");
        public int Add(List<bx_report_claim> claims)
        {
            int insertCount = 0;
            try
            {
                StringBuilder insertSql = new StringBuilder();
                insertSql.Append("insert into  bx_report_claim(LicenseNo,AccidentPlace,AccidentPsss,DriverName,IsCommerce,IsOwners,IsThreecCar,ReportDate) values");
                int recordCount = claims.Count;
                int currentCount = 1;
                foreach (bx_report_claim claim in claims)
                {

                    insertSql.Append(string.Format("('{0}','{1}','{2}','{3}',{4},{5},{6},'{7}')", claim.LicenseNo,
                        claim.AccidentPlace, claim.AccidentPsss, claim.DriverName, claim.IsCommerce, claim.IsOwners,
                        claim.IsThreecCar, claim.ReportDate));
                    if (recordCount - currentCount > 0)
                    {
                        insertSql.Append(",");
                    }
                    currentCount++;
                }
                insertCount = DataContextFactory.GetDataContext().Database.ExecuteSqlCommand(insertSql.ToString());
               
            }
            catch (Exception ex)
            {
                logError.Info("插入报备记录发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return insertCount;
        }

        public List<bx_report_claim> FindList(string licenseno)
        {
            return DataContextFactory.GetDataContext().bx_report_claim.Where(x => x.LicenseNo == licenseno).ToList();
        }

        public int Remove(string licenseno)
        {
            var deleteSql = "delete  from  bx_report_claim where  licenseno='" + licenseno+"'";
            return DataContextFactory.GetDataContext().Database.ExecuteSqlCommand(deleteSql);
        }

    }
}
