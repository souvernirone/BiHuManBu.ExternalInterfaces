
using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Services.ViewModels
{
    public class QuoteResultCarInfoViewModel
    {
        public int Source { get; set; }
        public string EngineNo { get; set; }
        public string CarVin { get; set; }
        public string MoldName { get; set; }
        public string RegisterDate { get; set; }
        public string CredentislasNum { get; set; }
        public string LicenseOwner { get; set; }
        public int IdType { get; set; }
        public int LicenseType { get; set; }
        public int CarType { get; set; }
        public int CarUsedType { get; set; }
        public int SeatCount { get; set; }
        public decimal ExhaustScale { get; set; }
        public decimal CarEquQuality { get; set; }
        public decimal TonCount { get; set; }
        public decimal PurchasePrice { get; set; }
        public int FuelType { get; set; }
        public int ProofType { get; set; }
        public int LicenseColor { get; set; }
        public int ClauseType { get; set; }
        public int RunRegion { get; set; }
        public string Risk { get; set; }
        public string XinZhuanXu { get; set; }
        public string SyVehicleClaimType { get; set; }
        public string JqVehicleClaimType { get; set; }
        public string VehicleStyle { get; set; }
        /// <summary>
        /// 车型别名
        /// </summary>
        public string VehicleAlias { get; set; }
        /// <summary>
        /// 年款
        /// </summary>
        public string VehicleYear { get; set; }
        /// <summary>
        /// 是否贷款车  1贷款  0 不贷款  -1不知道是否贷款
        /// </summary>
        public int? IsLoans { get; set; }

        /// <summary>
        /// 过户时间  具体时间: 过户   "": 未过户    "-1": 不知道是否过户
        /// </summary>
        public string TransferDate { get; set; }
    }

    public class QuoteReqCarInfoViewModel
    {
        public string AutoMoldCode { get; set; }
        public int AutoMoldCodeSource { get; set; }
        public int IsNewCar { get; set; }
        public decimal NegotiatePrice { get; set; }
        public int IsPublic { get; set; }
        public int CarUsedType { get; set; }
        public string DriveLicenseTypeName { get; set; }
        public string DriveLicenseTypeValue { get; set; }
        /// <summary>
        /// 是否修改座位数1是0否
        /// </summary>
        public string SeatUpdated { get; set; }
        /// <summary>
        /// 上次修改的太平洋实际折扣系数
        /// </summary>
        public string RequestActualDiscounts { get; set; }
        /// <summary>
        /// 上次修改的人太平折扣系数相关请求参数
        /// </summary>
        public List<DiscountViewModel> RequestDiscount {get;set;}

        /// <summary>
        /// 平安底价报价 1是0否
        /// </summary>
        public string RequestIsPaFloorPrice { get; set; }

        /// <summary>
        /// 驾照号，为驾意险准备的
        /// </summary>
        public string DriverCard { get; set; }
        /// <summary>
        /// 驾照类型，为驾意险准备的
        /// </summary>
        public string DriverCardType { get; set; }
    }
}
