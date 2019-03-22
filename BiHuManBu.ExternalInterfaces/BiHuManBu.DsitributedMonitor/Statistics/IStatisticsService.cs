using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.DsitributedMonitor.Statistics
{
    public interface IStatisticsService
    {
        void Add(int agent);
    }
}
