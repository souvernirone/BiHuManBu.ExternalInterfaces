using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Services.BjdServices.Interfaces
{
    public interface ISetQuoteReqNewService
    {
        MyBaoJiaViewModel SetQuoteReq(MyBaoJiaViewModel my, List<long> listquote1, bx_quotereq_carinfo quotereqcarinfo, ref string postBizStartDate, ref string postForceStartDate);
    }
}
