using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.UserinfoFactoryService.Interfaces
{
    public interface IQuoteAddService
    {
        long QuoteAdd(PostPrecisePriceRequest request, string _url, IUserInfoRepository repository, int topAgentId = 0);
    }
}
