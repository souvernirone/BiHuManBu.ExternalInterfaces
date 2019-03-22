
using System.ComponentModel.DataAnnotations;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class GetOrdersRequest:BaseRequest
    {
        [Required]
        public string OpenId { get; set; }
        [Range(1, 1000000)]
        public int TopAgentId { get; set; }
        public string Search { get; set; }
        [Range(1,10000)]
        public int PageIndex { get; set; }
        [Range(1, 10000)]
        public int PageSize { get; set; }
        public int? IsOnlyMine { get; set; }
        /// <summary>
        /// 当前代理人Id。微信新增字段
        /// 20170228账号统一，openid查询改为childagent查询
        /// </summary>
        [Range(1, 1000000)]
        public int ChildAgent { get; set; }
    }
}
