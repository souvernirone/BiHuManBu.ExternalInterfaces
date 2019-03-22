
using System;
using System.Collections.Generic;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Models;
using log4net;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class TransferRecordRepository : ITransferRecordRepository
    {
        private ILog logError;
        public TransferRecordRepository()
        {
            logError = LogManager.GetLogger("ERROR");
        }
        public long Add(bx_transferrecord record)
        {
            long recordid = 0;
            try
            {
                var rcd = DataContextFactory.GetDataContext().bx_transferrecord.Add(record);
                DataContextFactory.GetDataContext().SaveChanges();
                recordid = rcd.Id;
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return recordid;
        }

        public List<bx_transferrecord> FindListByBuidList(List<long> buids)
        {
            return DataContextFactory.GetDataContext().bx_transferrecord.Where(x => buids.Contains(x.BuId) && x.ToAgentId == 0).ToList();
        }

        public List<bx_transferrecord> FindByBuid(long buid)
        {
            return DataContextFactory.GetDataContext().bx_transferrecord.OrderByDescending(o => o.Id).Where(x => x.BuId == buid).ToList();
        }

        /// <summary>
        /// 取sa的第一条记录
        /// </summary>
        /// <param name="buid"></param>
        /// <returns></returns>
        public bx_transferrecord FindFirstSaByBuid(long buid)
        {
            return DataContextFactory.GetDataContext().bx_transferrecord.OrderBy(o=>o.Id).FirstOrDefault(x => x.BuId == buid && x.StepType==1);
        }
    }
}
