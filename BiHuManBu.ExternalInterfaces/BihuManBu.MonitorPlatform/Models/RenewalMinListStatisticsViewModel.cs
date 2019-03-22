using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BihuManBu.MonitorPlatform.Models
{
    public class RenewalMinListStatisticsViewModel
    {
        public List<MinStatistics> Statistics { get; set; }
    }

    public class MinStatistics
    {
        public string Time { get; set; }
        public double Count { get; set; }
    }
}