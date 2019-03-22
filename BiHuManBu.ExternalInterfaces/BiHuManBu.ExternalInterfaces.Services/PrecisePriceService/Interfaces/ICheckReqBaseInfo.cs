using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.PrecisePriceService.Interfaces
{
    public interface ICheckReqBaseInfo
    {
        BaseViewModel CheckBaseInfo(PostPrecisePriceRequest request);
    }
}
