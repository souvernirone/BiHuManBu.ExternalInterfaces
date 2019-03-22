using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Models
{
    public interface IUserinfoRenewalInfoRepository
    {
        int Add(bx_userinfo_renewal_info bxWorkOrder);
        int Update(bx_userinfo_renewal_info bxWorkOrder);
        bx_userinfo_renewal_info FindById(int workOrderId);
        bx_userinfo_renewal_info FindByBuid(long buid);
        bx_userinfo_renewal_info FindByBuidAsync(long buid);
    }
}
