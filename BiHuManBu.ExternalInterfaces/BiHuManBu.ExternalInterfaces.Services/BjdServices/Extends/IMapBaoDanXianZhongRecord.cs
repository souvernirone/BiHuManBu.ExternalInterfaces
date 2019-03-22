using BiHuManBu.ExternalInterfaces.Models;
using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Services.BjdServices.Extends
{
    public interface IMapBaoDanXianZhongRecord
    {
        bj_baodanxianzhong MapBaodanxianzhong(bj_baodanxinxi baodanxinxi, bx_quoteresult quoteresult, bx_savequote savequote, bx_submit_info submitInfo, List<bx_ywxdetail> jylist);
    }
}
