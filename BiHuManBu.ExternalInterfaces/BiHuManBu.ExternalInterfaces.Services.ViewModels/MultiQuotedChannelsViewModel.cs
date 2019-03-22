using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.ViewModels
{
    public class ResponseMultiQuotedChannelsViewModel : BaseViewModel
    {
        /// <summary>
        /// 代理人渠道集合
        /// </summary>
        public List<MultiQuotedChannelsViewModel> ListModels { get; set; }
    }

    public class MultiQuotedChannelsViewModel
    {
        /// <summary>
        /// 渠道ID
        /// </summary>
        public long ChannelId { get; set; }
        /// <summary>
        /// 渠道名称
        /// </summary>
        public string ChannelName { get; set; }
        /// <summary>
        /// 保险系统来源，0=平安，1=太平洋，2=人保
        /// </summary>
        public long Source { get; set; }
        /// <summary>
        /// 渠道状态
        /// 正常 = 0,无法连接 = 1,版本号不一致 = 2,超过并发上限 = 3,登录失败 = 4,执行错误 = 97,vpn无法连接 = 98,其它配置错误 = 99,关机 = 100,未知状态 = 101
        /// </summary>
        public int ChannelStatus { get; set; }
        /// <summary>
        /// 渠道状态对应的中文解释
        /// </summary>
        public string ChannelStatusMessage { get; set; }
        /// <summary>
        /// 1是平安接口标识 0否
        /// </summary>
        public string IsPaicApi { get; set; }
    }

    public class MultiQuotedChannelViewModel : BaseViewModel
    {
        /// <summary>
        /// 渠道ID
        /// </summary>
        public long ChannelId { get; set; }
    }
}
