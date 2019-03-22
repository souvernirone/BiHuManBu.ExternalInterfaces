namespace BiHuManBu.ExternalInterfaces.Services.ViewModels
{
    public class GetIntelligentReInfoViewModel:BaseViewModel
    {
        /// <summary>
        /// 车五项信息
        /// </summary>
        public LicenseViewModel VehicleInfo { get; set; }
        /// <summary>
        /// 险种信息
        /// </summary>
        public SaveQuoteViewModel SaveQuote { get; set; }
        /// <summary>
        /// 是否推荐险种 1推荐险种0上年续保险种
        /// </summary>
        //public int IsIntelligent { get; set; }
    }
}
