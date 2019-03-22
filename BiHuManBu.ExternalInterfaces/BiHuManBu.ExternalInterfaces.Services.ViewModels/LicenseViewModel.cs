using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.ViewModels
{
    public class LicenseViewModel
    {
        public string LicenseNo { get; set; }
        public string CarVin { get; set; }
        public string EngineNo { get; set; }
        public string MoldName { get; set; }
        public string RegisterDate { get; set; }
        public string CarOwnersName { get; set; }
        /// <summary>
        /// 车辆种类
        /// </summary>
        public int CarType { get; set; }
        /// <summary>
        /// 使用性质
        /// </summary>
        public int CarUsedType { get; set; }
        /// <summary>
        /// 座位数
        /// </summary>
        public int SeatCount { get; set; }
        /// <summary>
        /// 大车还是小车
        /// </summary>
        public int RenewalCarType { get; set; }
        /// <summary>
        /// 排量
        /// </summary>
        public string ExhaustScale { get; set; }
    }
}
