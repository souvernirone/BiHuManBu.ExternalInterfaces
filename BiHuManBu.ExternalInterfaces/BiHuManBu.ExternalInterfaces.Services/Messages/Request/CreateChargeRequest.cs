using System.ComponentModel.DataAnnotations;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class CreateChargeRequest
    {
        private int _totalCount = 0;

        [Range(1, 1000000)]
        public int Agent { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        public string LicenseNo { get; set; }
        [Required]
        [StringLength(32, MinimumLength = 32)]
        public string SecCode { get; set; }
        
        public double TotalPrice { get; set; }

        public int TotalCount {
            get { return  _totalCount = int.Parse((TotalPrice/UnitPirce).ToString()); }
            set { _totalCount = value; }
        }

        /// <summary>
        /// 0:获取行驶证信息
        /// </summary>
        public int ChargeType { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public double UnitPirce { get; set; }


    }
}
