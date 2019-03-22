using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public  class MySqlMonitorRepository
    {
        public IEnumerable<MySqlMonitory> GetStatus()
        {
            var items = DataContextFactory.GetDataContext().Database.SqlQuery<MySqlMonitory>("show global status").ToList();
            return items;
        }
    }

    public class MySqlMonitory
    {
        public string Variable_name { get; set; }
        public string Value { get; set; }
    }

    public class MySqlMonitoryModel
    {
        public List<MySqlMonitory> List { get; set; } 
    }
}
