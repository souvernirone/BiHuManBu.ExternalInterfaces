using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.BjdServices.Interfaces
{
    public interface IGetBjdInfoService
    {
        BaojiaItemViewModel GetBjdInfo(GetBjdItemRequest request, IEnumerable<KeyValuePair<string, string>> pairs);
    }
}
