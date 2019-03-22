using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.ViewModels.IndependentViewModel
{
    /// <summary>
    /// 交强/商业 基础信息
    /// </summary>
    public class CvrgBaseVO
    {
        /// <summary>
        /// 申请单号
        /// </summary>
        public string CAppNo { get; set; }

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
        /// 查询码
        /// </summary>
        public string CQryCde { get; set; }

        /// <summary>
        /// 当前时间
        /// </summary>
        public string NowDate { get; set; }
    }
}
