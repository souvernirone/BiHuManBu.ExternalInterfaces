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
    
    public partial class bx_devicedetail
    {
        public long id { get; set; }
        public Nullable<long> b_uid { get; set; }
        public string device_name { get; set; }
        public Nullable<int> device_quantity { get; set; }
        public Nullable<double> device_amount { get; set; }
        public Nullable<System.DateTime> purchase_date { get; set; }
        public Nullable<double> device_depreciationamount { get; set; }
        public Nullable<int> device_type { get; set; }
    }
}
