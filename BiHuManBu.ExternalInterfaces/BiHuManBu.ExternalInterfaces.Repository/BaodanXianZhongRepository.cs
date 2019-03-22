using System;
using System.Data.Entity.Migrations;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Models;
using log4net;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class BaodanXianZhongRepository : IBaodanXianZhongRepository
    {
        private ILog logError;

        public BaodanXianZhongRepository()
        {
            logError = LogManager.GetLogger("ERROR");

        }
        public bj_baodanxianzhong Add(bj_baodanxianzhong baodanxianzhong)
        {
            var item = new bj_baodanxianzhong();
            try
            {
                item = DataContextFactory.GetDataContext().bj_baodanxianzhong.Add(baodanxianzhong);
                DataContextFactory.GetDataContext().SaveChanges();
                return item;
            }
            catch (Exception ex)
            {

                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return item;
        }

        public bj_baodanxianzhong Find(long bxid)
        {
            return DataContextFactory.GetDataContext().bj_baodanxianzhong.FirstOrDefault(x => x.BaoDanXinXiId == bxid);
        }

        public int Update(bj_baodanxianzhong baodanxianzhong)
        {
            int count = 0;
            try
            {
                DataContextFactory.GetDataContext().bj_baodanxianzhong.AddOrUpdate(baodanxianzhong);
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
