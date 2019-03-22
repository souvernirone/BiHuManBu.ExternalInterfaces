using System.ComponentModel.DataAnnotations;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class GetMyListRequest : AppBaseRequest
    {
        private int _isOnlyMine = 1;
        private int _orderCreateTime = 1;
        public string LicenseNo { get; set; }

        [Range(1, 10000)]
        public int PageSize { get; set; }

        [Range(1, 10000)]
        public int CurPage { get; set; }

        /// <summary>
        /// 是否只查属于自己代理的bx_userinfo。1，是；0，否
        /// 如果是1，则不查下级代理
        /// </summary>
        public int IsOnlyMine
        {
            get { return _isOnlyMine; }
            set { _isOnlyMine = value; }
        }

        /// <summary>
        /// 根据什么排序。1，录入时间；2，到期时间
        /// 列表排序默认按照录入时间排
        /// </summary>
        public int OrderBy
        {
            get { return _orderCreateTime; }
            set { _orderCreateTime = value; }
        }
    }
}
