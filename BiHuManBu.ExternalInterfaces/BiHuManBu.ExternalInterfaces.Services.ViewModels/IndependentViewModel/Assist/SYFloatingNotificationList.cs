using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.ViewModels.IndependentViewModel
{
    /// <summary>
    /// 商业浮动通知列表
    /// </summary>
    public class SYFloatingNotificationList
    {
        /// <summary>
        /// 商业险基本信息
        /// </summary>
        public CvrgBaseVO SYBaseVO { get; set; }

        /// <summary>
        /// 商业险 险别信息 列表
        /// </summary>
        public List<CvrgVO> SYCvrgList { get; set; }

        /// <summary>
        /// 车辆信息
        /// </summary>
        public VhlVOBase SYVhlVO { get; set; }

        /// <summary>
        /// 商业出险信息
        /// </summary>
        public List<ClaimVo> SYClaimList { get; set; }

        /// <summary>
        /// 商业险系数
        /// </summary>
        public prmCoefBase SYprmCoef { get; set; }

        /// <summary>
        /// 结果信息
        /// </summary>
        public List<Msg> SYMsgList { get; set; }
    }
}
