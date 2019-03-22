using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Services.PrecisePriceService.Interfaces
{
    public interface IGetMultiChannels
    {
        string GetStrMultiChannels(int agent, long buid, List<int> quoteSourceGroup, int cityCode);
    }
}
