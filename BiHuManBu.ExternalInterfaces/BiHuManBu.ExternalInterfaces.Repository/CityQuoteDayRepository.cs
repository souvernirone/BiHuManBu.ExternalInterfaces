using System;
using System.Collections.Generic;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.IRepository;
using log4net;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class CityQuoteDayRepository : ICityQuoteDayRepository
    {
        private ILog logError = LogManager.GetLogger("ERROR");
        public int GetDaysNum(int cityId)
        {
            int daysNum = 0;
            bx_cityquoteday model = new bx_cityquoteday();
            try
            {
                model = DataContextFactory.GetDataContext().bx_cityquoteday.FirstOrDefault(i => i.cityid == cityId);
                if (model != null)
                {
                    daysNum = model.quotedays.HasValue ? model.quotedays.Value : 90;//如果库中没值，默认90天
                }
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return daysNum;
        }

        public List<bx_cityquoteday> GetList(int agentId)
        {
            var items = DataContextFactory.GetDataContext().Database.SqlQuery<bx_cityquoteday>(@"select * from  bx_cityquoteday
                                                        where  cityid in (
                                                        select distinct city_id from   bx_agent_config where  agent_id =" + agentId + " and is_used=1 )");
            return items.ToList();
        }
        public bx_cityquoteday Get(int cityId)
        {
            bx_cityquoteday model = null;
            try
            {
                model = new EntityContext().bx_cityquoteday.FirstOrDefault(i => i.cityid == cityId);
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return model;
        }
    }
}
