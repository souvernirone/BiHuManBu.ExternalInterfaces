using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Services.ViewModels
{
    public class GetCreaditInfoViewModel:BaseViewModel
    {
        public List<UserClaimViewModel> List { get; set; } 
    }
    public class GetCreaditDetailInfoViewModel : BaseViewModel
    {
        public List<UserClaimDetailViewModel> List { get; set; }

        /// <summary>
        /// 总出险次数20181106add
        /// </summary>
        public string ClaimCount { get; set; }
        /// <summary>
        /// 交强出险次数20181106add
        /// </summary>
        public string ForceCliamCount { get; set; }
        /// <summary>
        /// 商业出险次数20181106add
        /// </summary>
        public string BizClaimCount { get; set; }
    }
}