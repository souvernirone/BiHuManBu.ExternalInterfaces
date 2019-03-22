using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Services.ViewModels.CarVehicle
{
    public interface ICarVehicleInfoViewModel
    {
        /// <summary>
        /// 业务状态
        /// </summary>
        int BusinessStatus { get; set; }
        /// <summary>
        /// 自定义状态描述
        /// </summary>
        string StatusMessage { get; set; }

        List<CarVehicleItem> Items { get; set; }
        string CustKey { get; set; }
    }
}
