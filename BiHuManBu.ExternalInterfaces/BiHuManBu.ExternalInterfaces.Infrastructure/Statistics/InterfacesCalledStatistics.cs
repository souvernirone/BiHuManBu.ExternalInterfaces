using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BiHuManBu.Redis;
using ServiceStack.Common.Extensions;
using ServiceStack.Logging;

namespace BiHuManBu.ExternalInterfaces.Infrastructure.Statistics
{
    public  class InterfacesCalledStatistics
    {
        public static void StatisticsCalled(int agent)
        {
            //ILog _errorLog = LogManager.GetLogger("ERROR");
            //string strAgent = agent.ToString();
            //List<string> statisticsGroup = new List<string>()
            //{
            //    "statistics:second:",
            //    "statistics:minute:",
            //    "statistics:hour:",
            //    "statistics:day:",
            //    "statistics:week"
            //};
            ////判断是否存在，首次创建的话，要设置过期时间
            //bool isSecExisits = false;
            //long secCount = 0;

            //bool isMinExists = false;
            //long minCount = 0;

            //bool isHourExisits = false;
            //long hourCount = 0;

            //bool isDayExisits = false;
            //long dayCount = 0;

            //var currentSecond = DateTime.Now.ToString("yyyy/MM/dd/HH/mm/ss");
            //var zsetSecName = string.Format("statistics:second:{0}", currentSecond);

            //var currentMinute = DateTime.Now.ToString("yyyy/MM/dd/HH/mm");
            //var zsetMinName = string.Format("statistics:minute:{0}", currentMinute);

            //var currentHour = DateTime.Now.ToString("yyyy/MM/dd/HH");
            //var zsetHourName = string.Format("statistics:hour:{0}", currentHour);


            //var currentDay = DateTime.Now.ToString("yyyy/MM/dd");
            //var zsetDayName = string.Format("statistics:day:{0}", currentDay);

            //using (var client = RedisManager.GetClient())
            //{
            //    isSecExisits = client.GetSortedSetCount(zsetSecName) > 0;
            //    //secCount = client.GetListCount("statistics:minute:");


            //    isMinExists = client.GetSortedSetCount(zsetMinName) > 0;
            //    minCount = client.GetListCount("statistics:minute:");

            //    isHourExisits = client.GetSortedSetCount(zsetHourName) > 0;
            //    hourCount = client.GetListCount("statistics:hour:");


            //    isDayExisits = client.GetSortedSetCount(zsetDayName) > 0;
            //    dayCount = client.GetListCount("statistics:day:");

            //    using (var tran = client.CreateTransaction())
            //    {
            //        try
            //        {
            //            #region seconds
            //            tran.QueueCommand(p =>
            //            {
            //                p.IncrementItemInSortedSet(zsetSecName, strAgent, 1);
            //            });

            //            if (!isSecExisits)
            //            {
            //                tran.QueueCommand(p => p.ExpireEntryAt(zsetSecName, DateTime.Now.AddMinutes(2)));
            //            }

            //            #endregion

            //            #region miniutes
            //            tran.QueueCommand(p =>
            //            {
            //                p.IncrementItemInSortedSet(zsetMinName, strAgent, 1);
            //            });

            //            if (!isMinExists)
            //            {
            //                tran.QueueCommand(p => p.ExpireEntryAt(zsetMinName, DateTime.Now.AddHours(1)));

            //                tran.QueueCommand(p => p.PushItemToList("statistics:minute:", zsetMinName));

            //                if (minCount >= 60)
            //                {
            //                    tran.QueueCommand(p => p.TrimList("statistics:minute:", -60, -1));
            //                }
            //            }
            //            #endregion

            //            #region hour
            //            tran.QueueCommand(p => p.IncrementItemInSortedSet(zsetHourName, strAgent, 1));
            //            if (!isHourExisits)
            //            {
            //                tran.QueueCommand(p => p.ExpireEntryAt(zsetHourName, DateTime.Now.AddDays(1)));
            //                tran.QueueCommand(p => p.PushItemToList("statistics:hour:", zsetHourName));
            //                if (hourCount >= 24)
            //                {
            //                    tran.QueueCommand(p => p.TrimList("statistics:hour:", -24, -1));
            //                }
            //            }
            //            #endregion

            //            #region day
            //            tran.QueueCommand(p => p.IncrementItemInSortedSet(zsetDayName, strAgent, 1));
            //            if (!isDayExisits)
            //            {
            //                tran.QueueCommand(p => p.ExpireEntryAt(zsetDayName, DateTime.Now.AddDays(30)));
            //                tran.QueueCommand(p => p.PushItemToList("statistics:hour:", zsetHourName));
            //                if (dayCount >= 30)
            //                {
            //                    tran.QueueCommand(p => p.TrimList("statistics:hour:", -30, -1));
            //                }
            //            }
            //            #endregion
            //            tran.Commit();
            //        }
            //        catch (Exception ex)
            //        {
            //            _errorLog.Info("接口调用统计缓存发生异常：" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            //            tran.Rollback();

            //        }
            //    }
            //}
        }
    }
}
