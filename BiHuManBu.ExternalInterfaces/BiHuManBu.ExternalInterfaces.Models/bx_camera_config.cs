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
    
    public partial class bx_camera_config
    {
        public int id { get; set; }
        public string camera_id { get; set; }
        public string park_id { get; set; }
        public Nullable<int> cityid { get; set; }
        public string seccode { get; set; }
        public Nullable<System.DateTime> createtime { get; set; }
        public string charge_person { get; set; }
        public string remind_phone { get; set; }
        public Nullable<int> cqa_user_id { get; set; }
        public Nullable<int> IsFilteringOld { get; set; }
        public Nullable<int> IsDeleteFailed { get; set; }
        public Nullable<int> Days { get; set; }
        public Nullable<int> IsRemind { get; set; }
        public string CameraName { get; set; }
    }
}