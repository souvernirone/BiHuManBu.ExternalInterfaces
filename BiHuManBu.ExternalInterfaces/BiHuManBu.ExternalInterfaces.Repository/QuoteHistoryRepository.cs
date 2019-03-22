using log4net;
using System.Collections.Generic;
using System.Data;
using BiHuManBu.StoreFront.Infrastructure.DbHelper;
using BiHuManBu.ExternalInterfaces.Infrastructure.MySqlDbHelper;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Models.PartialModels;
using System.Configuration;
using BiHuManBu.ExternalInterfaces.Models.IRepository;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class QuoteHistoryRepository: IQuoteHistoryRepository
    {
        private static readonly string DBConnection = ConfigurationManager.ConnectionStrings["QuoteHistory"].ConnectionString;
        private readonly MySqlHelper _helpMain = new MySqlHelper(DBConnection);
        private ILog logError;

        public QuoteHistoryRepository()
        {
        }

        public List<bx_quote_history> GetByBuid(long buid, long groupspan)
        {
            var sql = string.Format("select id,b_uid,groupspan,licenseno,source,agent,lastbizdate,lastforcedate,quotestatus,submitstatus,savequote,quoteresult,quotereq,submitinfo,createtime,updatetime from bx_quote_history where b_uid={0} and groupspan={1} ", buid, groupspan);
            return _helpMain.ExecuteDataTable(CommandType.Text, sql).ToList<bx_quote_history>().ToList();
        }
    }
}
