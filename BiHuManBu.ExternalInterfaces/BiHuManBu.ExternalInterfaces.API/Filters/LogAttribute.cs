using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using log4net;
using log4net.Repository.Hierarchy;
using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.LogBuriedPoint.LogCollection;
using Newtonsoft.Json;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.API.Filters
{
    public class LogAttribute : ActionFilterAttribute
    {
        public string BusinessName { get; set; }//业务名
        public string StepName { get; set; }//步骤名

        public int ExecuteLot { get; set; }//步骤执行序号
        private Stopwatch _stopwatch;
        public LogAttribute(string businessName, string stepName, int executeLot)
        {
            BusinessName = businessName;
            StepName = stepName;
            ExecuteLot = executeLot;
        }
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
            ///生成TopTID  
            if (!actionContext.Request.Headers.Contains("TopTID"))
            {
                LogAssistant.GenerateTopTIDToRequestHeader(BusinessName, actionContext);
            }
            ///生成Traceid
            LogAssistant.GenerateTraceIDToRequestHeader(BusinessName, actionContext);

        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (!string.IsNullOrEmpty(BusinessName))
            {
                _stopwatch.Stop();
                long executeElapsedTime = _stopwatch.ElapsedMilliseconds;
                string request = LogAssistant.GetRequestParameter(actionExecutedContext);
                var response = actionExecutedContext.Response;
                if (response != null)
                {
                    try
                    {
                        var result = response.Content.ReadAsStringAsync().Result;
                        var briefResult = JsonConvert.DeserializeObject<BaseViewModel>(result);
                        if (briefResult.BusinessStatus != -10003)
                        {
                            AspectFExtensions.LogBuriedPoint(actionExecutedContext, BusinessName, StepName, ExecuteLot, executeElapsedTime, request, result, true, IpAddressHelper.GetIPAddress(), "服务器响应成功");
                        }
                        else
                        {
                            AspectFExtensions.LogBuriedPoint(actionExecutedContext, BusinessName, StepName, ExecuteLot, executeElapsedTime, request, result, false, IpAddressHelper.GetIPAddress(), "服务器响应失败");
                        }

                    }
                    catch (Exception ex)
                    {
                        AspectFExtensions.LogBuriedPoint(actionExecutedContext, BusinessName, StepName, ExecuteLot, executeElapsedTime, request, ex.Message, false, IpAddressHelper.GetIPAddress(), "接口执行成功，OnActionExecuted执行失败");
                    }
                }
                else
                {
                    AspectFExtensions.LogBuriedPoint(actionExecutedContext, BusinessName, StepName, ExecuteLot, executeElapsedTime, request, "", false, IpAddressHelper.GetIPAddress(), "服务器没有返回任何数据,请对接口代码进行异常捕获");
                }

            }

        }

    }

}
