using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.ViewModels.IndependentViewModel
{
    public class JSFloatingNotificationPrintListResponseMain
    {
        /// <summary>
        /// 交强浮动通知列表
        /// </summary>
        public JQFloatingNotificationList JQFloatingNotificationList { get; set; }

        /// <summary>
        /// 商业浮动通知列表
        /// </summary>
        public SYFloatingNotificationList SYFloatingNotificationList { get; set; }

    }
}
