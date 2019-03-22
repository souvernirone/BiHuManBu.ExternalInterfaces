using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BiHuManBu.ExternalInterfaces.Models;
using log4net;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class HistoryContractRepository : IHistoryClaimRepository
    {
        private ILog logError = LogManager.GetLogger("ERROR");
        public int Add(List<bx_history_contract> claims)
        {
            int insertCount = 0;
            try
            {
                StringBuilder insertSql = new StringBuilder();
                insertSql.Append("insert bx_history_contract(LicenseNo,Enddate,InsureCompanyName,IsCommerce,PolicyNo,Strdate) values");
                int recordCount = claims.Count;
                int currentCount = 1;
                foreach (bx_history_contract claim in claims)
                {

                    insertSql.Append(string.Format("('{0}','{1}','{2}',{3},'{4}','{5}')", claim.LicenseNo, claim.Enddate, claim.InsureCompanyName, claim.IsCommerce, claim.PolicyNo, claim.Strdate));
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
                logError.Info("插入理赔记录发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return insertCount;
        }

        public List<bx_history_contract> FindList(string licenseno)
        {
            return DataContextFactory.GetDataContext().bx_history_contract.Where(x => x.LicenseNo == licenseno).ToList();
        }

        public int Remove(string licenseno)
        {
            var deleteSql = "delete  from  bx_history_contract where  licenseno='" + licenseno + "'";
            return DataContextFactory.GetDataContext().Database.ExecuteSqlCommand(deleteSql);
        }

    }
}
