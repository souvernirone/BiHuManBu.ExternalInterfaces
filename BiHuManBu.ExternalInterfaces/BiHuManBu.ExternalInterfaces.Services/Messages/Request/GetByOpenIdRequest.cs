using System.ComponentModel.DataAnnotations;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class GetByOpenIdRequest
    {
        //[Required]
        //[StringLength(32, MinimumLength = 16)]
        public string OpenId { get; set; }
        [Range(0, 21000000000)]
        public int TopParentAgent { get; set; }

        /// <summary>
        /// 当前代理人Id。微信新增字段
        /// 20170228账号统一，openid查询改为childagent查询
        /// </summary>
        [Range(1, 1000000)]
        public int ChildAgent { get; set; }
    }
}
