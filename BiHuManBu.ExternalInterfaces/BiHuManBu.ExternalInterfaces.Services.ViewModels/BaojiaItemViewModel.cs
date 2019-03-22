
using System;
using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Services.ViewModels
{

    public class BaojiaItemViewModel:BaseViewModel
    {
        public BaojiaInfoViewModel BaoJiaInfo { get; set; }
        public BaoxianXianZhongViewModel XianZhongInfo { get; set; }
        public int ClaimCount { get; set; }
        public List<ClaimDetailViewModel> ClaimDetail { get; set; }
        public AgentRateViewModel AgentRate { get; set; }
        public AgentRateViewModel ZhiKeRate { get; set; }
        /// <summary>
        /// 出险信息
        /// </summary>
        public ClaimInfo ClaimInfo { get; set; }

        /// <summary>
        /// 优惠活动详情列表
        /// </summary>
        public List<PreActivity> Activitys { get; set; }

        public int ActivityCount { get; set; }

        /// <summary>
        /// 业务员信息
        /// </summary>
        public AgentViewModelByBJ AgentDetail { get; set; }
    }

    public class ClaimInfo
    {
        /// <summary>
        /// 商业出险次数
        /// </summary>
        public int LossBizCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string LossBizAmount { get; set; }
        /// <summary>
        /// 交强出险次数
        /// </summary>
        public int LossForceCount { get; set; }
        /// <summary>
        /// 交强出险金额
        /// </summary>
        public string LossForceAmount { get; set; }


    }

    public partial class ClaimDetailViewModel
    {
        public long Id { get; set; }
        public Nullable<long> Liid { get; set; }
        public Nullable<long> Buid { get; set; }
        public Nullable<System.DateTime> EndCaseTime { get; set; }
        public string StrEndCaseTime { get; set; }
        public Nullable<System.DateTime> LossTime { get; set; }
        public string StrLossTime { get; set; }
        public Nullable<double> PayAmount { get; set; }
        public string PayCompanyNo { get; set; }
        public string PayCompanyName { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public string StrCreateTime { get; set; }
    }
    /// <summary>
    /// 打算抛弃ClaimDetailViewModel
    /// </summary>
    public partial class ClaimDetailNewViewModel
    {
        public string EndCaseTime { get; set; }
        public string LossTime { get; set; }
        public Nullable<double> PayAmount { get; set; }
        public string PayCompanyNo { get; set; }
        public string PayCompanyName { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public string StrCreateTime { get; set; }
        /// <summary>
        /// 出险类别，0=商业险，1=交强险
        /// </summary>
        public int PayType { get; set; }
    }

    public class QRCodeResponseViewModel : BaseViewModel
    {
        public long Id { get; set; }
    }
}
