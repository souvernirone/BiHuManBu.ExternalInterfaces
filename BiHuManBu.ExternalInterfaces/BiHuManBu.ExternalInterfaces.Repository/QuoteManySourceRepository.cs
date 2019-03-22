using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.IRepository;
using log4net;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class QuoteManySourceRepository : IQuoteManySourceRepository
    {
        private ILog logError = LogManager.GetLogger("ERROR");
        public bx_quote_many_source GetModel(int agent, int source)
        {
            bx_quote_many_source model = new bx_quote_many_source();
            try
            {
                model = DataContextFactory.GetDataContext().bx_quote_many_source.First(x => x.child_agent == agent && x.source == source);
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return model;
        }

        /// <summary>
        /// 取某个代理下多个source对应的记录
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="source">逗号分割的字符串</param>
        /// <returns></returns>
        public List<bx_quote_many_source> GetModels(int agent, string sources)
        {
            List<bx_quote_many_source> models = new List<bx_quote_many_source>();
            try
            {
                string strSql = string.Format("select * from bx_quote_many_source where child_agent={0} and source in ({1})", agent, sources);
                models = DataContextFactory.GetDataContext().Database.SqlQuery<bx_quote_many_source>(strSql).ToList();
                return models;
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return models;
        }

        public int Update(bx_quote_many_source model)
        {
            int count = 0;
            try
            {
                DataContextFactory.GetDataContext().bx_quote_many_source.AddOrUpdate(model);
                count = DataContextFactory.GetDataContext().SaveChanges();
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return count;
        }

        public long Add(bx_quote_many_source model)
        {
            long modelId = 0;
            try
            {
                var result = DataContextFactory.GetDataContext().bx_quote_many_source.Add(model);
                DataContextFactory.GetDataContext().SaveChanges();
                return modelId;
            }
            catch (Exception ex)
            {
                logError.Info("发生异常：" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return modelId;
        }
    }
}
