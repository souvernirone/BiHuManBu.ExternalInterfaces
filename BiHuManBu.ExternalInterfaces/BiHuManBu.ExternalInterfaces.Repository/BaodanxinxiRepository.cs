using System;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Web.Profile;
using BiHuManBu.ExternalInterfaces.Infrastructure;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.ReportModel;
using log4net;
using Microsoft.SqlServer.Server;
using MySql.Data.MySqlClient;


namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class BaodanxinxiRepository : IBaodanxinxiRepository
    {
        private ILog logError;

        public BaodanxinxiRepository()
        {
            logError = LogManager.GetLogger("ERROR");
        }
        public bj_baodanxinxi Add(bj_baodanxinxi baodanxinxi)
        {
            var item = new bj_baodanxinxi();
            try
            {
                item = DataContextFactory.GetDataContext().bj_baodanxinxi.Add(baodanxinxi);
                DataContextFactory.GetDataContext().SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        logError.Info(string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage));
                    }
                }
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
              
            }

            return item;
        }

        public bj_baodanxinxi Find(long bxid)
        {
            return DataContextFactory.GetDataContext().bj_baodanxinxi.FirstOrDefault(x => x.Id==bxid);
        }

        private DataSet GetDataSet(string sqls)
        {
            string connctionStr = DataContextFactory.GetDataContext().Database.Connection.ConnectionString;
            using (var connection = new MySqlConnection(connctionStr))
            {
                var ds = new DataSet();
                try
                {
                    connection.Open();
                    var command = new MySqlDataAdapter(sqls, connection);
                    command.Fill(ds);
                }
                catch(Exception ex)
                {
                    logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
                }
                return ds;
            }
        }

        public BjdCorrelateViewModel Finds(long bxid)
        {
            var correlateViewModel = new BjdCorrelateViewModel();
            string sql =
                string.Format(
                    "SELECT * FROM bj_baodanxinxi WHERE Id = {0}; SELECT * FROM bj_baodanxianzhong WHERE BaoDanXinXiId = {0};SELECT * FROM bx_bj_union WHERE bx_id = {0};",
                    bxid);
            DataSet ds = GetDataSet(sql);
            string baodanxinxiStr = CommonHelper.TToJson(ds.Tables[0]);
            string baodanxianzhongStr = CommonHelper.TToJson(ds.Tables[1]);
            string bjUnionStr = CommonHelper.TToJson(ds.Tables[2]);

            correlateViewModel.Baodanxinxi =
                baodanxinxiStr.ToListT<bj_baodanxinxi>().FirstOrDefault();
            correlateViewModel.Baodanxianzhong =
                baodanxianzhongStr.ToListT<bj_baodanxianzhong>().FirstOrDefault();
            correlateViewModel.BjUnion =
                bjUnionStr.ToListT<bx_bj_union>().FirstOrDefault();

            return correlateViewModel;
        }


        public int Update(bj_baodanxinxi baodanxinxi)
        {
            int count = 0;
            try
            {
                 DataContextFactory.GetDataContext().bj_baodanxinxi.AddOrUpdate(baodanxinxi);
                count = DataContextFactory.GetDataContext().SaveChanges();
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);

            }
            return count;
        }
    }
}
