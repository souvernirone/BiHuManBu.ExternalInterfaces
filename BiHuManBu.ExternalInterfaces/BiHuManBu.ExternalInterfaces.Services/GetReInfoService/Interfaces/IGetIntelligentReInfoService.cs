using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.GetReInfoService.Interfaces
{
    public interface IGetIntelligentReInfoService
    {
        Task<GetIntelligentReInfoResponse> GetIntelligentReInfo(GetIntelligentReInfoRequest request, IEnumerable<KeyValuePair<string, string>> pairs);
    }
}
