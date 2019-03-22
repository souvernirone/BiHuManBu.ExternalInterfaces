using System.Collections.Generic;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.Mapper
{
    public  static class ReportClaimMapper
    {
        public static ReportClaim ConvertToReportClaimViewMode(this bx_report_claim claim)
        {
            var reportClaim = new ReportClaim();
            reportClaim.AccidentPlace = claim.AccidentPlace;
            reportClaim.AccidentPsss = claim.AccidentPsss;
            reportClaim.DriverName = claim.DriverName;
            reportClaim.IsCommerce = claim.IsCommerce.HasValue?claim.IsCommerce.Value:0;
            reportClaim.IsOwners = claim.IsOwners.HasValue ? claim.IsOwners.Value : 0;
            reportClaim.IsThreecCar = claim.IsThreecCar.HasValue ? claim.IsThreecCar.Value : 0;
            reportClaim.ReportDate = claim.ReportDate.HasValue ? claim.ReportDate.Value.ToString("yyyy-MM-dd") : string.Empty;
            return reportClaim;
        }

        public static List<ReportClaim> ConvertToReportClaimsViewModel(this List<bx_report_claim> claims)
        {
            var list = new List<ReportClaim>();
            if (claims.Count > 0)
            {
                list.AddRange(claims.Select(ConvertToReportClaimViewMode));
            }
            return list;
        } 
    }
}
