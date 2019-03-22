
using System.ComponentModel.DataAnnotations;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class GetReInfoListRequest : BaseRequest
    {
        private int _lastYearSource = -1;
        private int _renewalStatus = -1;
        public string LicenseNo { get; set; }

        [Range(1, 10000)]
        public int PageSize { get; set; }
        [Range(1, 10000)]
        public int CurPage { get; set; }
        [Range(1, 1000000)]
        public int ChildAgent { get; set; }

        /// <summary>
        /// 是否只查属于自己代理的bx_userinfo。1，是；0，否
        /// 如果是1，则不查下级代理
        /// </summary>
        public int? IsOnlyMine { get; set; }

        /// <summary>
        /// addby20160909
        /// 目前只对app用
        /// 登陆状态
        /// </summary>
        public string BhToken { get; set; }

        /// <summary>
        /// 上一年续保公司
        /// -1全部 >-1保险公司，旧编号
        /// </summary>
        public int LastYearSource
        {
            get { return _lastYearSource; }
            set { _lastYearSource = value; }
        }

        /// <summary>
        /// 续保状态
        /// -1全部 1成功 0失败
        /// </summary>
        public int RenewalStatus
        {
            get { return _renewalStatus; }
            set { _renewalStatus = value; }
        }
    }
}
