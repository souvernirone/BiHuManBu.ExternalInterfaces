using System;
using System.Collections.Generic;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Models;
using log4net;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class DeviceDetailRepository : IDeviceDetailRepository
    {
        private ILog logError;

        public DeviceDetailRepository()
        {
            logError = LogManager.GetLogger("ERROR");
        }
        public List<bx_devicedetail> FindList(long buid)
        {
            return DataContextFactory.GetDataContext().bx_devicedetail.Where(x => x.b_uid == buid).ToList();
        }

        public int Delete(long buid)
        {
            return DataContextFactory.GetDataContext().Database.ExecuteSqlCommand("delete from  bx_devicedetail where  b_uid=" + buid);
        }

        public long Add(bx_devicedetail devicedetail)
        {
            long id = 0;
            try
            {
                var d = DataContextFactory.GetDataContext().bx_devicedetail.Add(devicedetail);
                DataContextFactory.GetDataContext().SaveChanges();
                id = d.id;

            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }

            return id;
        }

    }
}
