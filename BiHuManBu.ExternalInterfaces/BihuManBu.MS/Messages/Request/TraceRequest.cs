namespace BihuManBu.MS.Messages.Request
{
    public class TraceRequest
    {
        public string LicenseNo { get; set; }
        public long Agent { get; set; }
        /// <summary>
        /// 下级 child
        /// 或者 custkey（对外）
        /// </summary>
        public string ChildAgent { get; set; }
        /// <summary>
        /// 一次全新的跟踪链
        /// </summary>
        public string RootId { get; set; }
        /// <summary>
        /// 每个action内的一个唯一标示，它来源于上一步的childRootId，需要手动+1.
        /// </summary>
        public string ChildRootId { get; set; }
        /// <summary>
        /// 每个action内，需要记录多个步骤的话，可以用 x.1,x.2的形式记录步奏
        /// </summary>
        public double Steps { get; set; }
        /// <summary>
        /// 服务器ip
        /// </summary>
        public string ServerIp { get; set; }
        /// <summary>
        /// 请求或者响应时间，用来分析性能
        /// </summary>
        public string RstOrRspTime { get; set; }
        /// <summary>
        /// 统计每个action需要消耗的时间，可以在action内的加入 stopwatch计数器
        /// </summary>
        public double ExecuteTime { get; set; }
        /// <summary>
        /// 记录的消息体，包括请求参数，响应内容，及错误日志
        /// </summary>
        public string MessageBody { get; set; }
        /// <summary>
        /// 每个action的名字
        /// </summary>
        public string ActionName { get; set; }

    }
}