using System;
using System.Collections.Generic;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Models;
using log4net;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class SubmitInfoRepository : ISubmitInfoRepository
    {
        private ILog logInfo = LogManager.GetLogger("INFO");
        private ILog logError = LogManager.GetLogger("ERROR");
        
        public bx_submit_info GetSubmitInfo(long buid, int source)
        {
            bx_submit_info submitInfo = new bx_submit_info();
            try
            {
                submitInfo = DataContextFactory.GetDataContext().bx_submit_info.FirstOrDefault(x => x.b_uid == buid && x.source == source);
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return submitInfo;
        }
        /// <summary>
        /// 获取核保列表
        /// </summary>
        /// <param name="buid"></param>
        /// <returns></returns>
        public List<bx_submit_info> GetSubmitInfoList(long buid)
        {
            var list = new List<bx_submit_info>();
            try
            {
                list = DataContextFactory.GetDataContext().bx_submit_info.Where(x => x.b_uid == buid).ToList();
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return list;
        }
        public bool HasSubmitInfo(long buid)
        {
            try
            {
                var model = from si in DataContextFactory.GetDataContext().bx_submit_info
                            where si.b_uid == buid
                            select 1;
                return model.Count() > 0;
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return false;
        }

    }
}
