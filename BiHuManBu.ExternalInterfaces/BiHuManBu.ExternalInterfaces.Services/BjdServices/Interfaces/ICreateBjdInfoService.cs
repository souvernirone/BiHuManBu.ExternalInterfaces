using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;

namespace BiHuManBu.ExternalInterfaces.Services.BjdServices.Interfaces
{
    public interface ICreateBjdInfoService
    {
        long UpdateBjdInfo(CreateOrUpdateBjdInfoRequest request, IEnumerable<KeyValuePair<string, string>> pairs);
    }
}
