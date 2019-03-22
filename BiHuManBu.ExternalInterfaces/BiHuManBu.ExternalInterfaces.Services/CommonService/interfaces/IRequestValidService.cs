using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Services.CommonService.interfaces
{
    public interface IRequestValidService
    {
        bool ValidateReqest(IEnumerable<KeyValuePair<string, string>> list, string configKey, string checkCode);
    }
}