using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BiHuManBu.ExternalInterfaces.Repository;
using BiHuManBu.ExternalInterfaces.Services;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.Redis;
using log4net;
using ServiceStack.Text;

namespace BihuManBu.MS.Controllers
{
    public class HomeController : Controller
    {
        private ILog loginfo = LogManager.GetLogger("INFO");
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RenewalSta()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetRenewalStaData()
        {
            //Random rd = new Random();
            //using (var cli2 = RedisManager.GetClient())
            //{
            //    using (var tran = cli2.CreatePipeline())
            //    {
            //        int agent = rd.Next();
            //        var statisticsName = string.Format("sta-{0}", agent);
            //        var statisticsTimeSpan = string.Format("staGeneal-{0}", DateTime.Now.ToString("yyyy-MM-ddHH:mm"));
            //        tran.QueueCommand(p =>
            //        {
            //            p.IncrementItemInSortedSet(statisticsTimeSpan, agent.ToString(), 1.0);
            //            p.ExpireEntryAt(statisticsTimeSpan, DateTime.Now.AddMinutes(1));
            //            p.IncrementValueInHash(statisticsName, "xb", 1);
            //            p.ExpireEntryAt(statisticsName, DateTime.Now.AddMinutes(1));
            //        });
            //        tran.Flush();
            //    }
            //}
            //IDictionary<string, double> items;


            //using (var cli = RedisManager.GetClient())
            //{
                var statisticsTimeSpan = string.Format("staGeneal-{0}", DateTime.Now.ToString("yyyy-MM-ddHH:mm"));
                //items = cli.GetRangeWithScoresFromSortedSetByHighestScore(statisticsTimeSpan, 100000000, 0, 0, 50);// cli.GetAllItemsFromSortedSet(statisticsTimeSpan);
                //items = cli.GetAllWithScoresFromSortedSet(statisticsTimeSpan);
            //}
            List<RnewalStatistics> statisticses = new List<RnewalStatistics>();
            //foreach (KeyValuePair<string, double> item in items)
            //{
            //    statisticses.Add(new RnewalStatistics
            //    {
            //        Agent = item.Key,
            //        Counts = item.Value
            //    });
            //}
            //Random rd = new Random();
            //statisticses.Add(new RnewalStatistics()
            //{
            //    Agent = rd.Next(100).ToString(),
            //    Counts = rd.Next(300),
            //});
            //statisticses.Add(new RnewalStatistics()
            //{
            //    Agent = rd.Next(200).ToString(),
            //    Counts = rd.Next(400)
            //});
            return Json(statisticses,JsonRequestBehavior.AllowGet);
            
        }

