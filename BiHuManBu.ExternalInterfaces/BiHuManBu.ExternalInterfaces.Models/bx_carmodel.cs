//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace BiHuManBu.ExternalInterfaces.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class bx_carmodel
    {
        public long Id { get; set; }
        public string VehicleName { get; set; }
        public string VehicleNo { get; set; }
        public Nullable<int> VehicleSeat { get; set; }
        public Nullable<decimal> VehicleQuality { get; set; }
        public Nullable<decimal> mass { get; set; }
        public Nullable<decimal> VehicleExhaust { get; set; }
        public Nullable<decimal> PriceT { get; set; }
        public Nullable<decimal> PriceTr { get; set; }
        public string VehicleYear { get; set; }
        public string Risk { get; set; }
        public string VehicleAlias { get; set; }
        public string Manufacturer { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
        public string PingAnVehicleNo { get; set; }
    }
}
