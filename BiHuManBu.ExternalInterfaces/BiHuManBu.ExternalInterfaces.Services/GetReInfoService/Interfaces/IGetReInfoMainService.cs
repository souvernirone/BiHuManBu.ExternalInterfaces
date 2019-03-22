using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.GetReInfoService.Interfaces
{
    public interface IGetReInfoMainService
    {
        Task<GetReInfoResponse> GetReInfo(GetReInfoRequest request);
    }
}
