using System.Collections.Generic;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;

namespace BiHuManBu.ExternalInterfaces.Services.Interfaces
{
    public interface IChargeService
    {
        int  Add(CreateChargeRequest request);
        Task<UpdateChargeResponse> Update(UpdateChargeRequest request, IEnumerable<KeyValuePair<string, string>> pairs);

        Task<GetCarClaimResponse> UpdateClaim(UpdateChargeRequest request,
            IEnumerable<KeyValuePair<string, string>> pairs);

        Task<GetReportClaimResponse> GetReportClaim(GetReportClaimRequest request,
            IEnumerable<KeyValuePair<string, string>> pairs);
    }
}
