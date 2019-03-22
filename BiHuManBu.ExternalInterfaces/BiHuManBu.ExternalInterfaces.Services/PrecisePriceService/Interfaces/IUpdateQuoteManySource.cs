using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;

namespace BiHuManBu.ExternalInterfaces.Services.PrecisePriceService.Interfaces
{
    public interface IUpdateQuoteManySource
    {
        void UpdateQuoteManySourceMethod(int childAgent, int topAgent, string multiChannels, List<MultiChannels> models);
    }
}
