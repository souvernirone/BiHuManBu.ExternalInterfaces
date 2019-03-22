using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Models
{
    public interface ICarOrderUserInfoRepository
    {
        bx_order_userinfo Find(int userId);
        bx_order_userinfo FindByBuid(long buid);
        bx_order_userinfo FindByOpenIdAndLicense(string openid, string licenseno);
        bx_order_userinfo FindByOpenIdAndLicense(string openid, string licenseno, string agent);
        long Add(bx_order_userinfo userinfo);
        int Update(bx_order_userinfo userinfo);

        List<bx_order_userinfo> FindByAgentAndLicense(bool isAgent, string strPass, string LicenseNo, int PageSize, int CurPage);
    }
}
