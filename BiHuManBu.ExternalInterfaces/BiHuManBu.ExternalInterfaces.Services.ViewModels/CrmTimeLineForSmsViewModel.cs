using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.ViewModels
{
    public class CrmTimeLineForSmsViewModel
    {
        public int Id { get; set; }
        public Nullable<int> agent_id { get; set; }
        public string sent_mobile { get; set; }
        public string content { get; set; }
        public Nullable<System.DateTime> create_time { get; set; }
        public string agent_name { get; set; }
        public Nullable<int> sent_type { get; set; }
        public string license_no { get; set; }
        public Nullable<int> business_type { get; set; }

        public long Source { get; set; }
        public string sourceName { get; set; }
        public double BizRate { get; set; }
        public double ForceRate { get; set; }

        /// <summary>
        /// 优惠活动ID
        /// </summary>
        public int ActivityId { get; set; }

        /// <summary>
        /// 报价单信息ID
        /// </summary>
        public long Bxid { get; set; }

        /// <summary>
        /// 城市Code
        /// </summary>
        public int CityId { get; set; }
    }
}
