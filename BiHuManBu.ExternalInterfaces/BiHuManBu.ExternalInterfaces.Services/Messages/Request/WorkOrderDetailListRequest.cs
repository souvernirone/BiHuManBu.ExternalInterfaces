
using System.ComponentModel.DataAnnotations;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class WorkOrderDetailListRequest:BaseRequest
    {
        [Required]
        public string LicenseNo { get; set; }

        [StringLength(32, MinimumLength = 10)]
        public string CustKey { get; set; }

        [Range(1, 2100000000)]
        public int ChildAgent { get; set; }

        /// <summary>
        /// addby20160909
        /// 目前只对app用
        /// 登陆状态
        /// </summary>
        public string BhToken { get; set; }
    }
}
