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
    
    public partial class bx_agent_config
    {
        public long id { get; set; }
        public Nullable<int> agent_id { get; set; }
        public string agent_account { get; set; }
        public string bx_url { get; set; }
        public string bx_user { get; set; }
        public string bx_sysname { get; set; }
        public Nullable<int> source { get; set; }
        public string dev_pin { get; set; }
        public string user_pin { get; set; }
        public Nullable<System.DateTime> create_time { get; set; }
        public Nullable<System.DateTime> update_time { get; set; }
        public Nullable<int> ukey_id { get; set; }
        public Nullable<int> reconciliation { get; set; }
        public Nullable<int> city_id { get; set; }
        public string config_name { get; set; }
        public Nullable<int> is_used { get; set; }
        public Nullable<int> is_duizhang { get; set; }
        public Nullable<int> isusedeploy { get; set; }
        public string macurl { get; set; }
        public Nullable<int> isurl { get; set; }
        public int isSubmit { get; set; }
        public int is_paic_api { get; set; }
    }
}