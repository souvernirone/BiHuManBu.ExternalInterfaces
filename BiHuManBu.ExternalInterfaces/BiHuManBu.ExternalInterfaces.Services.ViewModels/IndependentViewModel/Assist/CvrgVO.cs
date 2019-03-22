using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.ViewModels.IndependentViewModel
{
    public class CvrgVO
    {
        /// <summary>
        /// 险别名称
        /// </summary>
        public string cCustCvrgNme { get; set; }

        /// <summary>
        /// 保额
        /// </summary>
        public string nAmt { get; set; }

        /// <summary>
        /// 基准保费
        /// </summary>
        public string nBefPrm { get; set; }

        /// <summary>
        /// 应交保费
        /// </summary>
        public string nPrm { get; set; }
    }
}
