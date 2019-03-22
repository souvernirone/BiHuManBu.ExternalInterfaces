using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;

namespace BiHuManBu.ExternalInterfaces.Services.SubmitInfoService.Interfaces
{
    public interface IPostSubmitInfoService
    {
        PostSubmitInfoResponse PostSubmitInfo(PostSubmitInfoRequest request, IEnumerable<KeyValuePair<string, string>> pairs);
    }
}
