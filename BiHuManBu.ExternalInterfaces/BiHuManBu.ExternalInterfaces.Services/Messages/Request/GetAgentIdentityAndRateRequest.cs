

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class GetAgentIdentityAndRateRequest:BaseRequest
    {

        //openid 当前试算的openid，agent即是fromagent的id，parentagent 从个人中心进来的时候 区分顶级经纪人的id

        public string OpenId { get; set; }
        public int ParentAgent { get; set; }
        public long Buid { get; set; }
        public long Source { get; set; }

        /// <summary>
        /// 当前代理人Id。微信新增字段
        /// 20170228账号统一，openid查询改为childagent查询
        /// </summary>
        //[Range(1, 1000000)]
        public int ChildAgent { get; set; }
    }
}
