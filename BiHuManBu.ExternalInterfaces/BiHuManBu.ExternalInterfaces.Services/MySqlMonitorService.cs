using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Repository;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;

namespace BiHuManBu.ExternalInterfaces.Services
{
    public class MySqlMonitorService:IMySqlMonitorService
    {
        private  static MySqlMonitorRepository _monitorRepository = new MySqlMonitorRepository();
        public IEnumerable<MySqlMonitory> GetDate()
        {
            var data = _monitorRepository.GetStatus();
            return data;
        }
    }
}
