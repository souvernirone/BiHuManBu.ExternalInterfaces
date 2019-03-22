﻿using System;
using System.Collections.Generic;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.ReportModel;
using log4net;
using System.Data.Entity.Migrations;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class QuoteResultRepository : IQuoteResultRepository
    {
        private ILog logInfo = LogManager.GetLogger("INFO");
        private ILog logError = LogManager.GetLogger("ERROR");
        public bx_quoteresult GetQuoteResultByBuid(long buid, int source)
        {
            bx_quoteresult quoteresult = new bx_quoteresult();
            try
            {
                quoteresult = DataContextFactory.GetDataContext().bx_quoteresult.FirstOrDefault(x => x.B_Uid == buid && x.Source == source);

            }
            catch (Exception ex)
            {

                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return quoteresult;
        }
        public bx_quoteresult GetQuoteResultByBuid(long buid)
        {
            bx_quoteresult quoteresult = new bx_quoteresult();
            try
            {
                quoteresult = DataContextFactory.GetDataContext().bx_quoteresult.FirstOrDefault(x => x.B_Uid == buid);

            }
            catch (Exception ex)
            {

                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return quoteresult;
        }
        /// <summary>
        /// 获取到报价返回信息列表
        /// </summary>
        /// <param name="buid"></param>
        /// <returns></returns>
        public List<bx_quoteresult> GetQuoteResultList(long buid)
        {
            var list = new List<bx_quoteresult>();
            try
            {
                list = DataContextFactory.GetDataContext().bx_quoteresult.Where(x => x.B_Uid == buid).ToList();
            }
            catch (Exception ex)
            {

                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return list;
        }

        /// <summary>
        /// 根据buid获取商业险和交强险开始时间
        /// </summary>
        /// <param name="buid"></param>
        /// <returns></returns>
        public InsuranceStartDate GetStartDate(long buid)
        {
            var model = new InsuranceStartDate();
            try
            {
                string strSql = string.Format("SELECT MAX(BizStartDate) AS BizStartDate,MAX(ForceStartDate) AS ForceStartDate FROM bx_quoteresult WHERE b_uid={0}", buid);
                model = DataContextFactory.GetDataContext().Database.SqlQuery<InsuranceStartDate>(strSql).FirstOrDefault();
                //var list =
                //    from qr in DataContextFactory.GetDataContext().bx_quoteresult
                //    where qr.B_Uid == buid
                //    select new InsuranceStartDate()
                //    {
                //        BizStartDate = qr.BizStartDate,
                //        ForceStartDate = qr.ForceStartDate
                //    };
                //model = list.FirstOrDefault();
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return model;
        }
        public int Update(bx_quoteresult item)
        {
            int count = 0;
            try
            {
                DataContextFactory.GetDataContext().bx_quoteresult.AddOrUpdate(item);
                count = DataContextFactory.GetDataContext().SaveChanges();
            }
            catch (Exception ex)
            {
                ILog logError = LogManager.GetLogger("ERROR");
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return count;
        }
    }
}