using BiHuManBu.ExternalInterfaces.Models.ReportModel;
using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Services.ViewModels
{
    public class GetReInfoViewModel:BaseViewModel
    {
        public UserInfoViewModel UserInfo { get; set; }
        public SaveQuoteViewModel SaveQuote { get; set; }
        public PACheckCode PACheckCode { get; set; }
        /// <summary>
        /// 过户车的模型
        /// </summary>
        public List<TransferModelNew> TransferModelList { get; set; }
        public XianZhong XianZhong { get; set; }
        public string CustKey { get; set; }
    }

    public class GetReInfoNewViewModel : AppBaseViewModel
    {
        public UserInfoViewModel UserInfo { get; set; }
        public SaveQuoteViewModel SaveQuote { get; set; }
        public string CreateTime { get; set; }
        /// <summary>
        /// 是否已分配
        /// </summary>
        public int IsDistrib { get; set; }
    }

    public class AppReInfoViewModel : BaseViewModel
    {
        public UserInfoViewModel UserInfo { get; set; }
        public SaveQuoteViewModel SaveQuote { get; set; }
        public WorkOrderViewModel WorkOrder { get; set; }
        public List<WorkOrderDetail> DetailList { get; set; }
        /// <summary>
        /// 当前记录拥有者，bx_userinfo的anget
        /// </summary>
        public int Agent { get; set; }
        public string AgentName { get; set; }
        /// <summary>
        /// 车牌录入者
        /// </summary>
        public int SaAgent { get; set; }
        public string SaAgentName { get; set; }
        /// <summary>
        /// 是否已分配
        /// </summary>
        public int IsDistrib { get; set; }
        public long? Buid { get; set; }
        public string CustKey { get; set; }
    }
    public class PACheckCode
    {
        #region 平安验证码
        /// <summary>
        /// 平安校验图片，返回base64串
        /// </summary>
        public string VerificationCode { get; set; }
        /// <summary>
        /// 上次请求的渠道UKey信息（和RequestKey 保持原有会话）
        /// </summary>
        public int PAUKey { get; set; }
        /// <summary>
        /// 上次请求的渠道RequestKey信息（和UKey 保持原有会话）
        /// </summary>
        public string RequestKey { get; set; }
        #endregion
    }
}