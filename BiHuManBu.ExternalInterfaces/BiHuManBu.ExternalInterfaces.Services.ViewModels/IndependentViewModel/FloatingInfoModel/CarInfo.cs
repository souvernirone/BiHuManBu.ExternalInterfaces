using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.ViewModels.IndependentViewModel.FloatingInfoModel
{
    public class CarInfo
    {
        /// <summary>
        /// 车辆厂牌型号
        /// </summary>
        public string CModelNmeJY { get; set; }

        /// <summary>
        /// 车辆类型
        /// </summary>
        public string CarType { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        public string CPlateNo { get; set; }

        /// <summary>
        /// 车牌种类
        /// </summary>
        public string CPlateTyp { get; set; }

        /// <summary>
        /// 发动机号码
        /// </summary>
        public string CEngNo { get; set; }

        /// <summary>
        /// 车架号
        /// </summary>
        public string CFrmNo { get; set; }

        /// <summary>
        /// 车主姓名
        /// </summary>
        public string CRegOwner { get; set; }
    }
}
