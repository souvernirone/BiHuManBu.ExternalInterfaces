
using System.ComponentModel.DataAnnotations;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class GetDiscountPriceRequest
    {
        /// <summary>
        /// 新车购置价格
        /// </summary>
        public decimal PurchasePrice { get; set; }
        /// <summary>
        /// 商业险起保时间
        /// </summary>
        [RegularExpression(@"^\d{4}-\d{2}-\d{2}$")]
        public string BizStartDate { get; set; }
        /// <summary>
        /// 初登日期
        /// </summary>
        [RegularExpression(@"^\d{4}-\d{2}-\d{2}$")]
        public string RegisterDate { get; set; }
    }
}
