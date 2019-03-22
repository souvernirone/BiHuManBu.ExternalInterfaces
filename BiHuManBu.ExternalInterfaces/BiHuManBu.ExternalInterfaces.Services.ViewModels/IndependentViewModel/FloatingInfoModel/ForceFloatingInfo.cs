using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.ViewModels.IndependentViewModel.FloatingInfoModel
{
    public class ForceFloatingInfo
    {
        /// <summary>
        /// 交强基础信息
        /// </summary>
        public ForceBaseInfo ForceBaseInfo { get; set; }
        /// <summary>
        /// 交强险独有 ,出险列表
        /// </summary>
        public List<ClaimVo> ForceClaim { get; set; }
        /// <summary>
        /// 交强险独有 车船税信息
        /// </summary>
        public TaxInfo TaxInfo { get; set; }
    }
}
