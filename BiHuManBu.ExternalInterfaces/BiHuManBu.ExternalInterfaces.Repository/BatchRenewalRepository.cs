using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.IRepository;
using log4net;
using System;
using System.Linq;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class BatchRenewalRepository : IBatchRenewalRepository
    {
        private ILog logError = LogManager.GetLogger("ERROR");
        public BatchRenewalRepository()
        {
        }

        /// <summary>
        /// 根据Buid修改批量续保子表状态
        /// </summary>
        /// <param name="buId">userInfoId</param>
        /// <param name="itemStatus">状态</param>
        /// <returns>修改影响行数</returns>
        public int UpdateItemStatus(long buId, int itemStatus)
        {
            try
            {
                var sql = string.Format("UPDATE bx_batchrenewal_item SET ItemStatus={1},HistoryItemStatus={1},UpdateTime=NOW() WHERE BUId ={0}  AND IsDelete=0  ", buId, itemStatus); ;
                return DataContextFactory.GetDataContext().Database.ExecuteSqlCommand(sql);
            }
            catch (Exception ex)
            {
                logError.Error("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return 0;
        }
        public bx_batchrenewal_item GetItemByBuId(long buId)
        {
            return DataContextFactory.GetDataContext().bx_batchrenewal_item.Where(x => x.BUId == buId && x.IsDelete == 0 && x.IsNew == 1).FirstOrDefault();
        }
        public bx_batchrenewal_item GetItemByBuIdSync(long buId)
        {
            return new EntityContext().bx_batchrenewal_item.Where(x => x.BUId == buId && x.IsDelete == 0 && x.IsNew == 1).FirstOrDefault();
        }

    }
}
