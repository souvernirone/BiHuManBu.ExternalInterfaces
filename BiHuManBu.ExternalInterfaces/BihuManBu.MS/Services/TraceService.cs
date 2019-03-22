using System;
using BihuManBu.MS.Messages.Request;
using BiHuManBu.Redis;

namespace BihuManBu.MS.Services
{
    public class TraceService
    {
        public void Statistics(TraceRequest request)
        {
            
        }

        private void LogIntoCache(TraceRequest request)
        {
            var expireTime = DateTime.Now.AddMinutes(10);
            //using (var cli = RedisManager.GetClient())
            //{
                //顶级分组 每天
                var topAgentGroup = string.Format("ta:{0}", DateTime.Now.ToString("yyyy-MM-dd"));
                //每个顶级下的次级
                var childAgentPerTopAgent = string.Format("{0}:cld", topAgentGroup);
                //每个下级
                var childAgent = string.Format("{0}:ta:{1}", childAgentPerTopAgent, request.ChildAgent);
                //每个下级的所有车牌号
                var licensenosPerChildAgent = string.Format("{0}:lis", childAgent);
                //每个人算的单个车牌号
                var licensenoLevel = string.Format("{0}:ca:{1}", licensenosPerChildAgent, request.LicenseNo);
                //每个车牌号下共发起几次跟踪链接
                var rootIdsPerLicenseno = string.Format("{0}:rts:{1}",licensenoLevel,request.RootId);
                //每个跟踪链下共有多少次步骤
                var childRootIdsPerRootId = string.Format("{0}:tc:{1}", rootIdsPerLicenseno, request.ChildRootId);
                //using (var pipe=cli.CreatePipeline())
                //{
                //    pipe.QueueCommand(p =>
                //    {
                //        p.AddItemToSet(topAgentGroup,request.Agent.ToString());
                //    });
                //    pipe.QueueCommand(p =>
                //    {
                //        p.ExpireEntryAt(topAgentGroup, expireTime);
                //    });
                //    pipe.QueueCommand(p =>
                //    {
                //        p.AddItemToSet(childAgent,childAgentPerTopAgent);
                //    });
                //    pipe.QueueCommand(p =>
                //    {
                //        p.ExpireEntryAt(topAgentGroup, expireTime);
                //    });
                //    pipe.QueueCommand(p =>
                //    {
                       
                //    });
                //}
            //}
        }
    }
}