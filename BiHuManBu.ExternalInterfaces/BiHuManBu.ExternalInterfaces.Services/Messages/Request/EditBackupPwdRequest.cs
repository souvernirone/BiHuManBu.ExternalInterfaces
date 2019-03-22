using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class EditBackupPwdRequest : BaseRequest
    {
        [Range(1, 100000000)]
        public int UkeyId { get; set; }
        [Required(ErrorMessage = "备用密码1不能为空"), MaxLength(20), MinLength(8)]
        public string PwdOne { get; set; }
        [Required(ErrorMessage = "备用密码2不能为空"), MaxLength(20), MinLength(8)]
        public string PwdTwo { get; set; }
        [Required(ErrorMessage = "备用密码3不能为空"), MaxLength(20), MinLength(8)]
        public string PwdThree { get; set; }

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
