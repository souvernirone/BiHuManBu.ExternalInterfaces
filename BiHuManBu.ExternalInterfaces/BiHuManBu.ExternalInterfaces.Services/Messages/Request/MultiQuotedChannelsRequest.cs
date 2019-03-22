using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class MultiQuotedChannelsRequest:BaseRequest2
    {
        /// <summary>
        /// 城市编码
        /// </summary>
        public int CityCode { get; set; }
    }
}
