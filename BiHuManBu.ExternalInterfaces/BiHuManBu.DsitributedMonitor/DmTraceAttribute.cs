using System;
using System.Web.Http.Filters;
using log4net;

namespace BiHuManBu.DsitributedMonitor
{
    public class DmTraceAttribute:ActionFilterAttribute
    {
        static object _lockObj = new object();
        private static ILog log = LogManager.GetLogger("TRACE");
        private DateTime startTime;
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            
            startTime = DateTime.Now;
            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            int outSeconds;
            int.TryParse((System.Configuration.ConfigurationManager.AppSettings["ActionTrace"] ?? "60"), out outSeconds);

            var timeSpan = (DateTime.Now - startTime).Seconds;
            
                lock (_lockObj)
                {
                    //IO操作  以后缓存，报表
                    var info = string.Format("Action请求串：{0}，请求时间{1}秒", actionExecutedContext.ActionContext.Request.RequestUri,
                        timeSpan);
                    log.Info(info);

                }
            base.OnActionExecuted(actionExecutedContext);
        }

        private DmContext GetContext(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            DmContext dm = null;
            try
            {
              var item=  actionContext.Request.Headers.GetValues("DM");
            }
            catch (Exception)
            {
                var id = System.Guid.NewGuid().ToString();

                dm = new DmContext()
                {
                    RootId = id,
                    ParentId = 1,
                    ChildId = 1
                };
            }
            return dm;

        }
    }
}
