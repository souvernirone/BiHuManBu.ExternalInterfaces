using BiHuManBu.ExternalInterfaces.Models;
using System;
using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Services.BjdServices.Extends
{
    public interface IGetModelsFromQuoteHistory
    {
        Tuple<bx_savequote, bx_quotereq_carinfo, List<bx_quoteresult>, List<bx_submit_info>, string, List<int>> GetModels(long buid, long groupspan);
    }
}
