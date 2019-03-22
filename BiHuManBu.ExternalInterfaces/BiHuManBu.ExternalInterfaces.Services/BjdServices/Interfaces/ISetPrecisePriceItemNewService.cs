using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Services.BjdServices.Interfaces
{
    public interface ISetPrecisePriceItemNewService
    {
        MyBaoJiaViewModel SetPrecisePriceItem(MyBaoJiaViewModel my, bx_userinfo userinfo,
            List<bx_quoteresult_carinfo> quoteresultCarinfo,
            List<long> listquote01, bool allfail,
            bx_savequote sq, List<bx_quoteresult> qrList, List<bx_submit_info> siList);
    }
}
