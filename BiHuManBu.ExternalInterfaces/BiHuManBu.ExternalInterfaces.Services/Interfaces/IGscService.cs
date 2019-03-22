using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Models;

namespace BiHuManBu.ExternalInterfaces.Services.Interfaces
{
    public interface IGscService
    {
        bx_gsc_userinfo FindUserInfoByGsc(string usercode, string pwd);
    }
}
