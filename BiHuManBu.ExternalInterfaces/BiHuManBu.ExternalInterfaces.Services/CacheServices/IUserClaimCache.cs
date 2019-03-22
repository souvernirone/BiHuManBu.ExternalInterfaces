using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;

namespace BiHuManBu.ExternalInterfaces.Services.CacheServices
{
    public interface IUserClaimCache
    {
        Task<GetEscapedInfoResponse> GetList(GetEscapedInfoRequest request);
        Task<GetViolationInfoResponse> GetList(GetViolationInfoRequest request);
    }
}
