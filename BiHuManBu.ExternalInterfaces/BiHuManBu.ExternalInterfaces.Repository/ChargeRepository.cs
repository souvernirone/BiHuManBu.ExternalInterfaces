using System;
using System.Data.Entity.Migrations;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Models;
using log4net;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class ChargeRepository : IChargeRepository
    {
        private ILog logError = LogManager.GetLogger("ERROR");
        public int Add(bx_charge charege)
        {
            var item = DataContextFactory.GetDataContext().bx_charge.Add(charege);
            DataContextFactory.GetDataContext().SaveChanges();
            return item.id;
        }

        public bx_charge Find(int agent, int busyKey)
        {
            return DataContextFactory.GetDataContext().bx_charge.FirstOrDefault(x => x.agent == agent && x.id == busyKey);
        }


        public int Update(bx_charge charege)
        {
            int count = 0;
            try
            {
                DataContextFactory.GetDataContext().bx_charge.AddOrUpdate(charege);
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
