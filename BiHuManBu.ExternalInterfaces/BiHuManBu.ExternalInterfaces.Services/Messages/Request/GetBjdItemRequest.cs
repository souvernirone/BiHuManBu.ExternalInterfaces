
using System.ComponentModel.DataAnnotations;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class GetBjdItemRequest//:BaseRequest
    {
        public int Source { get; set; }
        [Range(1, long.MaxValue)]
        public long Bxid { get; set; }
        /// <summary>
        /// 是否测试 1是0否
        /// </summary>
        public int Test { get; set; }
        /// <summary>
        /// 是否为接口url，判断南北机房数据同步问题做的处理1是0否。如果是1，则不继续请求
        /// </summary>
        public int JieKouUrl { get; set; }

        #region forApp新增，微信端未用到
        [StringLength(32, MinimumLength = 10)]
        public string CustKey { get; set; }

        public int ChildAgent { get; set; }

        public string BhToken { get; set; }
        public int Agent { get; set; }

        [StringLength(32, MinimumLength = 32)]
        public string SecCode { get; set; }

        #endregion
    }
}
