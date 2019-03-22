

using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;

namespace BiHuManBu.ExternalInterfaces.Services.Interfaces
{
    public interface IEnumService
    {
        GetParaBhTypeResponse GetParaBhType(GetParaBhTypeRequest request,IEnumerable<KeyValuePair<string, string>> pairs);
    }
}
