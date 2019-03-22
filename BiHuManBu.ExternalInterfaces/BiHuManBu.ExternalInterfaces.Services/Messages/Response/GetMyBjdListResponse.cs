using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Response
{
    public class GetMyBjdListResponse
    {
        public int TotalCount { get; set; }

        public int CarUsedType { get; set; }
        public string LicenseOwner { get; set; }
        public string InsuredName { get; set; }
        public string PostedName { get; set; }
        public int IdType { get; set; }
        public string CredentislasNum { get; set; }
        public string CityCode { get; set; }
        public string EngineNo { get; set; }
        public string CarVin { get; set; }
        public string PurchasePrice { get; set; }
        public int SeatCount { get; set; }


        public string UserName { get; set; }
        public string LicenseNo { get; set; }
        public string MoldName { get; set; }
        public string RegisterDate { get; set; }
        public string last_business_end_date { get; set; }
        public string last_end_date { get; set; }
        public long b_uid { get; set; }
        public List<ViewModels.MyPrecisePriceItemViewModel> precisePriceItem { get; set; }
    }
}
