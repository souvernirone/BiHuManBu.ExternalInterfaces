using System;
using System.Collections.Generic;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.ReportModel;
using log4net;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class AgentConfigRepository : IAgentConfigRepository
    {
        private ILog logError = LogManager.GetLogger("ERROR");
        public List<bx_agent_config> Find(int agentid)
        {
            return DataContextFactory.GetDataContext().bx_agent_config.Where(x => x.agent_id == agentid && x.is_used == 1).ToList();
        }

        public List<bx_agent_config> FindNewCity(int agentid)
        {
            return DataContextFactory.GetDataContext().bx_agent_config.Where(x => x.agent_id == agentid && x.is_used == 1).OrderBy(o => o.city_id).ToList();
        }
        public List<bx_agent_config> FindCities(int agentid, int cityId)
        {
            return DataContextFactory.GetDataContext().bx_agent_config.Where(x => x.agent_id == agentid && x.is_used == 1 && x.city_id == cityId).OrderBy(o => o.city_id).ToList();
        }

        /// <summary>
        /// 获取城市集合
        /// </summary>
        /// <param name="agentId"></param>
        /// <returns></returns>
        public List<int> FindCity(int agentId)
        {
            return DataContextFactory.GetDataContext().bx_agent_config.Where(x => x.agent_id == agentId).Select(s => s.city_id.Value).Distinct().ToList();
        }

        public bx_agent_config FindById(long id)
        {
            return DataContextFactory.GetDataContext().bx_agent_config.FirstOrDefault(x => x.id == id);
        }
        public List<AgentConfigNameModel> FindListById(string ids)
        {
            List<AgentConfigNameModel> list = new List<AgentConfigNameModel>();
            try
            {
                string sql = string.Empty;
                if (!string.IsNullOrEmpty(ids))
                {
                    sql = string.Format("SELECT id,config_name AS ConfigUkeyName,is_paic_api as IsPaicApi FROM bx_agent_config where id in ({0})", ids);
                    list = DataContextFactory.GetDataContext().Database.SqlQuery<AgentConfigNameModel>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return list;
        }

        public List<long> FindSource(int agentid)
        {
            List<long> intlist = new List<long>();
            var query = (from ac in DataContextFactory.GetDataContext().bx_agent_config
                         where ac.agent_id == agentid
                         select ac.source.Value).Distinct().ToList();
            foreach (int it in query)
            {
                //20160905修改source0123=>1248，数据库里返回的老数据转换
                intlist.Add(SourceGroupAlgorithm.GetNewSource(it));
            }
            return intlist;
        }

        public List<bx_agent_config> FindBy(int agentid, int citycode)
        {
            return DataContextFactory.GetDataContext().bx_agent_config.Where(x => x.agent_id == agentid && x.is_used == 1 && x.city_id == citycode).ToList();
        }


        public List<bx_agent_config> FindByIds(List<long> ids)
        {
            return DataContextFactory.GetDataContext().bx_agent_config.Where(x => ids.Contains(x.id) && x.is_used == 1).ToList();
        }

        public bx_agent_config FindByChannelId(int agentid, long channelId)
        {
            return DataContextFactory.GetDataContext().bx_agent_config.FirstOrDefault(x => x.agent_id == agentid && x.is_used == 1 && x.id == channelId);
        }

        public List<MultiChannelsModel> FindConfigSourceList(int agentid, int citycode)
        {
            List<MultiChannelsModel> list = new List<MultiChannelsModel>();
            try
            {
                //SELECT * FROM ( SELECT * FROM bx_agent_config where agent_id={0} and city_id={1} and is_used=1 ORDER BY create_time ASC) AS m GROUP BY m.source
                string sql = string.Format("SELECT MIN(id) AS ChannelId,source AS Source FROM bx_agent_config WHERE agent_id={0} AND city_id={1} AND is_used=1 GROUP BY Source", agentid, citycode);
                list = DataContextFactory.GetDataContext().Database.SqlQuery<MultiChannelsModel>(sql).ToList();
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return list;
        }

        public List<int> GetConfigCityList(int agentid)
        {
            List<int> list = new List<int>();
            try
            {
                string sql = string.Format("SELECT DISTINCT(city_id) FROM bx_agent_config WHERE agent_id={0} and is_used=1", agentid);
                list = DataContextFactory.GetDataContext().Database.SqlQuery<int>(sql).ToList();
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return list;
        }
    }
}
