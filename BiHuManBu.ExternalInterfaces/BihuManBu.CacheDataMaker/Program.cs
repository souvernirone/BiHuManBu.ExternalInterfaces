using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BihuManBu.CacheDataMaker.RedisOperator;
using BiHuManBu.ExternalInterfaces.Models;
using ServiceStack.Common;
using ServiceStack.Common.Extensions;
using ServiceStack.Redis;

namespace BihuManBu.CacheDataMaker
{
    class Program
    {


        static void Main(string[] args)
        {
            int k = 0;
            try
            {
                for (int i = 0; i < 100000000; i++)
                {
                    k = i;

                    using (var client = RedisManager.GetClient())
                    {
                        using (var pip = client.CreatePipeline())
                        {
                            pip.QueueCommand(p => p.AddItemToList("example" + i, "dsfsdtewtsfdfsssssssssssssssssssssssssssssssssssssssssssssssssss" + i));
                            pip.QueueCommand(p => p.AddItemToList("example" + i, "dsfsdtewtsfdfsssssssssssssssssssssssssssssssssssssssssssssssssss1" + i));
                            pip.Flush();

                        }
                    }
                    Console.WriteLine("序列："+i+"已经推入缓存");

                    if (i > 1000)
                    {
                        var cli= RedisManager.GetClient();
                        DateTime startTime = DateTime.Now;
                        var  t=  cli.GetItemFromList("example" + i,0);
                        DateTime stopTime = DateTime.Now;
                        TimeSpan elapsedTime = stopTime - startTime;
                        Console.WriteLine("in milliseconds:" + elapsedTime.TotalMilliseconds);
                        cli.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                var t = k;
                throw new Exception(ex.InnerException.ToString());
            }




        }






        //static void Main(string[] args)
        //{
        //    List<string> statisticsGroup = new List<string>()
        //    {
        //        "statistics:minute:",
        //        "statistics:hour:",
        //        "statistics:day:",
        //        "statistics:week"
        //    };
        //    //判断是否存在，首次创建的话，要设置过期时间
        //    bool isMinExists = false;
        //    long minCount = 0;

        //    bool isHourExisits = false;
        //    long hourCount = 0;

        //    bool isDayExisits = false;
        //    long dayCount = 0;

        //    var currentMinute = DateTime.Now.ToString("yyyy/MM/dd/HH/mm");
        //    var zsetMinName = string.Format("statistics:minute:{0}", currentMinute);

        //    var currentHour = DateTime.Now.ToString("yyyy/MM/dd/HH");
        //    var zsetHourName = string.Format("statistics:hour:{0}", currentHour);


        //    var currentDay = DateTime.Now.ToString("yyyy/MM/dd");
        //    var zsetDayName = string.Format("statistics:day:{0}", currentDay);

        //    using (var client = RedisManager.GetClient(14))
        //    {
        //        isMinExists = client.GetSortedSetCount(zsetMinName) > 0;
        //        minCount = client.GetListCount("statistics:minute:");

        //        isHourExisits = client.GetSortedSetCount(zsetHourName) > 0;
        //        hourCount = client.GetListCount("statistics:hour:");


        //        isDayExisits = client.GetSortedSetCount(zsetDayName) > 0;
        //        dayCount = client.GetListCount("statistics:day:");

        //        using (var tran = client.CreateTransaction())
        //        {
        //            try
        //            {
        //                #region miniutes
        //                tran.QueueCommand(p =>
        //                {
        //                    p.IncrementItemInSortedSet(zsetMinName, "102", 1);
        //                });
        //                if (!isMinExists)
        //                {
        //                    tran.QueueCommand(p => p.ExpireEntryAt(zsetMinName, DateTime.Now.AddHours(1)));

        //                    tran.QueueCommand(p => p.PushItemToList("statistics:minute:", zsetMinName));

        //                    if (minCount >= 6)
        //                    {
        //                        tran.QueueCommand(p => p.TrimList("statistics:minute:", -6, -1));
        //                    }
        //                }
        //                #endregion

        //                #region hour
        //                tran.QueueCommand(p => p.IncrementItemInSortedSet(zsetHourName, "102", 1));
        //                if (!isHourExisits)
        //                {
        //                    tran.QueueCommand(p => p.ExpireEntryAt(zsetHourName, DateTime.Now.AddDays(1)));
        //                    tran.QueueCommand(p => p.PushItemToList("statistics:hour:", zsetHourName));
        //                    if (hourCount >= 24)
        //                    {
        //                        tran.QueueCommand(p => p.TrimList("statistics:hour:", -24, -1));
        //                    }
        //                }
        //                #endregion

        //                #region day
        //                tran.QueueCommand(p => p.IncrementItemInSortedSet(zsetDayName, "102", 1));
        //                if (!isDayExisits)
        //                {
        //                    tran.QueueCommand(p => p.ExpireEntryAt(zsetDayName, DateTime.Now.AddDays(30)));
        //                    tran.QueueCommand(p => p.PushItemToList("statistics:hour:", zsetHourName));
        //                    if (dayCount >= 30)
        //                    {
        //                        tran.QueueCommand(p => p.TrimList("statistics:hour:", -30, -1));
        //                    }
        //                }
        //                #endregion

        //                tran.Commit();
        //            }
        //            catch (Exception)
        //            {
        //                tran.Rollback();
        //            }
        //        }
        //    }

        //}


        //static void Main(string[] args)
        //{
        //    for (int k = 0; k < 10; k++)
        //    {
        //        //using (var client=RedisManager.GetClient())
        //        //{
        //        //    using (var tran = client.CreateTransaction())
        //        //    {
        //        //        try
        //        //        {

        //        //            for (int i = 0; i < k; i++)
        //        //            {
        //        //                tran.QueueCommand(p =>
        //        //                {
        //        //                    p.Add("ttt" + i, i);
        //        //                });
        //        //            }

        //        //            tran.Commit();
        //        //        }
        //        //        catch (Exception ex)
        //        //        {
        //        //            tran.Rollback();
        //        //        }
        //        //    }
        //        //}

        //        using (var cli=RedisManager.GetClient())
        //        {
        //            using (var pipe=cli.CreatePipeline())
        //            {
        //                for (int i = 0; i < 10; i++)
        //                {
        //                    pipe.QueueCommand(p =>
        //                    {
        //                        p.Add("ttt" + i, i);
        //                    });
        //                }

        //                pipe.Flush();

        //            }
        //        }


        //    }



        //    //var client = RedisManager.GetClient();
        //    //while (true)
        //    //{
        //    //    try
        //    //    {
        //    //        client.Watch("ttt1");
        //    //       // client.Set("ttt1", 20000);
        //    //        using (var tran = client.CreateTransaction())
        //    //        {
        //    //            tran.QueueCommand(r=>r.Set("ki",111));
        //    //            tran.Commit();
        //    //        }

        //    //    }
        //    //    catch (RedisException ex)
        //    //    {

        //    //        break;
        //    //    }


        //    //}

        //    //int x = 0;
        //    //try
        //    //{
        //    //    for (int j = 0; j < 1000; j++)
        //    //    {
        //    //        x = j;
        //    //        Thread.Sleep(TimeSpan.FromSeconds(1));
        //    //        var s=RedisManager.GetClient().Get<string>("ttt" + j);
        //    //        Console.WriteLine("当前顺序:"+j+"  值是:");
        //    //    }
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    var err = ex.Message;

        //    //}



        //    //for (int k = 0; k < 1; k++)
        //    //{
        //    //    DoRedisString set = new DoRedisString();
        //    //    for (int i = 0; i < 10; i++)
        //    //    {
        //    //        set.Set("ttt" + i, i.ToString());
        //    //    }

        //    //        using (var tran = RedisManager.GetClient().CreateTransaction())
        //    //        {
        //    //            try
        //    //            {

        //    //                for (int i = 0; i < 10; i++)
        //    //                {
        //    //                    tran.QueueCommand(p =>
        //    //                    {
        //    //                        p.Add("ttt" + i, i);
        //    //                    });
        //    //                }




        //    //                tran.Commit();
        //    //            }
        //    //            catch (Exception ex)
        //    //            {
        //    //                tran.Rollback();
        //    //            }
        //    //        }



        //    //    using (var tran = RedisClient.NewFactoryFn().CreateTransaction())
        //    //    {
        //    //        try
        //    //        {

        //    //            for (int i = 0; i < 10000; i++)
        //    //            {
        //    //                tran.QueueCommand(p =>
        //    //                {
        //    //                    p.Add("ttt" + i, i);
        //    //                });
        //    //            }




        //    //            tran.Commit();

        //    //        }
        //    //        catch (Exception ex)
        //    //        {
        //    //            tran.Rollback();
        //    //        }
        //    //    }
        //    //}



        //    //using (var client=RedisManager.GetClient())
        //    //{

        //    //    #region 数据预加载  
        //    //    using (EntityContext context = new EntityContext())
        //    //    {
        //    //        //bx_message
        //    //        var messagefromDb = context.bx_message.ToList();
        //    //        foreach (var message in messagefromDb)
        //    //        {
        //    //            client.AddItemToSet("message:Agent", message.Agent_Id.ToString());
        //    //            client.AddItemToSet("messageIdentity:" + message.Agent_Id, message.Id.ToString());
        //    //            client.AddItemToSet("message:status:" + message.Msg_Status, message.Id.ToString());
        //    //            client.AddItemToSet("message:type:" + message.Msg_Type, message.Id.ToString());
        //    //            client.AddItemToSortedSet("message:sendtime:", message.Id.ToString(),
        //    //                message.Send_Time.HasValue ? message.Send_Time.Value.Ticks : 0);
        //    //            client.SetRangeInHash("messageItemHash:" + message.Id,
        //    //               new List<KeyValuePair<string, string>>
        //    //               {
        //    //                   new KeyValuePair<string, string>("licenseno",string.IsNullOrWhiteSpace(message.License_No)?string.Empty:message.License_No),
        //    //                   new KeyValuePair<string, string>("title",message.Title),
        //    //                   new KeyValuePair<string, string>("body",string.IsNullOrWhiteSpace(message.Body)?string.Empty:message.Body),
        //    //                   new KeyValuePair<string, string>("msg_type",message.Msg_Type.ToString()),
        //    //                   new KeyValuePair<string, string>("create_time",message.Create_Time.ToString()),
        //    //                   new KeyValuePair<string, string>("msg_status",message.Msg_Status.ToString()),
        //    //                   new KeyValuePair<string, string>("send_time",message.Send_Time.HasValue?message.Send_Time.Value.ToString():string.Empty),
        //    //                   new KeyValuePair<string, string>("create_agent_id",message.Create_Agent_Id.ToString()),
        //    //                   new KeyValuePair<string, string>("url",string.IsNullOrWhiteSpace(message.Url)?string.Empty:message.Url),
        //    //                   new KeyValuePair<string, string>("Buid",message.Buid.ToString())
        //    //               });
        //    //        }

        //    //       //bx_notice_xb
        //    //        var noticeFromDb = context.bx_notice_xb.ToList();
        //    //        foreach (bx_notice_xb xb in noticeFromDb)
        //    //        {
        //    //            client.AddItemToSet("notice:Agent", xb.agent_id.ToString());
        //    //            client.AddItemToSet("noticeIdentity:" + xb.agent_id, xb.id.ToString());
        //    //            client.AddItemToSet("notice:status:" + xb.stauts, xb.id.ToString());
        //    //            client.SetRangeInHash("noticeItemHash:" + xb.id,
        //    //                new List<KeyValuePair<string, string>>
        //    //                {
        //    //                    new KeyValuePair<string, string>("licenseno",xb.license_no),
        //    //                    new KeyValuePair<string, string>("status",xb.stauts.ToString()),
        //    //                    new KeyValuePair<string, string>("lastforceenddate",xb.last_force_end_date.HasValue?xb.last_force_end_date.Value.ToString():string.Empty),
        //    //                    new KeyValuePair<string, string>("lastbizenddate",xb.Last_biz_end_date.HasValue?xb.Last_biz_end_date.Value.ToString():string.Empty),
        //    //                    new KeyValuePair<string, string>("nextforcestartdate",xb.next_force_start_date.HasValue?xb.next_force_start_date.Value.ToString():string.Empty),
        //    //                    new KeyValuePair<string, string>("nextbizstartdate",xb.next_biz_start_date.HasValue?xb.next_biz_start_date.Value.ToString():string.Empty),
        //    //                    new KeyValuePair<string, string>("source",xb.source.ToString()),
        //    //                    new KeyValuePair<string, string>("days",xb.days.ToString()),
        //    //                    new KeyValuePair<string, string>("buid",xb.b_uid.ToString()),
        //    //                    new KeyValuePair<string, string>("daynum",xb.day_num.ToString())
        //    //                }
        //    //                );
        //    //        }

        //    //        //bx_consumer_review
        //    //        var consumerFromDb = context.bx_consumer_review.ToList();
        //    //        foreach (bx_consumer_review review in consumerFromDb)
        //    //        {
        //    //            client.AddItemToSet("consumer:Agent",review.operatorId.ToString());
        //    //            client.AddItemToSet("consumerIdentity:"+review.operatorId,review.id.ToString());
        //    //            client.AddItemToSet("consumer:status:"+review.status,review.id.ToString());
        //    //            client.SetRangeInHash("consumerItemHash:"+review.id,
        //    //                new List<KeyValuePair<string,string>>
        //    //                {
        //    //                    new KeyValuePair<string, string>("content",review.content??string.Empty),
        //    //                    new KeyValuePair<string, string>("status",review.status.ToString()),
        //    //                    new KeyValuePair<string, string>("buid",review.b_uid.ToString()),
        //    //                    new KeyValuePair<string, string>("operatorname",review.operatorName??string.Empty),
        //    //                    new KeyValuePair<string, string>("nextreveiwdate",review.next_review_date.HasValue?review.next_review_date.Value.ToString():string.Empty),
        //    //                    new KeyValuePair<string, string>("readstatus",review.read_status.ToString()),
        //    //                    new KeyValuePair<string, string>("resultstatus",review.result_status.ToString()),
        //    //                    new KeyValuePair<string, string>("intentioncompany",review.intentioncompany.HasValue?review.intentioncompany.ToString():string.Empty)
        //    //                });
        //    //        }


        //    //    }
        //    //    #endregion

        //    //    //var requestAgentId = 8696;

        //    //    //#region 获取数据 bx_message
        //    //    ////where 过滤
        //    //    //client.StoreDifferencesFromSet("intoMsgId:" + requestAgentId, "messageIdentity:" + requestAgentId, new string[] { "message:type:1", "message:type:2" });

        //    //    ////日期过滤
        //    //    //var stime = DateTime.Now.Date.Ticks;
        //    //    //var etime = DateTime.Now.AddDays(1).Date.Ticks;

        //    //    //var rs = client.GetRangeFromSortedSetByHighestScore("message:sendtime:", stime, etime);

        //    //    //var t = client.GetAllItemsFromSet("intoMsgId:" + requestAgentId);

        //    //    //t.IntersectWith(rs);

        //    //    ////bx_message 数据组合
        //    //    //var msgList = new List<bx_message>();
        //    //    //foreach (string s in t)
        //    //    //{
        //    //    //    var item = client.GetAllEntriesFromHash("messageItemHash:" + s);
        //    //    //    var v = item["licenseno"];//等等

        //    //    //}
        //    //    ////删除数据项目
        //    //    //client.ExpireEntryIn("intoMsgId:"+requestAgentId, TimeSpan.FromSeconds(1));
        //    //    //#endregion 

        //    //    //#region 获取数据  bx_notice
        //    //    ////where
        //    //    //var nt = client.GetIntersectFromSets(new[] {"noticeIdentity:" + requestAgentId, "notice:status:0"});

        //    //    //var noticeList = new List<bx_notice_xb>();
        //    //    //foreach (string s in nt)
        //    //    //{
        //    //    //    var item = client.GetAllEntriesFromHash("noticeItemHash:" + s);
        //    //    //    noticeList.Add(new bx_notice_xb()
        //    //    //    {
        //    //    //        license_no = item["licenseno"]
        //    //    //    });
        //    //    //}

        //    //    //#endregion

        //    //    //#region 获取数据region bx_consumer_review
        //    //    ////where  
        //    //    //var cr =
        //    //    //    client.GetIntersectFromSets(new string[] {"consumerIdentity:" + requestAgentId, "consumer:status:0"});
        //    //    //var consumerList = new List<bx_consumer_review>();
        //    //    //foreach (string s in cr)
        //    //    //{
        //    //    //    var item = client.GetAllEntriesFromHash("noticeItemHash:" + s);
        //    //    //    consumerList.Add(new bx_consumer_review
        //    //    //    {
        //    //    //        b_uid =int.Parse(item["buid"])
        //    //    //    });
        //    //    //}

        //    //    //#endregion

        //    //    #region 单元操作

        //    //    using (var trans=client.CreateTransaction())
        //    //    {
        //    //        try
        //    //        {
        //    //            trans.QueueCommand(p =>
        //    //            {
        //    //                client.AddItemToSet("message:Agent", "8696");
        //    //                client.AddItemToSet("messageIdentity:" + "8696", "1");
        //    //                client.AddItemToSet("message:status:" + "0", "1");
        //    //                client.AddItemToSet("message:type:" + "1", "1");
        //    //                client.AddItemToSortedSet("message:sendtime:", "1", 0);
        //    //                client.SetRangeInHash("messageItemHash:" + "1",
        //    //                   new List<KeyValuePair<string, string>>
        //    //               {
        //    //                   new KeyValuePair<string, string>("licenseno",string.Empty),

        //    //               });
        //    //            });
        //    //            trans.Commit();
        //    //        }
        //    //        catch (Exception ex)
        //    //        {

        //    //            trans.Rollback();
        //    //        }

        //    //    }
        //    //    #endregion

        //    //}
        //}
    }
}
