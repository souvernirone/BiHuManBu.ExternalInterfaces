using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Models;

namespace BiHuManBu.ExternalInterfaces.Services.Interfaces
{
    public interface IUserService
    {
        bx_userinfo FindByOpenIdAndLicense(string openId, string licenseno);
        long Add(bx_userinfo userinfo);
        int Update(bx_userinfo userinfo);

        user FindUserByOpenId(string openid);

        user AddUser(string openid, string mobile);
    }
}
