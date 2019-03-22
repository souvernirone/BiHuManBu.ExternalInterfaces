using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.GetReInfoService.Interfaces
{
    public interface IToCenterFromReInfoService
    {
        Task<GetReInfoResponse> PushCenterService(GetReInfoRequest request, long buid, string reqCacheKey);
    }
}
