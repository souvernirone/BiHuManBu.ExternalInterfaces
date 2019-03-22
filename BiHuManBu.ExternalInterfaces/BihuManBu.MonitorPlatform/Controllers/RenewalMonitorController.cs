using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using BihuManBu.MonitorPlatform.Models;
using BiHuManBu.Redis;
using log4net;

namespace BihuManBu.MonitorPlatform.Controllers
{
    public class RenewalMonitorController : Controller
    {
        ILog _logError = LogManager.GetLogger("ERROR");

        List<string> _statisticsGroup = new List<string>()
            {
                "statistics:second:",
                "statistics:minute:",
                "statistics:hour:",
                "statistics:day:",
                "statistics:week"
            };

        public ActionResult SecList()
        {
            return View();
        }

        public JsonResult GetNewsetPerSec()
        {
            try
            {
                using (var client = RedisManager.GetClient())
                {
                    var dateFormat = DateTime.Now.AddSeconds(-1).ToString("yyyy/MM/dd/HH/mm/ss");
                    var dataName = string.Format("{0}{1}", "statistics:second:", dateFormat);
                    //var mincollect = client.GetAllItemsFromList("statistics:minute:");


                    var dict = client.GetAllWithScoresFromSortedSet(dataName);

                    var values = dict.Sum(x => x.Value);
                    return Json(values, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {

                _logError.Info("seconds-list发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MinList()
        {
            
           
            return View();
        }

        public JsonResult GetMinList()
        {
            
          
            var groupResults = new Dictionary<string, double>();
            try
            {
                using (var client = RedisManager.GetClient())
                {
                    var mincollect = client.GetAllItemsFromList("statistics:minute:");

                    foreach (string item in mincollect)
                    {
                        var dict = client.GetAllWithScoresFromSortedSet(item);
                        var temp = item.Split(':');
                        var time = temp[2].Split('/');
                        var formatData = string.Format("{0}-{1}-{2} {3}:{4}:00", time[0], time[1], time[2], time[3], time[4]);
                        //var formatData = string.Format("{0}{1}", time[3], time[4]);
                        if (groupResults.ContainsKey(formatData))
                        {
                            groupResults[formatData] = dict.Sum(x => x.Value);
                        }
                        else
                        {
                             groupResults.Add(formatData, dict.Sum(x => x.Value));
                        }

                    }

                }
            }
            catch (Exception ex)
            {

                _logError.Info("minutes-list发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }

            List<StatisticsUnit> list = new List<StatisticsUnit>();
           
            foreach (var result in groupResults)
            {
               list.Add(new StatisticsUnit()
               {
                   Tick = result.Key,
                   Times = result.Value
               });
            }
            return Json(list.Take(10), JsonRequestBehavior.AllowGet);
            //return Json(null);
        }

        public JsonResult GetNewset()
        {

            var groupResults = new Dictionary<string, double>();
            using (var client = RedisManager.GetClient())
            {
                var dateFormat = DateTime.Now.AddMinutes(-1).ToString("yyyy/MM/dd/HH/mm");
                var dataName = string.Format("{0}{1}", "statistics:minute:", dateFormat);
                //var mincollect = client.GetAllItemsFromList("statistics:minute:");


                var dict = client.GetAllWithScoresFromSortedSet(dataName);

                var values = dict.Sum(x => x.Value);
                return Json(values, JsonRequestBehavior.AllowGet);

            }
            //var groupResults = new Dictionary<string, double>();
            //using (var client = RedisManager.GetClient())
            //{
            //    var mincollect = client.GetAllItemsFromList("statistics:minute:");
            //    var item = mincollect.LastOrDefault();
               
            //    var dict = client.GetAllWithScoresFromSortedSet(item);
            //    var temp = item.Split(':');
            //    var time = temp[2].Split('/');
            //    var formatData = string.Format("{0}{1}{2} {3}:{4}", time[0], time[1], time[2], time[3], time[4]);
            //    groupResults.Add(formatData, dict.Sum(x => x.Value));
            //}
            //return Json(groupResults, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /RenewalMonitory/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /RenewalMonitory/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /RenewalMonitory/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /RenewalMonitory/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /RenewalMonitory/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /RenewalMonitory/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /RenewalMonitory/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /RenewalMonitory/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        public JsonResult Test()
        {
            var t = "[[1,2,3,4,5,6,67,7]]";
            return Json(t, JsonRequestBehavior.AllowGet);
        }


        public class StatisticsUnit
        {
            public string Tick { get; set; }
            public double Times { get; set; }
        }
    }
}
