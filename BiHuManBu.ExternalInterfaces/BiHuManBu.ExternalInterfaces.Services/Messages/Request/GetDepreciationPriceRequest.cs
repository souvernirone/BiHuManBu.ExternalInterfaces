using System.ComponentModel.DataAnnotations;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class GetDepreciationPriceRequest
    {
        [RegularExpression(@"^\d{10,10}$", ErrorMessage = "起保日期应该是时间戳格式,单位是秒")]
        public string Bizstartdate { get; set; }

        [RegularExpression(@"^\d{4}-\d{2}-\d{2}$")]
        public string Registerdate { get; set; }

        /// <summary>
        /// 车辆类型 0小车 1货车 兼容内部的
        /// </summary>
        public int CarType { get; set; }
        /// <summary>
        /// 车辆类型 0小车 1货车
        /// </summary>
        //public int RenewalCarType { get; set; }

        [Range(1, 10000000)]
        public decimal PurchasePrice { get; set; }
    }
}