using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;

namespace BiHuManBu.ExternalInterfaces.Services.BjdServices.Extends
{
    public interface IMapBaoDanXinXiRecord
    {
        bj_baodanxinxi MapBaodanxinxi(CreateOrUpdateBjdInfoRequest request, bx_submit_info submitInfo,
            bx_quoteresult quoteresult, bx_savequote savequote, bx_userinfo userinfo,bx_quotereq_carinfo reqCarInfo, bx_preferential_activity activity);
    }
}
