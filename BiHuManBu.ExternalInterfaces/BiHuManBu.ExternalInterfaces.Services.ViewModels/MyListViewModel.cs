
using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Services.ViewModels
{
    public class MyListViewModel : BaseViewModel
    {
        public int TotalCount { get; set; }
        public List<MyInfo> MyInfoList { get; set; }
    }

    public class MyInfo
    {
        public long Buid { get; set; }
        public string LicenseNo { get; set; }
        public string MoldName { get; set; }
        public string CreateTime { get; set; }
        /// <summary>
        /// 续保状态
        /// </summary>
        public int RenewalStatus { get; set; }
        /// <summary>
        /// 是否已报价 1是0否
        /// </summary>
        public int IsPrecisePrice { get; set; }

        #region 重新请求报价会用到
        public string ItemCustKey { get; set; }
        public string ItemChildAgent { get; set; }
        public string CityCode { get; set; }
        #endregion

        /// <summary>
        /// 回访状态
        /// </summary>
        public int VisitedStatus { get; set; }

        /// <summary>
        /// 交强险到期时间
        /// </summary>
        public string ForceExpireDate { get; set; }
        /// <summary>
        /// 商业险到期时间
        /// </summary>
        public string BusinessExpireDate { get; set; }
        /// <summary>
        /// 交强险剩余到期天数，负数表示脱保，正数表示到期天数
        /// </summary>
        public int ExpireDateNum { get; set; }

        /// <summary>
        /// 交强险起保时间
        /// </summary>
        //public string NextForceStartDate { get; set; }
        /// <summary>
        /// 商业险起保时间
        /// </summary>
        //public string NextBusinessStartDate { get; set; }

        public List<PrecisePriceInfo> PrecisePrice { get; set; }
    }

    public class PrecisePriceInfo
    {
        public long Source { get; set; }
        public double BizTotal { get; set; }
        public double ForceTotal { get; set; }
        public int QuoteStatus { get; set; }
        public int SubmitStatus { get; set; }
        public string SubmitResult { get; set; }
    }
}
