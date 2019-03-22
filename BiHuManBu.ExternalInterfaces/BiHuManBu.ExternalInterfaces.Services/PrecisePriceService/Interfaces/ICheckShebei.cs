using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;

namespace BiHuManBu.ExternalInterfaces.Services.PrecisePriceService.Interfaces
{
    public interface ICheckShebei
    {
        string CheckRequestShebei(PostPrecisePriceRequest request);
    }
}
