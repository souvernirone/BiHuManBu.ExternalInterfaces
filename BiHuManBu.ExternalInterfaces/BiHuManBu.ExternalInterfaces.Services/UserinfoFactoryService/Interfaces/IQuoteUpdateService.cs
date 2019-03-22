using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;

namespace BiHuManBu.ExternalInterfaces.Services.UserinfoFactoryService.Interfaces
{
    public interface IQuoteUpdateService
    {
        long QuoteUpdate(PostPrecisePriceRequest request, bx_userinfo userinfo, string _url, IUserInfoRepository repository);
    }
}
