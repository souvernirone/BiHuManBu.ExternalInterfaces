using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.ViewModels.IndependentViewModel
{
    /// <summary>
    /// 交强险险系数
    /// </summary>
    public class prmCoefBase
    {
        /// <summary>
        /// 与道路交通事故相联系的费率浮动比率
        /// </summary>
        public string FloatRatio { get; set; }

        /// <summary>
        /// 未发生交通事故的年限
        /// </summary>
        public string NResvNum { get; set; }
        /// <summary>
        /// 无赔款优待及上年赔款记录浮动系数
        /// </summary>
        public string cAgoClmRecQuick { get; set; }
        /// <summary>
        /// 自主核保系数
        /// </summary>
        public string nAutoCheCoef { get; set; }
        /// <summary>
        /// 自主渠道系数
        /// </summary>
        public string nAutoChaCoef { get; set; }
        /// <summary>
        /// 违法系数
        /// </summary>
        public string nResvNum { get; set; }
        /// <summary>
        /// 最终浮动系数
        /// </summary>
        public string nIrrRatio { get; set; }

    }
}
