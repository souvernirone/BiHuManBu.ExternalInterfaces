using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.ViewModels.IndependentViewModel
{
    /// <summary>
    /// 基本信息
    /// </summary>
    public class Msg
    {
        /// <summary>
        /// 失败原因
        /// </summary>
        public string ErrorMsg { get; set; }

        /// <summary>
        /// 是否成功  1:是、0:否
        /// </summary>
        public string Flag { get; set; }
    }
}
