using System.ComponentModel.DataAnnotations;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class ChangeReInfoAgentRequest:BaseRequest
    {
        /// <summary>
        /// 车牌号
        /// </summary>
        [Required]
        public string LicenseNo { get; set; }

        /// <summary>
        /// @（意向和受理记录都需要）
        /// </summary>
        [Range(1,2100000000)]
        public int OwnerAgent { get; set; }

        [Range(1, 2100000000)]
        public int AssignId { get; set; }

        /// <summary>
        /// addby20160909
        /// 目前只对app用
        /// 登陆状态
        /// </summary>
        public string BhToken { get; set; }
    }
}
