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
    
    public partial class bx_sms_account_content
    {
        public int id { get; set; }
        public Nullable<int> agent_id { get; set; }
        public string sent_mobile { get; set; }
        public string content { get; set; }
        public Nullable<System.DateTime> create_time { get; set; }
        public string agent_name { get; set; }
        public Nullable<int> sent_type { get; set; }
        public string license_no { get; set; }
        public Nullable<int> business_type { get; set; }
        public int BatchId { get; set; }
        public int sendstatus { get; set; }
        public int isdelete { get; set; }
    }
}