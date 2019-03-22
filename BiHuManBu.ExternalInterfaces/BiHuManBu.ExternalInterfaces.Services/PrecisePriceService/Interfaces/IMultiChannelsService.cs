using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.PrecisePriceService.Interfaces
{
    public interface IMultiChannelsService
    {
        void MultiChannels(string multiChannels, int childAgent, int agent, long buid, int quoteGroup,int cityCode);
    }
}
