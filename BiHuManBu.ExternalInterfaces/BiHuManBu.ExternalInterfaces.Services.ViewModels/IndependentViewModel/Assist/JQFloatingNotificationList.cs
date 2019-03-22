using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.ViewModels.IndependentViewModel
{
    /// <summary>
    /// 交强浮动通知列表
    /// </summary>
    public class JQFloatingNotificationList
    {
        /// <summary>
        /// 交强基本信息
        /// </summary>
        public CvrgBaseVO JQBaseVO { get; set; }

        /// <summary>
        /// 交强险别信息 列表
        /// </summary>
        public List<CvrgVO> JQCvrgList { get; set; }


        /// <summary>
        /// 交强车辆信息
        /// </summary>
        public VhlVOBase JQVhlVO { get; set; }


        /// <summary>
        /// 交强险独有 ,出险列表
        /// </summary>
        public List<ClaimVo> JQClaimList { get; set; }

        /// <summary>
        /// 交强险险系数
        /// </summary>
        public prmCoefBase JQprmCoef { get; set; }

        /// <summary>
        ///  交强险独有 车船税信息
        /// </summary>
        public VsTax VsTax { get; set; }

        /// <summary>
        /// 结果信息
        /// </summary>
        public List<Msg> JQMsgList { get; set; }
    }
}
