namespace BiHuManBu.ExternalInterfaces.Models.ReportModel
{
    public class InsuranceEndDateAndClaim
    {
        public string LastBusinessEndDdate { get; set; }
        public string LastForceEndDdate { get; set; }
        public int ClaimCount { get; set; }
        public int ForceClaimCount { get; set; }
        public int BizClaimCount { get; set; }

    }
}
