﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BihuManBu.MonitorPlatform.Controllers
{
    public class HighChartsController : Controller
    {
        //
        // GET: /HighCharts/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /HighCharts/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /HighCharts/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /HighCharts/Create

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
        // GET: /HighCharts/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /HighCharts/Edit/5

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
        // GET: /HighCharts/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /HighCharts/Delete/5

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

        public JsonResult GetJson()
        {
            var data =
                "{'twitter':[['2012-11-01',289],['2012-11-02',201],['2012-11-03',276],['2012-11-04',434],['2012-11-05',331],['2012-11-06',241],['2012-11-07',217],['2012-11-08',220],['2012-11-09',220],['2012-11-10',331],['2012-11-11',321],['2012-11-12',217],['2012-11-13',362],['2012-11-14',465],['2012-11-15',465],['2012-11-16',465],['2012-11-17',331],['2012-11-18',507],['2012-11-19',294],['2012-11-20',331],['2012-11-21',368],['2012-11-22',279],['2012-11-23',496],['2012-11-24',248],['2012-11-25',368],['2012-11-26',465],['2012-11-27',321],['2012-11-28',496],['2012-11-29',414],['2012-11-30',225]],'facebook':[['2012-11-01',27],['2012-11-02',55],['2012-11-03',54],['2012-11-04',31],['2012-11-05',169],['2012-11-06',36],['2012-11-07',93],['2012-11-08',69],['2012-11-09',36],['2012-11-10',27],['2012-11-11',186],['2012-11-12',41],['2012-11-13',40],['2012-11-14',165],['2012-11-15',82],['2012-11-16',147],['2012-11-17',42],['2012-11-18',80],['2012-11-19',147],['2012-11-20',172],['2012-11-21',36],['2012-11-22',96],['2012-11-23',46],['2012-11-24',41],['2012-11-25',82],['2012-11-26',72],['2012-11-27',120],['2012-11-28',57],['2012-11-29',48],['2012-11-30',48]]}";
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}