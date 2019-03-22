
using System;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Models;
using log4net;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class AppSettingRepository : IAppSettingRepository
    {
        private ILog _logError = LogManager.GetLogger("ERROR");
        
        public bx_appsetting FindByKey(string appSettingKey)
        {
            var model = new bx_appsetting();
            try
            {
                model = DataContextFactory.GetDataContext().bx_appsetting.FirstOrDefault(x => x.settingKey == appSettingKey && x.isDeleted == 0);
            }
            catch (Exception ex)
            {
                _logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return model;
        }
    }
}
