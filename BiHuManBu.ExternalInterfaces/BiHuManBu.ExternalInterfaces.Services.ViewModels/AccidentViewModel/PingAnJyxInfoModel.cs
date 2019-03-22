using System.Collections.Generic;

namespace BaoXian.Model.Common.Model
{
    /// <summary>
    ///     平安驾意险model
    /// </summary>
    public class PingAnJyxInfoModel
    {   
        /// <summary>
        ///     总分类(驾意险，"全车驾乘险""一年期综合意外&航空意外险")
        /// </summary>
        public string displayName { get; set; }

        /// <summary>
        /// </summary>
        public string displayTypeCode { get; set; }

        /// <summary>
        ///     子套餐
        /// </summary>
        public List<showPackageInfoList> showPackageInfoList { get; set; }
    }

    /// <summary>
    ///     子套餐
    /// </summary>
    public class showPackageInfoList
    {
        /// <summary>
        /// 座位数
        /// </summary>
        public string SeatCount { get; set; }

        /// <summary>
        /// </summary>
        public string autoCustomer { get; set; }

        /// <summary>
        /// </summary>
        public decimal discount { get; set; }

        /// <summary>
        /// </summary>
        public string includeVirtualInsured { get; set; }

        /// <summary>
        /// </summary>
        public decimal insuredAmount { get; set; }

        /// <summary>
        /// </summary>
        public int leastAcceptInsureAge { get; set; }

        /// <summary>
        /// </summary>
        public string marketAndPackCode { get; set; }

        /// <summary>
        /// </summary>
        public string packageCode { get; set; }

        /// <summary>
        /// 平安驾乘人员意外保险(基础版
        /// </summary>
        public string packageName { get; set; }

        /// <summary>
        /// </summary>
        public string productCode { get; set; }

        /// <summary>
        /// </summary>
        public List<sellList> sellList { get; set; }

        /// <summary>
        /// </summary>
        public int topAcceptInsureAge { get; set; }

        /// <summary>
        /// </summary>
        public int topCopyNum { get; set; }

        /// <summary>
        /// </summary>
        public string version { get; set; }

        /// <summary>
        ///     险别子项
        /// </summary>
        public List<BxSubList> BxSubList { get; set; }
   
    }

    public class sellList
    {
        public int displayNo { get; set; }
        public string encodeKey { get; set; }
        public string encodeValue { get; set; }
    }

    /// <summary>
    /// 具体险种金额
    /// </summary>
    public class BxSubList
    {
        /// <summary>
        /// </summary>
        public string dutyCode { get; set; }

        /// <summary>
        /// </summary>
        public string insuranceCoverage { get; set; }

        /// <summary>
        /// </summary>
        public decimal insuredAmount { get; set; }

        /// <summary>
        /// </summary>
        public string planCode { get; set; }

        /// <summary>
        /// 险种类别
        /// </summary>
        public string planName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal unitInsuredAmount { get; set; }
    }

}