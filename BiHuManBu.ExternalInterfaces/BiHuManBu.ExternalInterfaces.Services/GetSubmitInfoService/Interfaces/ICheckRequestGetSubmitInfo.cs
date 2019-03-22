using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Services.GetSubmitInfoService.Interfaces
{
    public interface ICheckRequestGetSubmitInfo
    {
        BaseResponse CheckRequest(GetSubmitInfoRequest request, IEnumerable<KeyValuePair<string, string>> pairs);
    }
}
