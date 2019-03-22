using BiHuManBu.ExternalInterfaces.Models.PartialModels;
using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Models.IRepository
{
    public interface IQuoteHistoryRepository
    {
        List<bx_quote_history> GetByBuid(long buid, long groupspan);
    }
}
