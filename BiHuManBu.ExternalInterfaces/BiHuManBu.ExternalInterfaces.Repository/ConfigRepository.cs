using System;
using System.Collections.Generic;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.IRepository;
using log4net;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class ConfigRepository : IConfigRepository
    {
        private readonly ILog logError = LogManager.GetLogger("ERROR");
        public bx_config Find(string configKey, int configType)
        {
            try
            {
                bx_config model = DataContextFactory.GetDataContext().bx_config.FirstOrDefault(i => i.config_key.Equals(configKey) && i.config_type == configType && i.is_delete == 0);
                return model;
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return null;
        }

        public List<bx_config> FindList(int configType)
        {
            try
            {
                List<bx_config> list = DataContextFactory.GetDataContext().bx_config.Where(i => i.config_type == configType && i.is_delete == 0).ToList();
                return list;
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return null;
        }
    }
}
