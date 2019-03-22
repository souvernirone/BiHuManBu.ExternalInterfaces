using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Services.Messages.RemoteMessage;
using BiHuManBu.ExternalInterfaces.Services.ViewModels.CarVehicle;

namespace BiHuManBu.ExternalInterfaces.Services.Mapper
{
    public static  class NewBxCarVehicleInfoMapper
    {
        public static NewCarVehicleDetail ConvertToCarVehicle(this NewBxCarVehicleInfo info, int type)
        {
            var yearday = string.Empty;
            if (string.IsNullOrWhiteSpace(info.VehicleYear))
            {
                yearday = string.Empty;
            }
            else if (info.VehicleYear.Length == 4)
            {
                yearday = info.VehicleYear;
            }
            else if (info.VehicleYear.Length >= 6)
            {
                yearday = info.VehicleYear.Substring(0, 6);
            }
            var  item = new NewCarVehicleDetail
            {
                VehicleName = info.VehicleName,
                VehicleAlias = info.VehicleAlias,
                VehicleExhaust = info.VehicleExhaust.ToString(CultureInfo.InvariantCulture),
                VehicleSeat = info.VehicleSeat.ToString(CultureInfo.InvariantCulture),
                PurchasePrice = info.PriceT.ToString(CultureInfo.InvariantCulture),
                VehicleYear = yearday,
                VehicleNo = info.VehicleNo,
                Source = info.SourceFlags,
                SourceName = info.SourceNames??string.Empty
            };
            return item;
        }

        public static List<NewCarVehicleDetail> ConvertToCarVehicleItems(this List<NewBxCarVehicleInfo> infos, int type = 0)
        {
            return infos.Select(info => info.ConvertToCarVehicle(type)).ToList();
        }
    }
}
