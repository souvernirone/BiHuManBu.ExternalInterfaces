using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Repository;

namespace BiHuManBu.ExternalInterfaces.Services.Interfaces
{
    public interface IMySqlMonitorService
    {
        IEnumerable<MySqlMonitory> GetDate();
    }
}
