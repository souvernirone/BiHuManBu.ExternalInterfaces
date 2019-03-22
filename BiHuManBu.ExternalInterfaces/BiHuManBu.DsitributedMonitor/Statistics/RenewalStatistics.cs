using System;
using BiHuManBu.Redis;

namespace BiHuManBu.DsitributedMonitor.Statistics
{
    public  class RenewalStatistics:IStatisticsService
    {
        //private ILog logError = LogManager.GetLogger("ERROR");
        public void Add(int agent)
        {
            //using (var cli = RedisManager.GetClient(0))
            //{
            //    try
            //    {
            //        using (var tran = cli.CreatePipeline())
            //        {
            //            var statisticsName = string.Format("sta-{0}", agent);
            //            var statisticsTimeSpan = string.Format("staGeneal-{0}", DateTime.Now.ToString("yyyy-MM-ddHH:mm"));
            //            tran.QueueCommand(p =>
            //            {
            //                p.IncrementItemInSortedSet(statisticsTimeSpan, agent.ToString(), 1.0);
                           
            //            });
            //            tran.QueueCommand(p =>
            //            {
            //                p.ExpireEntryAt(statisticsTimeSpan, DateTime.Now.AddMinutes(1));
            //            });
            //            tran.QueueCommand(p =>
            //            {
            //                p.IncrementValueInHash(statisticsName, "xb", 1);
            //            });
            //            tran.QueueCommand(p =>
            //            {
            //                p.ExpireEntryAt(statisticsName, DateTime.Now.AddMinutes(1));
            //            });
            //            tran.Flush();
            //        }

            //    }
            //    catch (Exception ex)
            //    {
            //        //logError.Info("InsertUserInfo清空缓存失败:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            //    }
                
               
               
            //}
        }
    }
}
