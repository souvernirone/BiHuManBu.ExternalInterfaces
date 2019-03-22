namespace BiHuManBu.ExternalInterfaces.Services.ViewModels
{
    public class GetPrecisePriceViewModel:BaseViewModel
    {
        public GetPrecisePriceOfUserInfoViewModel UserInfo { get; set; }
        public PrecisePriceItemViewModel Item { get; set; }
        public QuoteResultCarInfoViewModel CarInfo { get; set; }
        public string CustKey { get; set; }
        public string CheckCode { get; set; }
    }
    public class GetPrecisePriceNewViewModel : AppBaseViewModel
    {
        public GetPrecisePriceOfUserInfoViewModel UserInfo { get; set; }
        public PrecisePriceItemViewModel Item { get; set; }
        public QuoteResultCarInfoViewModel CarInfo { get; set; }
        public string CheckCode { get; set; }
    }

    public class GetPrecisePriceViewModelWithBuid : BaseViewModel
    {
        public GetPrecisePriceOfUserInfoViewModel UserInfo { get; set; }
        public PrecisePriceItemViewModelWithBuid Item { get; set; }
        public QuoteResultCarInfoViewModel CarInfo { get; set; }
        public QuoteReqCarInfoViewModel ReqInfo { get; set; }
        
        public string CustKey { get; set; }
        public string CheckCode { get; set; }
    }

    public class ChannelInfo
    {
        public long ChannelId { get; set; }
        public string ChannelName { get; set; }
        public string IsPaicApi { get; set; }
    }

    /// <summary>
    /// 安心是否需要验车(0不强制要求验车 1系统要求验车 2核保要求验车)
    /// </summary>
    public class ValidateCar
    {
        public string ForceValidateCar { get; set; }
        public string BizValidateCar { get; set; }
        public string IsValidateCar { get; set; }
    }

    /// <summary>
    /// 人太平三家可以修改的折扣系数
    /// </summary>
    public class DiscountViewModel
    {
        /// <summary>
        /// 新的source值
        /// </summary>
        public long Source { get; set; }
        /// <summary>
        /// 渠道系数（目前只有平安，其他传0）
        /// </summary>
        public decimal CR { get; set; }
        /// <summary>
        /// 核保系数（目前只有平安，其他传0）
        /// </summary>
        public decimal SR { get; set; }
        /// <summary>
        /// 通融原因（目前只有平安，其他传""）
        /// </summary>
        public string TRCR { get; set; }
        /// <summary>
        /// 实际折扣系数（目前是人、太、平）
        /// </summary>
        public decimal AD { get; set; }
    }
}