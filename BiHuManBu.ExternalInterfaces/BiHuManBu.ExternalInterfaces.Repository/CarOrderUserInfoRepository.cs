using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Models;
using log4net;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class CarOrderUserInfoRepository : ICarOrderUserInfoRepository
    {
        private ILog logInfo = LogManager.GetLogger("INFO");
        private ILog logError = LogManager.GetLogger("ERROR");

        public bx_order_userinfo Find(int id)
        {
            bx_order_userinfo userinfo = new bx_order_userinfo();
            try
            {
                userinfo = DataContextFactory.GetDataContext().bx_order_userinfo.First(x => x.Id == id);
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return userinfo;

        }

        public bx_order_userinfo FindByBuid(long buid)
        {
            bx_order_userinfo userinfo = new bx_order_userinfo();
            try
            {
                userinfo = DataContextFactory.GetDataContext().bx_order_userinfo.AsNoTracking().FirstOrDefault(x => x.Id == buid);
            }
            catch (Exception ex)
            {

                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return userinfo;
        }

        public bx_order_userinfo FindByOpenIdAndLicense(string openid, string licenseno)
        {
            bx_order_userinfo tt = new bx_order_userinfo();
            try
            {
                tt = DataContextFactory.GetDataContext().bx_order_userinfo.FirstOrDefault(x => x.OpenId == openid && x.LicenseNo == licenseno);
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }

            return tt;
        }

        public bx_order_userinfo FindByOpenIdAndLicense(string openid, string licenseno, string agent)
        {
            bx_order_userinfo tt = new bx_order_userinfo();
            try
            {
                tt = DataContextFactory.GetDataContext().bx_order_userinfo.FirstOrDefault(x => x.OpenId == openid && x.LicenseNo == licenseno && x.Agent == agent);
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }

            return tt;
        }

        public long Add(bx_order_userinfo userinfo)
        {
            bx_order_userinfo item = new bx_order_userinfo();
            try
            {
                item = DataContextFactory.GetDataContext().bx_order_userinfo.Add(userinfo);
                var returnResult = DataContextFactory.GetDataContext().SaveChanges();
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }

            return item.Id;
        }

        public int Update(bx_order_userinfo userinfo)
        {
            int count = 0;
            try
            {
                DataContextFactory.GetDataContext().bx_order_userinfo.AddOrUpdate(userinfo);
                count = DataContextFactory.GetDataContext().SaveChanges();
            }
            catch (Exception ex)
            {

                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return count;
        }

        public List<bx_order_userinfo> FindByAgentAndLicense(bool isAgent, string strPass, string LicenseNo, int PageSize, int CurPage)
        {
            List<bx_order_userinfo> listuserinfo = new List<bx_order_userinfo>();
            try
            {
                listuserinfo = DataContextFactory.GetDataContext().bx_order_userinfo
                    .Where(i => (isAgent ? (i.Agent.Equals(strPass)) : (i.OpenId.Equals(strPass)))
                        && (string.IsNullOrEmpty(LicenseNo) ? true : i.LicenseNo.Contains(LicenseNo))
                        ).ToList();
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return listuserinfo.OrderByDescending(i => i.Id).Take(PageSize * CurPage).Skip(PageSize * (CurPage - 1)).ToList();
        }

    }
}
