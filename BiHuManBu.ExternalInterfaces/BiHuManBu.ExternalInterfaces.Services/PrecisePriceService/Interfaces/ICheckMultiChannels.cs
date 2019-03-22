using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.PrecisePriceService.Interfaces
{
    public interface ICheckMultiChannels
    {
        BaseViewModel CheckMultiChannelsUsed(string multiChannels,int agent,int citycode, long quotegroup, out string newMultiChannels);
    }
}
