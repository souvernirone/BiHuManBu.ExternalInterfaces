using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Models;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Response
{
    public  class GetReportClaimResponse:BaseResponse
    {
        public string LicenseNo { get; set; }
        public List<bx_report_claim> ClaimList { get; set; }
        public List<bx_history_contract> ContractList { get; set; } 
        public int TotalCount { get; set; }
        public int UsedCount { get; set; }
    }
}
