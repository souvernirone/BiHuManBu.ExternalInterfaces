using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Services.Messages.RemoteMessage;
using BiHuManBu.ExternalInterfaces.Services.ViewModels.CarVehicle;

namespace BiHuManBu.ExternalInterfaces.Services.Mapper
{
     public static  class CarVehicleMapper
    {
         public static ICarVehicleItem ConvertToCarVehicle(this BxCarVehicleInfo info,int type)
         {
             if (info == null)
             {
                 throw new ArgumentException("车型参数为空");
             }
             ICarVehicleItem item;

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
             if (type == 0)
             {
                 item = new CarVehicleItem
                 {
                     Info =
                         string.Format("{0}/{1}/{2}/{3}/{4}/{5}", info.VehicleName, info.VehicleAlias,
                             info.VehicleExhaust, info.VehicleSeat, info.PriceT, yearday),
                     VehicleNo = info.VehicleNo,
                     PurchasePrice = info.PriceT.ToString(CultureInfo.InvariantCulture),
                     VehicleAlias = null,
                     VehicleExhaust = null,
                     VehicleName = null,
                     VehicleSeat = null,
                     VehicleYear = null
                 };
             }else
             {
                 item = new CarVehicleDetail
                 {
                     VehicleName = info.VehicleName,
                     VehicleAlias = info.VehicleAlias,
                     VehicleExhaust = info.VehicleExhaust.ToString(CultureInfo.InvariantCulture),
                     VehicleSeat = info.VehicleSeat.ToString(CultureInfo.InvariantCulture),
                     PurchasePrice = info.PriceT.ToString(CultureInfo.InvariantCulture),
                     //VehicleYear = string.IsNullOrWhiteSpace(info.VehicleYear) ? string.Empty : info.VehicleYear.Substring(0, 4),
                     VehicleNo = info.VehicleNo,
                     //CarType = info.VehicleClassType,
                     //TypeName = info.VehicleClassName,
                     Info = null
                 };
                 item.VehicleYear = yearday;
             }
             return item;
         }

         public static List<ICarVehicleItem> ConvertToCarVehicleItems(this List<BxCarVehicleInfo> infos,int type=0)
         {
             return infos.Select(info => info.ConvertToCarVehicle(type)).ToList();
         }
    }
}
