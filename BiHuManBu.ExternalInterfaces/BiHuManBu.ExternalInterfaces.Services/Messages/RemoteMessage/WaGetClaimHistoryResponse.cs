using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.RemoteMessage
{
    public class WaGetClaimHistoryResponse : BaseResponse
    {
        public List<bx_car_claims> ClaimList { get; set; }
    }
}
