using System;
using System.Collections.Generic;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.IRepository;
using log4net;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class DriverLicenseTypeRepository : IDriverLicenseTypeRepository
    {
        private ILog logError = LogManager.GetLogger("ERROR");
        public List<bx_drivelicense_cartype> FindList()
        {
            List<bx_drivelicense_cartype> list = new List<bx_drivelicense_cartype>();
            try
            {
                list =
                    DataContextFactory.GetDataContext().bx_drivelicense_cartype.Where(x =>x.company==3&&x.is_used==1).ToList();
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);

            }
            return list;
        }
    }
}
