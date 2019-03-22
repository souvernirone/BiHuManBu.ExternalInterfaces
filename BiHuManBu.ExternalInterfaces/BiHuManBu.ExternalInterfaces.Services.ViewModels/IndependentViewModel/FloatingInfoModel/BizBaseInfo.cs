using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.ViewModels.IndependentViewModel.FloatingInfoModel
{
    public class BizBaseInfo
    {
        /// <summary>
        /// 保单号
        /// </summary>
        public string CPlyNo { get; set; }

        /// <summary>
        /// 投保人名称
        /// </summary>
        public string CAppName { get; set; }

        /// <summary>
        /// 保险起期
        /// </summary>
        public string TSrcInsrncBgnTm { get; set; }

        /// <summary>
        /// 保险止期
        /// </summary>
        public string TSrcInsrncEndTm { get; set; }

        /// <summary>
        /// 浮动因素计算起期
        /// </summary>
        public string TLastStartDate { get; set; }

        /// <summary>
        /// 浮动因素计算止期
        /// </summary>
        public string TLaseEndDate { get; set; }

        /// <summary>
        /// 应交总保费
        /// </summary>
        public string NPrm { get; set; }

        /// <summary>
        /// 大写总保费
        /// </summary>
        public string DNPrm { get; set; }

        /// <summary>
        /// 与道路交通事故相联系的费率浮动比率
        /// </summary>
        public string FloatRatio { get; set; }

        /// <summary>
        /// 无赔款优待及上年赔款记录浮动系数
        /// </summary>
        public string CAgoClmRecQuick { get; set; }
        /// <summary>
        /// 自主核保系数
        /// </summary>
        public string NAutoCheCoef { get; set; }
        /// <summary>
        /// 自主渠道系数
        /// </summary>
        public string NAutoChaCoef { get; set; }
        /// <summary>
        /// 违法系数
        /// </summary>
        public string NResvNum { get; set; }
        /// <summary>
        /// 最终浮动系数
        /// </summary>
        public string NIrrRatio { get; set; }
    }
}
