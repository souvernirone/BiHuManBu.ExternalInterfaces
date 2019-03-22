
using System.ComponentModel.DataAnnotations;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class EditAgentUKeyRequest : BaseRequest
    {
        /// <summary>
        /// 员工工号-登录账号
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string UserCode { get; set; }

        /// <summary>
        /// UkeyId
        /// </summary>
        [Range(1, 100000000)]
        public int UkeyId { get; set; }

        /// <summary>
        /// 旧密码
        /// </summary>
        //[Required]
        //[StringLength(100, MinimumLength = 8)]
        public string OldPassWord { get; set; }

        /// <summary>
        /// 需要修改的密码
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string NewPassWord { get; set; }

        private int _reqSource = 1;
        /// <summary>
        /// 默认1对外接口请求，需要旧密码；2内部请求，运营后台；3内部crm请求，需要校验agent
        /// </summary>
        public int ReqSource
        {
            get { return _reqSource; }
            set { _reqSource = value; }
        }

    }
}
