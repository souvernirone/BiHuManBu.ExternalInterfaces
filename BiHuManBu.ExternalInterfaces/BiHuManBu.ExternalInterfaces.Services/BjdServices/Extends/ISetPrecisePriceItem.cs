using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.BjdServices.Extends
{
    public interface ISetPrecisePriceItem
    {
        List<long> FindSource(bx_userinfo userInfo, GetMyBjdDetailRequest request);
    }
}
