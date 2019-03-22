using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;

namespace BiHuManBu.ExternalInterfaces.Services.Interfaces
{
    public interface IAppVerifyService
    {
        AppBaseResponse Verify(AppBaseRequest request, IEnumerable<KeyValuePair<string, string>> pairs);
    }
}
