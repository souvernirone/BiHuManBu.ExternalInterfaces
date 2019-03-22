using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Models.ReportModel
{
    /// <summary>
    /// bx_agent_config表新增定制模型
    /// </summary>
    public class MultiChannelsModel
    {
        public int? Source { get; set; }
        public long ChannelId { get; set; }
    }
}
