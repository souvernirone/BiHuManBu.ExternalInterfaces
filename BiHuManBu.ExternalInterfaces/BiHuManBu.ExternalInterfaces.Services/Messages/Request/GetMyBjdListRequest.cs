
using System;
using System.ComponentModel.DataAnnotations;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class GetMyBjdListRequest : BaseRequest
    {
        public string LicenseNo { get; set; }
        public int? buid { get; set; }
        [Range(1, 10000)]
        public int PageSize { get; set; }
        [Range(1, 10000)]
        public int CurPage { get; set; }
        public string OpenId { get; set; }
        [Range(1, 1000000)]
        public int TopParentAgent { get; set; }

        /// <summary>
        /// 是否只查属于自己代理的bx_userinfo。1，是；0，否
        /// 如果是1，则不查下级代理
        /// </summary>
        public int? IsOnlyMine { get; set; }

        /// <summary>
        /// 当前代理人Id。微信新增字段
        /// 20170228账号统一，openid查询改为childagent查询
        /// </summary>
        [Range(1, 1000000)]
        public int ChildAgent { get; set; }
    }
}
