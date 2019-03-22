using System;
using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Services.ViewModels
{
    /// <summary>
    /// 续保列表
    /// </summary>
    public class ReInfoListViewModel:BaseViewModel
    {
        public int TotalCount { get; set; }
        public List<ReInfo> ReInfoList { get; set; }
    }
    
    public class ReInfo:BaseViewModel
    {
        public string CreateTime { get; set; }
        public int? Intention_View { get; set; }
        /// <summary>
        /// 当前记录拥有者，bx_userinfo的anget
        /// </summary>
        public string Agent { get; set; }
        /// <summary>
        /// 车牌录入者
        /// </summary>
        public int SaAgent { get; set; }
        /// <summary>
        /// 是否已分配
        /// </summary>
        public int IsDistrib { get; set; }

        public long? Buid { get; set; }
        public UserInfoViewModel UserInfo { get; set; }
    }


    public class ReListViewModel : BaseViewModel
    {
        public int TotalCount { get; set; }
        public List<Re> ReList { get; set; }
    }
    public class Re:BaseViewModel
    {
        public long Buid { get; set; }
        public int? LastYearSource { get; set; }
        public int? RenewalStatus { get; set; }
        public string CreateTime { get; set; }
        public UserInfoViewModel UserInfo { get; set; }
    }
}
