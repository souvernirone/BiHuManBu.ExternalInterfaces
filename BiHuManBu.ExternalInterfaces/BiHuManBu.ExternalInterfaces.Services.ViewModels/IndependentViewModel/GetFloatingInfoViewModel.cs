using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Services.ViewModels.IndependentViewModel.FloatingInfoModel;

namespace BiHuManBu.ExternalInterfaces.Services.ViewModels.IndependentViewModel
{
    public class GetFloatingInfoViewModel : BaseViewModel
    {
        /// <summary>
        /// 车辆信息
        /// </summary>
        public CarInfo CarInfo { get; set; }

        /// <summary>
        /// 交强浮动通知列表
        /// </summary>
        public ForceFloatingInfo ForceFloatingInfo { get; set; }

        /// <summary>
        /// 商业浮动通知列表
        /// </summary>
        public BizFloatingInfo BizFloatingInfo { get; set; }
    }
}
