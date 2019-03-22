using System;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Models;
using log4net;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class CarInfoRepository : ICarInfoRepository
    {
        private ILog logError = LogManager.GetLogger("ERROR");
        public bx_carinfo Find(string licenseno)
        {
            bx_carinfo carinfo = new bx_carinfo();
            try
            {
                carinfo = DataContextFactory.GetDataContext().bx_carinfo.FirstOrDefault(x => x.license_no == licenseno);
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return carinfo;
        }

        public bx_carinfo FindOrderDate(string licenseno, int renewaltype = 0)
        {
            bx_carinfo carinfo = new bx_carinfo();
            try
            {
                carinfo = DataContextFactory.GetDataContext().bx_carinfo.FirstOrDefault(x => x.license_no == licenseno&&x.RenewalCarType==renewaltype);

            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return carinfo;
        }
        public bx_carinfo FindVinCarInfo(string carVin, int renewaltype = 0)
        {
            bx_carinfo carinfo = new bx_carinfo();
            try
            {
                carinfo = DataContextFactory.GetDataContext().bx_carinfo.FirstOrDefault(x => x.vin_no == carVin && x.RenewalCarType == renewaltype);

            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return carinfo;
        }
    }
}
