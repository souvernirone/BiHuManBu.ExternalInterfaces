namespace BiHuManBu.ExternalInterfaces.Services.ViewModels
{
    public class SubmitInfoViewModel : BaseViewModel
    {
        public SubmitInfoDetail Item { get; set; }
        public string CustKey { get; set; }
        public string OrderId { get; set; }
        public string CheckCode { get; set; }

    }
    public class SubmitInfoNewViewModel : AppBaseViewModel
    {
        public SubmitInfoDetail Item { get; set; }
        public string OrderId { get; set; }
        public string CheckCode { get; set; }

    }

    public class SubmitInfoDetail
    {
        public int Source { get; set; }
        /// <summary>
        /// 核保状态
        /// </summary>
        public int SubmitStatus { get; set; }
        /// <summary>
        /// 核保状态描述
        /// </summary>
        public string SubmitResult { get; set; }
        /// <summary>
        /// 商业险投保单号
        /// </summary>
        public string BizNo { get; set; }
        /// <summary>
        /// 交强险投保单号
        /// </summary>
        public string ForceNo { get; set; }
        /// <summary>
        /// 商业险费率
        /// </summary>
        public double BizRate { get; set; }
        /// <summary>
        /// 交强车船费率
        /// </summary>
        public double ForceRate { get; set; }
        /// <summary>
        /// 核保渠道id
        /// </summary>
        public string ChannelId { get; set; }
        /// <summary>
        /// 精算口径
        /// </summary>
        public string JingSuanKouJing { get; set; }
        /// <summary>
        /// 安心报价核保流程,支付预定单号
        /// </summary>
        public string OrderNo { get; set; }
    }
}