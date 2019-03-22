using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Response
{
    public class GetChannelStatusResponse:BaseResponse
    {
        public List<AgentCacheChannelModel> CacheChannelList;
    }
    public class GetSingelChannelStatusResponse : BaseResponse
    {
        public AgentCacheChannelModel CacheChannel;
    }

    /// <summary>
    ///缓存渠道模型
    /// </summary>
    public partial class CacheChannelModel
    {
        public CacheChannelModel()
        {
            ExcuteByCountType = new Dictionary<sbyte, List<long>>();
            ExcuteTimesByCountType = new Dictionary<sbyte, sbyte>();
        }

        /// <summary>
        ///*保险公司类型
        /// </summary>
        public int Source { get; set; }

        /// <summary>
        ///*所属城市
        /// </summary>
        public int City { get; set; }

        /// <summary>
        ///*1为url 2为macurl
        /// </summary>
        public int IsUrl { get; set; }

        /// <summary>
        ///*url请求地址,网站需要
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        ///*mac请求地址,服务需要
        /// </summary>
        public string MacUrl { get; set; }

        /// <summary>
        ///*是否加入调度
        /// </summary>
        public int IsUseDeploy { get; set; }

        /// <summary>
        ///当前渠道是否可用
        /// </summary>
        public int IsUse { get; set; }

        /// <summary>
        ///当前已占用
        /// </summary>
        public int CurrentOccupancy { get; set; }

        /// <summary>
        ///累计执行次数-自己走自己的
        /// </summary>
        public int ExcuteTimesMyself { get; set; }

        /// <summary>
        ///累计执行次数-别人走自己的
        /// </summary>
        public int ExcuteTimesHimself { get; set; }

        /// <summary>
        ///有的请求需要额外计数,用以控制多长时间此类请求最多几次
        /// </summary>
        public Dictionary<sbyte, List<long>> ExcuteByCountType { get; set; }

        public Dictionary<sbyte, sbyte> ExcuteTimesByCountType { get; set; }
    }

    
    /// <summary>
    ///缓存渠道模型
    /// </summary>
    public partial class AgentCacheChannelModel
    {

        /// <summary>
        /// 渠道ID
        /// </summary>
        public long ChannelId { get; set; }
        /// <summary>
        /// 渠道名称
        /// </summary>
        public string ChannelName { get; set; }

        public AgentCacheChannelModel()
        {
            ExcuteByCountType = new Dictionary<sbyte, List<long>>();
            ExcuteTimesByCountType = new Dictionary<sbyte, sbyte>();
        }

        /// <summary>
        ///*保险公司类型
        /// </summary>
        public int Source { get; set; }

        /// <summary>
        ///*所属城市
        /// </summary>
        public int City { get; set; }

        /// <summary>
        ///*1为url 2为macurl
        /// </summary>
        public int IsUrl { get; set; }

        /// <summary>
        ///*url请求地址,网站需要
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        ///*mac请求地址,服务需要
        /// </summary>
        public string MacUrl { get; set; }

        /// <summary>
        ///*是否加入调度
        /// </summary>
        public int IsUseDeploy { get; set; }

        /// <summary>
        ///当前渠道是否可用
        /// </summary>
        public int IsUse { get; set; }

        /// <summary>
        ///当前已占用
        /// </summary>
        public int CurrentOccupancy { get; set; }

        /// <summary>
        ///累计执行次数-自己走自己的
        /// </summary>
        public int ExcuteTimesMyself { get; set; }

        /// <summary>
        ///累计执行次数-别人走自己的
        /// </summary>
        public int ExcuteTimesHimself { get; set; }

        /// <summary>
        ///有的请求需要额外计数,用以控制多长时间此类请求最多几次
        /// </summary>
        public Dictionary<sbyte, List<long>> ExcuteByCountType { get; set; }

        public Dictionary<sbyte, sbyte> ExcuteTimesByCountType { get; set; }

        public int? IsPaicApi { get; set; }
    }
}
