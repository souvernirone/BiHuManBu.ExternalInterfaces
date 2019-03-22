using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.ViewModels.IndependentViewModel
{
    /// <summary>
    /// 出险信息
    /// </summary>
    public class ClaimVo
    {
        /// <summary>
        /// 序号
        /// </summary>
        public string nSeqNo { get; set; }

        /// <summary>
        /// 出险时间
        /// </summary>
        public string accidentTime { get; set; }

        /// <summary>
        /// 结案时间
        /// </summary>
        public string endcaseTime { get; set; }

        /// <summary>
        /// 赔款金额
        /// </summary>
        public string claimAmount { get; set; }

        /// <summary>
        /// 是否造成受害人死亡
        /// </summary>
        public string isDeath { get; set; }

        /// <summary>
        /// 投保保险公司
        /// </summary>
        public string companyId { get; set; }
    }
}
