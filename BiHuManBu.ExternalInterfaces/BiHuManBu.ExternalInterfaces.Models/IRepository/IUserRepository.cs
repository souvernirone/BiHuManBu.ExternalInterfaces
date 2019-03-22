using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Models
{
    public interface IUserRepository
    {
        int Add(user user);
        user Find(int userId);
        user FindByOpenId(string openId);
        user FindByMobile(string mobile);
        int Delete(int userId);
        int Update(user user);
    }
}
