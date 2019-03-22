using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class AddressRequest
    {
        public int id { get; set; }
        public Nullable<int> userid { get; set; }
        public string Name { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public Nullable<int> provinceId { get; set; }
        public Nullable<int> cityId { get; set; }
        public Nullable<int> areaId { get; set; }
        public Nullable<int> agentId { get; set; }

        public string openId { get; set; }

        /// <summary>
        /// 当前代理人Id。微信新增字段
        /// 20170228账号统一，openid查询改为childagent查询
        /// </summary>
        [Range(1, 1000000)]
        public int ChildAgent { get; set; }
    }
}
