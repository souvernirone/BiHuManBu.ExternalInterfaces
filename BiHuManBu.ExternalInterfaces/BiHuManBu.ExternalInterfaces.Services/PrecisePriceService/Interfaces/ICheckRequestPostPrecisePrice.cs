using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.PrecisePriceService.Interfaces
{
    public interface ICheckRequestPostPrecisePrice
    {
        BaseViewModel CheckRequest(PostPrecisePriceRequest request);
    }
}