        public ActionResult Test()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetMySqlMonitor()
        {
            string value = string.Empty;
            long question_value = 0;
            QuestionInfo info = new QuestionInfo();
            try
            {
             
            IMySqlMonitorService service = new MySqlMonitorService();
            var items = service.GetDate().ToList();



                if (Session["Question_total"] == null)
                {
                    Session["Question_total"] = 0;
                    info.QPS = 0;
                }
                else
                {
                    var questions = items.First(x => x.Variable_name == "Questions");
                    if (Session["Question_total"].ToString() == "0")
                    {
                        info.QPS = 0;
                    }
                    else
                    {
                        info.QPS = (long.Parse(questions.Value) - long.Parse(Session["Question_total"].ToString())) / 120.0;
                    }
                    Session["Question_total"] = questions.Value;
                }



                if (Session["Com_commit"] == null)
                {
                    Session["Com_commit"] = 0;
                    Session["Com_rollback"] = 0;
                    info.QTS = 0;
                }
                else
                {
                    var commit = items.First(x => x.Variable_name == "Com_commit");
                    var commit_roll = items.First(x => x.Variable_name == "Com_rollback");
                    if (Session["Com_commit"].ToString() == "0")
                    {
                        info.QTS = 0;
                    }
                    else
                    {
                        info.QTS = (long.Parse(commit.Value) - long.Parse(Session["Com_commit"].ToString())) + (long.Parse(commit_roll.Value) - long.Parse(Session["Com_rollback"].ToString())) / 120.0;
                    }
                    Session["Com_commit"] = commit.Value;
                    Session["Com_rollback"] = commit_roll.Value;

                }
               


                if (Session["Com_select"] == null)
                {
                    Session["Com_select"] = 0;
                    info.Select = 0;
                }
                else
                {
                    var select = items.First(x => x.Variable_name == "Com_select");
                    if (Session["Com_select"].ToString() == "0")
                    {
                        info.Select = 0;
                    }
                    else
                    {
                       
                        info.Select = (long.Parse(select.Value) - long.Parse(Session["Com_select"].ToString())) / 120.0;
                       
                    }
                    Session["Com_select"] = select.Value;
                }
               


                if (Session["Com_update"] == null)
                {
                    Session["Com_update"] = 0;
                    info.Update = 0;
                }
                else
                {
                    var update = items.First(x => x.Variable_name == "Com_update");
                    if (Session["Com_update"].ToString() == "0")
                    {
                        info.Update = 0;
                    }
                    else
                    {
                        
                        info.Update = (long.Parse(update.Value) - long.Parse(Session["Com_update"].ToString())) / 120.0;
                       
                    }
                    Session["Com_update"] = update.Value;
                    
                }
               

                if (Session["Com_insert"] == null)
                {
                    Session["Com_insert"] = 0;
                    info.Insert = 0;
                }
                else
                {
                    var insert = items.First(x => x.Variable_name == "Com_insert");
                    if (Session["Com_insert"].ToString() == "0")
                    {
                        info.Insert = 0;
                    }
                    else
                    {
                        
                        info.Insert = (long.Parse(insert.Value) - long.Parse(Session["Com_insert"].ToString())) / 120.0;
                       
                    }
                    Session["Com_insert"] = insert.Value;
                }



                if (Session["Com_delete"] == null)
                {
                    Session["Com_delete"] = 0;
                    info.Delete = 0;
                }
                else
                {
                    var delete = items.First(x => x.Variable_name == "Com_delete");
                    if (Session["Com_delete"].ToString() == "0")
                    {
                        info.Delete = 0;
                    }
                    else
                    {
                       
                        info.Delete = (long.Parse(delete.Value) - long.Parse(Session["Com_delete"].ToString())) / 120.0;
                        
                    }
                    Session["Com_delete"] = delete.Value;
                }

                //if (Session["Threads_cached"] == null)
                //{
                //    Session["Threads_cached"] = 0;
                //    info.ThreadsCached = 0;
                //}
                //else
                //{
                //    var threadcached = items.First(x => x.Variable_name == "Threads_cached");
                //    if (Session["Threads_cached"].ToString() == "0")
                //    {
                //        info.ThreadsCached = 0;
                //    }
                //    else
                //    {
                //        info.ThreadsCached = (long.Parse(threadcached.Value) -
                //                              long.Parse(Session["Threads_cached"].ToString()))/120.0;
                //    }

                //    Session["Threads_cached"] = threadcached.Value;
                //}
                info.ThreadsCached = long.Parse(items.First(x => x.Variable_name == "Threads_cached").Value);
                info.ThreadsConnected = long.Parse(items.First(x => x.Variable_name == "Threads_connected").Value);
                info.ThreadsCreated = long.Parse(items.First(x => x.Variable_name == "Threads_created").Value);
                info.ThreadsRunning = long.Parse(items.First(x => x.Variable_name == "Threads_running").Value);


                loginfo.Info("日志："+info.ToJson());
               
            }
            catch (Exception ex)
            {
                loginfo.Info("爆粗了:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }

            return Json(info, JsonRequestBehavior.AllowGet);
        }


        public ActionResult RefeshData()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetRefreshData()
        {
            List<int> test=new List<int>();
            test.Add(1);
            test.Add(2);
            test.Add(5);
            test.Add(6);
            return Json(test,JsonRequestBehavior.AllowGet);
        }

    }

    public class QuestionInfo
    {
        public double QPS { get; set; }
        public double QTS { get; set; }

        public double Update { get; set; }
        public double Insert { get; set; }
        public double Delete { get; set; }

        public double Select { get; set; }

        public double ThreadsCached { get; set; }

        public double ThreadsConnected { get; set; }
        public double ThreadsCreated { get; set; }
        public double ThreadsRunning { get; set; }
       
    }


    public class RnewalStatistics
    {
        public string Agent { get; set; }
        public double Counts { get; set; }
    }
    
}
