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
    
    public partial class bx_car_claims
    {
        public long id { get; set; }
        public string policy_no { get; set; }
        public string report_no { get; set; }
        public string license_no { get; set; }
        public Nullable<System.DateTime> endcase_time { get; set; }
        public Nullable<System.DateTime> loss_time { get; set; }
        public Nullable<double> pay_amount { get; set; }
        public string pay_company_no { get; set; }
        public string pay_company_name { get; set; }
        public Nullable<int> pay_type { get; set; }
        public Nullable<System.DateTime> create_time { get; set; }
        public Nullable<int> RenewalCarType { get; set; }
        public Nullable<int> businesssource { get; set; }
        public string extentionid { get; set; }
    }
}
