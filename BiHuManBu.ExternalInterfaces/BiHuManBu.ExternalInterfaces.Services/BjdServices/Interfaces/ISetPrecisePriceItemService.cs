using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.ReportModel;

namespace BiHuManBu.ExternalInterfaces.Services.BjdServices.Interfaces
{
    public interface ISetPrecisePriceItemService
    {
        MyBaoJiaViewModel SetPrecisePriceItem(MyBaoJiaViewModel my, bx_userinfo userinfo, GetMyBjdDetailRequest request, List<bx_quoteresult_carinfo> quoteresultCarinfo, int reqseatcount);
        MyPrecisePriceItemViewModel ConvertToViewModelNew(int source, bx_savequote savequote, bx_quoteresult quoteresult, bx_submit_info submitInfo, int quoteStatus, List<AgentConfigNameModel> agentChannelList, string carVin,List<bx_ywxdetail> jiayi, string strRate = null);
    }
}
