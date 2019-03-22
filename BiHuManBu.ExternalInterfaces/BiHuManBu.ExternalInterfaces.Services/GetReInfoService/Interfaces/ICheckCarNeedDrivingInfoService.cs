using BiHuManBu.ExternalInterfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.GetReInfoService.Interfaces
{
    public interface ICheckCarNeedDrivingInfoService
    {
        Task<bool> GetInfo(bx_userinfo userinfo);
    }
}
