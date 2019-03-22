using System;
using System.Web;
using ServiceStack.Text;

namespace BihuEyeSystem
{
    public class TraceModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += context_BeginRequest;
            context.EndRequest += context_EndRequest;
            
        }

        void context_EndRequest(object sender, EventArgs e)
        {
            var tracHeaderIdentity = HttpContext.Current.Request.Headers.Get("EyeTrace");
            if (!string.IsNullOrWhiteSpace(tracHeaderIdentity))
            {
                var tm = tracHeaderIdentity.FromJson<TraceModel>();
                tm.SId += 1;
                tm.ET = DateTime.Now;
                tm.Resp = HttpContext.Current.Request.ToString();
                tm.TimeTaken = (tm.ET - tm.BT).Milliseconds;
                HttpContext.Current.Response.Headers.Add("EyeTrace", tm.ToJson());
            }
           
        }

        void context_BeginRequest(object sender, EventArgs e)
        {
            var tracHeaderIdentity=HttpContext.Current.Request.Headers.Get("EyeTrace");
            if (string.IsNullOrWhiteSpace(tracHeaderIdentity))
            {
                TraceModel tm = new TraceModel()
                {
                    PId = Guid.NewGuid().ToString(),
                    SId = 0,
                    TId = 0,
                    BT = DateTime.Now,
                    Resp = HttpContext.Current.Request.ToString()
                };
                HttpContext.Current.Request.Headers.Add("EyeTrace",tm.ToJson());
            }
            else
            {
                var tm = tracHeaderIdentity.FromJson<TraceModel>();
                tm.SId += 1;
                tm.BT = DateTime.Now;
                tm.Resp = HttpContext.Current.Request.ToString();
                HttpContext.Current.Request.Headers.Add("EyeTrace", tm.ToJson());
            }
        }

        public void Dispose()
        {
            
        }
    }

    public class TraceModel
    {
        /// <summary>
        /// 唯一标识 第一次请求初始化
        /// </summary>
        public string PId { get; set; }
        /// <summary>
        /// 二级标示，跨请求的层级，自动加1
        /// </summary>
        public int SId { get; set; }
        /// <summary>
        /// 三级标示，方法内部标示，自动加1
        /// </summary>
        public int TId { get; set; }
        /// <summary>
        /// 服务器IP
        /// </summary>
        public string SIp { get; set; }
        /// <summary>
        /// 开始处理时间
        /// </summary>
        public DateTime BT { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime ET { get; set; }
        /// <summary>
        /// 请求内容
        /// </summary>
        public String Rest { get; set; }
        /// <summary>
        /// 响应内容
        /// </summary>
        public string Resp { get; set; }

        public int TimeTaken { get; set; }


    }
}