using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.ViewModels.IndependentViewModel.FloatingInfoModel
{
    public class TaxInfo
    {
        /// <summary>
        /// 未缴纳车船税起期
        /// </summary>
        public string TTaxEffBgnTm { get; set; }
        /// <summary>
        /// 未缴纳车船税止期
        /// </summary>
        public string TTaxEffEndTm { get; set; }
        /// <summary>
        /// 应缴纳税款
        /// </summary>
        public string NTaxableAmt { get; set; }
        /// <summary>
        /// 滞纳金
        /// </summary>
        public string NOverdueAmt { get; set; }
        /// <summary>
        /// 合计应缴纳总金额
        /// </summary>
        public string TaxSumAmount { get; set; }

        /// <summary>
        /// 总额金大写
        /// </summary>
        public string TaxSumAmountUp { get; set; }
    }
}
