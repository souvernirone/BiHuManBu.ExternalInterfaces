using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Services.ViewModels
{
    public class GetChannelStatusViewModel : BaseViewModel
    {
        public List<ChannelStatusModel> ChannelList { get; set; }
    }

    public class ChannelStatusModel : ChannelStatusBaseModel
    {
    }
    public class ChannelStatusModel2 :BaseViewModel
    {
        public ChannelStatusBaseModel StatusModel { get; set; }
    }

    public class ChannelStatusBaseModel
    {
        /// <summary>
        /// 顶级代理Id
        /// </summary>
        public int Agent { get; set; }
        /// <summary>
        /// 城市渠道
        /// </summary>
        public int CityCode { get; set; }
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
        /// 渠道枚举值(新)
        /// </summary>
        public long Source { get; set; }
        /// <summary>
        /// 渠道名称
        /// </summary>
        public string SourceName { get; set; }
        /// <summary>
        /// 渠道ID
        /// </summary>
        public long ChannelId { get; set; }
        /// <summary>
        /// 渠道名称
        /// </summary>
        public string ChannelName { get; set; }
        /// <summary>
        /// 是否平安接口标识1是0否 需要请求参数拉取
        /// </summary>
        public string IsPaicApi { get; set; }
    }
}
