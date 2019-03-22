using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.IRepository;
using log4net;
using System;
using System.Data.Entity.Migrations;
using System.Linq;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class CarRecordRepository : ICarRecordRepository
    {
        private ILog logError = LogManager.GetLogger("ERROR");
        public CarRecordRepository()
        { }

        /// <summary>
        /// 取对象对应的模型，需要考虑1索引2是否需要车架号发动机号获取数据
        /// </summary>
        /// <param name="carVin"></param>
        /// <param name="engino"></param>
        /// <returns></returns>
        public bx_car_record GetModel(string carVin, string engino)
        {
            bx_car_record item = new bx_car_record();
            try
            {
                item = DataContextFactory.GetDataContext().bx_car_record.FirstOrDefault(l => l.CarVin == carVin);// && l.EngineNo == engino
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return item;
        }

        public int AddUpdateCarRecord(bx_car_record model)
        {
            int count = 0;
            try
            {
                DataContextFactory.GetDataContext().bx_car_record.AddOrUpdate(model);
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
