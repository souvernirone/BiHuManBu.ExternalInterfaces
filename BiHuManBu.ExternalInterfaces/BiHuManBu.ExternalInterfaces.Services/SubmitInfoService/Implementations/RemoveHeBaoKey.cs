using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Infrastructure.CacheKeyFactory;
using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Infrastructure.MessageCenter;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.SubmitInfoService.Interfaces;
using BiHuManBu.Redis;
using log4net;

namespace BiHuManBu.ExternalInterfaces.Services.SubmitInfoService.Implementations
{
    public class RemoveHeBaoKey : IRemoveHeBaoKey
    {
        private readonly ILog _logError = LogManager.GetLogger("ERROR");

        public string RemoveHeBao(PostSubmitInfoRequest request)
        {
            //清理缓存
            string baojiaCacheKey =
                   CommonCacheKeyFactory.CreateKeyWithLicenseAndAgentAndCustKey(request.LicenseNo, request.ChildAgent, request.CustKey + request.RenewalCarType);
            string hebaokey = string.Format("{0}-{1}", baojiaCacheKey, SourceGroupAlgorithm.GetOldSource(request.Source));
            //using (var client = RedisManager.GetClient())
            //{
            //    using (var tran = client.CreatePipeline())
            //    {
            try
            {
                //            tran.QueueCommand(p =>
                //            {
                RedisManager.Remove(string.Format("{0}-hb-{1}", hebaokey, "key"));
                //});
                //tran.Flush();
            }
            catch (Exception ex)
            {
                _logError.Info("重新核保清空缓存失败:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
                throw new Exception("Redis 清理核保key发生异常", new RedisOperateException());
            }
            //}
            //}
            return hebaokey;
        }
    }
}
