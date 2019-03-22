
using System;

namespace BiHuManBu.ExternalInterfaces.Services.ViewModels
{
    public class ProtoUserInfoViewModel
    {
        public long Id { get; set; }
        public Nullable<int> UserId { get; set; }
        public string UserName { get; set; }
        public string LicenseNo { get; set; }
        public string Mobile { get; set; }
        public string OpenId { get; set; }
        public string CityCode { get; set; }
        public string RenewalIdNo { get; set; }
        public string EngineNo { get; set; }
        public string CarVIN { get; set; }
        public Nullable<int> Source { get; set; }
        public Nullable<int> LastYearSource { get; set; }
        public string MoldName { get; set; }
        public string RegisterDate { get; set; }
        public string ApproxDate { get; set; }
        public string Address { get; set; }
        public string VehicleId { get; set; }
        public string StandardName { get; set; }
        public Nullable<int> NeedEngineNo { get; set; }
        public Nullable<DateTime> CreateTime { get; set; }
        public Nullable<DateTime> UpdateTime { get; set; }
        public Nullable<int> ProcessStep { get; set; }
        public Nullable<int> QuoteStatus { get; set; }
        public Nullable<int> OrderStatus { get; set; }
        public Nullable<int> IsOrder { get; set; }
        public Nullable<int> IsReView { get; set; }
        public Nullable<int> JiSuanType { get; set; }
        public Nullable<int> IsService { get; set; }
        public Nullable<DateTime> ServiceTime { get; set; }
        public string Agent { get; set; }
        public string IdCard { get; set; }
        public Nullable<int> IsLastYear { get; set; }
        public string LicenseOwner { get; set; }
        public Nullable<int> IsTest { get; set; }
        public string InsuredName { get; set; }
        public string InsuredMobile { get; set; }
        public string InsuredIdCard { get; set; }
        public Nullable<int> IsInputBxData { get; set; }
        public Nullable<int> IsRiskVehicle { get; set; }
        public Nullable<int> IsPeopleQuote { get; set; }
        public Nullable<int> HongBaoId { get; set; }
        public Nullable<decimal> HongBaoAmount { get; set; }
        public Nullable<int> Datasource { get; set; }
        public string ApproxPeopleName { get; set; }
        public Nullable<int> ApproxPeopleId { get; set; }
        public Nullable<DateTime> ApproxCreateDate { get; set; }
        public Nullable<int> IsInstalment { get; set; }
        public Nullable<int> IsClosing { get; set; }
        public Nullable<int> IsSingleSubmit { get; set; }
        public Nullable<int> RenewalType { get; set; }
        public Nullable<int> RenewalStatus { get; set; }
        public Nullable<int> InsuredIdType { get; set; }
        public int IsDistributed { get; set; }
    }
}
