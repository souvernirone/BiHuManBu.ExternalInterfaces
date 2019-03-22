using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Models;
using log4net;
using System.Configuration;
using BiHuManBu.ExternalInterfaces.Infrastructure.MySqlDbHelper;
using System.Data;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class ReviewDetailRecordReponsitory : IReviewDetailRecordReponsitory
    {
        private static readonly string DBConnection = ConfigurationManager.ConnectionStrings["Analytics"].ConnectionString;
        private readonly MySqlHelper _helpMain = new MySqlHelper(DBConnection);
        public int Del(long buid)
        {
            var sql = string.Format(@"update bihu_analytics.tj_reviewdetail_record t set t.Deleted=1,t.UpdateTime=NOW() where BuId=
            {0}", buid);
            return _helpMain.ExecuteNonQuery(sql);
        }
    }
}
