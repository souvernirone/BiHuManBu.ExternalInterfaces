using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using log4net;
using log4net.Repository.Hierarchy;

namespace BiHuManBu.ExternalInterfaces.API.Filters
{
    public class ActionTraceAttribute : ActionFilterAttribute
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
            if (timeSpan >= outSeconds)
            {
                lock (_lockObj)
                {
                    //IO操作  以后缓存，报表
                    var info = string.Format("Action请求串：{0}，请求时间{1}秒", actionExecutedContext.ActionContext.Request.RequestUri,
                        timeSpan);
                    log.Info(info);
                    
                }
            }

            base.OnActionExecuted(actionExecutedContext);
        }
    }
}