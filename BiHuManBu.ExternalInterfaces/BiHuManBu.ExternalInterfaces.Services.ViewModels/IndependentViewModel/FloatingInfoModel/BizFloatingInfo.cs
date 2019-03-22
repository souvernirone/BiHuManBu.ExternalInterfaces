using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.ViewModels.IndependentViewModel.FloatingInfoModel
{
    public class BizFloatingInfo
    {
        /// <summary>
        /// 交强基础信息
        /// </summary>
        public BizBaseInfo BizBaseInfo { get; set; }
        /// <summary>
        /// 商业险 险别信息 列表
        /// </summary>
        public List<CvrgVO> BizQuoteResult { get; set; }

    }
}
