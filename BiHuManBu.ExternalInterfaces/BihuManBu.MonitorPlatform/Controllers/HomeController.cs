using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BihuManBu.MonitorPlatform.Models;
using MySql.Data.MySqlClient;

namespace BihuManBu.MonitorPlatform.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            eftestEntities ctx = new eftestEntities();

           //  var  students=   ctx.Database.SqlQuery<student>("select * from  student",new object()).ToList();

            //students.FirstOrDefault().Age = 89;
            //ctx.SaveChanges();

            //ctx.student.Where(x=>x.Name==EntityFunctions.AsNonUnicode())
            //var stus = ctx.Set<student>().SqlQuery("select * from  student", new object()).ToList();
            //stus.FirstOrDefault().Age = 89;
            //ctx.SaveChanges();

             var sql = "select Age from Student where Name = @Name and Age = @Age";

          //var tst=  ctx.Database.SqlQuery<int>(
          //      sql,
          //      new MySqlParameter("@Name", "fxp"),
          //      new MySqlParameter("@Age", 89)).ToList();



            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter()
                {
                    Direction = ParameterDirection.Input,
                    DbType = DbType.Int32,
                    Value = 1,
                    ParameterName = "@id"
                },
                new MySqlParameter()
                {
                   // Direction = ParameterDirection.Output,
                    DbType = DbType.Int32,
                    ParameterName = "@cnt"
                }
            };
         //var ttttt=   ((IObjectContextAdapter) ctx).ObjectContext.ExecuteStoreQuery<student>("call getage(@id,@cnt)",
         //       parameters.ToArray()).ToList();
        //    var str=  ctx.Database.SqlQuery<student>("call getage(@id,@cnt)", parameters.ToArray()).ToList();
            var list =
                ctx.ExecuteStoredProcedureList<student>("call getage(@id)", parameters.ToArray()).ToList();
            return View();
        }

        //
        // GET: /Home/Details/5

        public ActionResult Details(int id)
        {
            eftestEntities ctx = new eftestEntities();

            //  var  students=   ctx.Database.SqlQuery<student>("select * from  student",new object()).ToList();

            //students.FirstOrDefault().Age = 89;
            //ctx.SaveChanges();

            //ctx.student.Where(x=>x.Name==EntityFunctions.AsNonUnicode())
            //var stus = ctx.Set<student>().SqlQuery("select * from  student", new object()).ToList();
            //stus.FirstOrDefault().Age = 89;
            //ctx.SaveChanges();

            var sql = "select Age from Student where Name = @Name and Age = @Age";

            //var tst=  ctx.Database.SqlQuery<int>(
            //      sql,
            //      new MySqlParameter("@Name", "fxp"),
            //      new MySqlParameter("@Age", 89)).ToList();

            //var param = new MySqlParameter("@id", 1);

            //var list = ctx.Database.SqlQuery<int>("call getage(@id)", param).ToList();
            return View();
        }

        //
        // GET: /Home/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Home/Create

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
        // GET: /Home/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Home/Edit/5

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
        // GET: /Home/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Home/Delete/5

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
    }
}
