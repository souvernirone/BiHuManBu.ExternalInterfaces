
using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Models
{
    public interface IReportClaimRepository
    {
        int Add(List<bx_report_claim> claims);
        List<bx_report_claim> FindList(string licenseno);

        int Remove(string licenseno);

    }
}
