using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Models;
using log4net;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class PreferentialActivityRepository : IPreferentialActivityRepository
    {
        private ILog logError = LogManager.GetLogger("ERROR");

        /// <summary>
        /// 根据buid获取活动信息
        /// </summary>
        /// <param name="buid"></param>
        /// <returns></returns>
        public List<bx_preferential_activity> GetActivityByBuid(long buid)
        {
            var result = from pa in DataContextFactory.GetDataContext().bx_preferential_activity
                         where pa.activity_status == 1 &&
                         (from ba in DataContextFactory.GetDataContext().bx_buid_activity
                          where ba.B_Uid == buid
                          select ba.activity_id).Contains(pa.id)
                         select pa;
            return result.ToList();
        }

        /// <summary>
        /// 根据传过来的id查询活动内容
        /// </summary>
        /// <param name="stringId"></param>
        /// <returns></returns>
        public List<bx_preferential_activity> GetActivityByIds(string stringId)
        {
            string[] strArray = stringId.Split(',');
            if (strArray.Length > 0)
            {
                int[] intArray = Array.ConvertAll<string, int>(strArray, int.Parse);
                var result = DataContextFactory.GetDataContext().bx_preferential_activity.Where(
                    i => intArray.Contains(i.id) && i.activity_status == 1)
                    .ToList().OrderByDescending(o => o.create_time);
                return result.ToList();
            }
            return new List<bx_preferential_activity>();
        }

        public async Task<List<bx_preferential_activity>> GetActivityByIdsAsync(string stringId)
        {
            string[] strArray = stringId.Split(',');
            if (strArray.Length > 0)
            {
                int[] intArray = Array.ConvertAll<string, int>(strArray, int.Parse);
                var result = DataContextFactory.GetDataContext().bx_preferential_activity.Where(
                    i => intArray.Contains(i.id) && i.activity_status == 1)
                    .ToList().OrderByDescending(o => o.create_time);
                return result.ToList();
            }
            return new List<bx_preferential_activity>();
        }

        public bx_preferential_activity AddActivity(bx_preferential_activity request)
        {
            bx_preferential_activity response = new bx_preferential_activity();
            try
            {
                response = DataContextFactory.GetDataContext().bx_preferential_activity.Add(request);
                DataContextFactory.GetDataContext().SaveChanges();
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return response;
        }

        public bx_preferential_activity GetListByType(int Type, string ActivityContent)
        {
            return DataContextFactory.GetDataContext().bx_preferential_activity.Where(n=>n.activity_type == Type && n.activity_content == ActivityContent).ToList().OrderByDescending(n=>n.id).FirstOrDefault();
        }

    }
}
