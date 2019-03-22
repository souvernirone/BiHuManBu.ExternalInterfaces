using System.ComponentModel.DataAnnotations;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class GetReInfoDetailRequest:BaseRequest
    {
        public string LicenseNo { get; set; }

        [StringLength(32, MinimumLength = 10)]
        public string CustKey { get; set; }

        /// <summary>
        /// 该agent是从续保列表中传过来的agent，否则将有可能查不到数据
        /// </summary>
        [Range(1, 2100000000)]
        public int? ChildAgent { get; set; }

        public long? Buid { get; set; }

        /// <summary>
        /// addby20160909
        /// 目前只对app用
        /// 登陆状态
        /// </summary>
        public string BhToken { get; set; }
    }
}
