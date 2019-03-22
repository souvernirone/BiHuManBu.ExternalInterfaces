using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Response
{
    public class GetIntelligentReInfoResponse:BaseResponse
    {
        /// <summary>
        /// 车五项信息
        /// </summary>
        public bx_carinfo CarInfo { get; set; }
        public string CustKey { get; set; }
        /// <summary>
        /// 险种信息
        /// </summary>
        public SaveQuoteViewModel SaveQuote { get; set; }
        /// <summary>
        /// 是否推荐险种 1推荐险种0上年续保险种
        /// </summary>
        public int IsIntelligent { get; set; }
    }
}
