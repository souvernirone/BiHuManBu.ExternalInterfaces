using System.Linq;
using BiHuManBu.ExternalInterfaces.Models;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class GscRepostory:IGscRepository
    {
        public bx_gsc_userinfo FindUserInfoByGsc(string usercode, string pwd)
        {
            return DataContextFactory.GetDataContext().bx_gsc_userinfo.FirstOrDefault(x => x.usercode == usercode && x.pwd == pwd);
        }
    }
}
