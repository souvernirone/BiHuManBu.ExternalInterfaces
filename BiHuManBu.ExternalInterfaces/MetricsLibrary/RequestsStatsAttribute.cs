using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Metrics;
using Metrics.InfluxDB;
using Metrics.InfluxDB.Adapters;

namespace MetricsLibrary
{
    /// <summary>
    /// 统计请求速率,总次数
    /// </summary>
    public  class RequestsStatsAttribute : ActionFilterAttribute
    {
       
        private readonly string _metricsName;
        private string _address;
        private string _db;
        private Counter _totalRequestsCounter;
        private Meter _meter ;
        public RequestsStatsAttribute(string metricsName)
        {
            _metricsName = metricsName;
         
            _address = ConfigurationManager.AppSettings["MetricsAddress"];
            _db = ConfigurationManager.AppSettings["MetricsDbName"];
        }
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (string.IsNullOrWhiteSpace(_address) || string.IsNullOrWhiteSpace(_db))
            {

            }
            else
            {
                Metric.Config
                  .WithReporting(report => report
                      .WithInfluxDbHttp(_address, _db, TimeSpan.FromSeconds(2), null, c => c
                             .WithConverter(new DefaultConverter().WithGlobalTags("host=" + Dns.GetHostAddresses(Dns.GetHostName()).LastOrDefault() + ",env=dev"))
                                     .WithFormatter(new DefaultFormatter().WithLowercase(true))
                                              .WithWriter(new InfluxdbHttpWriter(c, 1))));
                _totalRequestsCounter = Metric.Counter(_metricsName + "_counter", Unit.Requests);
                _totalRequestsCounter.Increment();
                _meter = Metric.Meter(_metricsName + "_rate", Unit.Requests);
                _meter.Mark(); 
            }

            
            base.OnActionExecuting(actionContext);
        }
    }

    /// <summary>
    /// 统计tps
    /// </summary>
    public class TpsStatsAttribute : ActionFilterAttribute
    {
        Counter _tpsRequestsCounter ;
        private Counter _totalRequestCounter;
        private readonly string _metricsName;
        private string _address;
        private string _db;
        public TpsStatsAttribute(string metricsName)
        {
            _metricsName = metricsName;
            _address = ConfigurationManager.AppSettings["MetricsAddress"];
            _db = ConfigurationManager.AppSettings["MetricsDbName"];
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (string.IsNullOrWhiteSpace(_address) || string.IsNullOrWhiteSpace(_db))
            {
                //do nothing....
            }
            else
            {
                Metric.Config
                .WithReporting(report => report
                    .WithInfluxDbHttp(_address, _db, TimeSpan.FromSeconds(2), null, c => c
                           .WithConverter(new DefaultConverter().WithGlobalTags("host=" + Dns.GetHostAddresses(Dns.GetHostName()).LastOrDefault() + ",env=dev"))
                                   .WithFormatter(new DefaultFormatter().WithLowercase(true))
                                            .WithWriter(new InfluxdbHttpWriter(c, 1))));
                _tpsRequestsCounter = Metric.Counter(_metricsName + "_tps", Unit.Requests);
                _tpsRequestsCounter.Increment();
            }
           
            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (string.IsNullOrWhiteSpace(_address) || string.IsNullOrWhiteSpace(_db))
            {
                //do nothing....
            }
            else
            {
                _tpsRequestsCounter.Decrement();
            }
           
            base.OnActionExecuted(actionExecutedContext);
        }
    }
}
