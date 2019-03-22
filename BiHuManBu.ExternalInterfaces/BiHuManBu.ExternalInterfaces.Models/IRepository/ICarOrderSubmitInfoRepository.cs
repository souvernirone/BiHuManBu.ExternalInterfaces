using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Models
{
    public interface ICarOrderSubmitInfoRepository
    {
        long Add(bx_order_submit_info submitInfo);
        bx_order_submit_info GetSubmitInfo(long buid, int source);
    }
}
