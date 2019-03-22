using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Metrics;
using Metrics.InfluxDB;
using Metrics.InfluxDB.Adapters;

namespace MetricsLibrary
{
    public class MetricUtil
    {
        public static void UnitReports(string metricsName)
        {
           var  _metricsName = metricsName;
          var   _address = ConfigurationManager.AppSettings["MetricsAddress"];
           var  _db = ConfigurationManager.AppSettings["MetricsDbName"];

            if (!string.IsNullOrWhiteSpace(_metricsName) && !string.IsNullOrWhiteSpace(_db))
            {
                Metric.Config
                    .WithReporting(report => report
                        .WithInfluxDbHttp(_address, _db, TimeSpan.FromSeconds(2), null, c => c
                            .WithConverter(new DefaultConverter().WithGlobalTags("host=" + Dns.GetHostAddresses(Dns.GetHostName()).LastOrDefault() + ",env=dev"))
                                .WithFormatter(new DefaultFormatter().WithLowercase(true))
                                         .WithWriter(new InfluxdbHttpWriter(c, 1))));
                Metric.Gauge(_metricsName+"_error", () => 1, Unit.Errors);
            }
          
        }
    }
}
