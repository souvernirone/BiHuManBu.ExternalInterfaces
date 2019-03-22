using System;
using System.Data.Entity.Migrations;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Models;
using log4net;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class CustomerCategoriesRepository : ICustomerCategoriesRepository
    {
        private ILog logInfo = LogManager.GetLogger("INFO");
        private ILog logError = LogManager.GetLogger("ERROR");
        public bx_customercategories Get(int id)
        {
            var customerCategories = new bx_customercategories();
            try
            {
                customerCategories = DataContextFactory.GetDataContext().bx_customercategories.FirstOrDefault(x => x.Id == id);
                return customerCategories;
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
                return null;
            }

        }
        public bx_customercategories GetAsync(int id)
        {
            var customerCategories = new bx_customercategories();
            try
            {
                customerCategories = new EntityContext().bx_customercategories.FirstOrDefault(x => x.Id == id);
                return customerCategories;
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
                return null;
            }

        }
    }
}
