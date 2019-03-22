
using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Services.ViewModels
{
    public class GetRepeatSubmitViewModel : BaseViewModel
    {
        public GetRepeatSubmitInfo RepeatInfo { get; set; }
    }

    public class GetRepeatSubmitInfo
    {
        public string ForceExpireDate { get; set; }
        public string BusinessExpireDate { get; set; }
        public int RepeatSubmitResult { get; set; }
        public string RepeatSubmitMessage { get; set; }
        /// <summary>
        /// 1：组合而成  0：否
        /// </summary>
        public int CompositeRepeatType { get; set; }
        public Dictionary<int, int> RepeatSubmitPerComp { get; set; } 
    }
}
