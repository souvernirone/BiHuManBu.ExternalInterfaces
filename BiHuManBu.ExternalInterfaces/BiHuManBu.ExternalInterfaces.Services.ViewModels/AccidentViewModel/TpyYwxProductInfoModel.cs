using System.Collections.Generic;

namespace BaoXian.Model.Common.Model
{
    /// <summary>
    /// 太平洋驾意险model
    /// </summary>
    public class TpyYwxProductInfoModel
    {
        /// <summary>
        ///  
        /// </summary>
        public string TpyYwxClassId { get; set; }

        /// <summary>
        /// // RWX任我行  /  RWX2任我行二代  /  JYX驾意险
        /// </summary>
        public string TpyYwxClassName { get; set; }

        /// <summary>
        ///     子套餐
        /// </summary>
        public List<ShowSubList> ShowSubList { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class ShowSubList
    {
        /// <summary>
        ///     公司代码
        /// </summary>
        public string BranchCode { get; set; }

        /// <summary>
        ///     险别子项
        /// </summary>
        public List<CoverageVos> CoverageVos { get; set; }

        /// <summary>
        ///     险别ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     保单保费（元）
        /// </summary>
        public decimal Premium { get; set; }

        /// <summary>
        ///     险别编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        ///     险别名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        ///     险别
        /// </summary>
        public string ProductType { get; set; }
        /// <summary>
        /// 座位数
        /// </summary>
        public string SeatCount { get; set; }

        /// <summary>
        ///     险别状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        ///     合计保额（元）
        /// </summary>
        public decimal TotalAmount { get; set; }
    }


    /// <summary>
    /// 子项
    /// </summary>
    public class CoverageVos
    {
        /// <summary>
        ///     保额（元
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        ///     编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     名称
        /// </summary>
        public string Name { get; set; }
    }


}