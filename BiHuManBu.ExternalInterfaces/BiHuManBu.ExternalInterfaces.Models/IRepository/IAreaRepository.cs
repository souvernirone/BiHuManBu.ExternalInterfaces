using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Models
{
    public interface IAreaRepository
    {
        List<bx_area> Find();
        List<bx_area> FindByPid(int pid);
    }
}
