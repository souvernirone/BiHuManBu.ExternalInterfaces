using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Models;

namespace BiHuManBu.ExternalInterfaces.Services.Interfaces
{
    public interface IAreaService
    {
        List<bx_area> Find();
        List<bx_area> FindByPid(int pid);
    }
}
