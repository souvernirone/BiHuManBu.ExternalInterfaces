using BiHuManBu.ExternalInterfaces.Services.Messages.RemoteMessage;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.Interfaces
{
    public interface IGetAccidentListService
    {
        Task<WaBxSysJyxResponse> GetAccidentList(GetAccidentListRequest request, IEnumerable<KeyValuePair<string, string>> pairs);
    }
}
