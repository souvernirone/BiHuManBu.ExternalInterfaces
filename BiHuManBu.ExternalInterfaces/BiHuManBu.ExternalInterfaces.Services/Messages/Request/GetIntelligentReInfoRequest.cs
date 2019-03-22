using System.ComponentModel.DataAnnotations;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class GetIntelligentReInfoRequest : BaseRequest
    {
        /// <summary>
        /// 车牌号
        /// </summary>
        //[Required]
        [StringLength(30, MinimumLength = 5)]
        public string LicenseNo { get; set; }
        /// <summary>
        /// 投保城市
        /// </summary>
        //[Range(1, 10000)]
        //public int CityCode { get; set; }
        /// <summary>
        /// 客户端标识
        /// </summary>
        [Required]
        [StringLength(32, MinimumLength = 10)]
        public string CustKey { get; set; }
        /// <summary>
        /// 1大车0小车 默认为0
        /// </summary>
        public int RenewalCarType { get; set; }
        
        /// <summary>
        /// 对外接口用户无需传此参数
        /// </summary>
        public int ChildAgent { get; set; }
        /// <summary>
        /// 车架号 目前不需要，仅支持车牌号查询
        /// </summary>
        [RegularExpression(@"^[A-Z_0-9-]{0,50}$")]
        public string CarVin { set; get; }
        /// <summary>
        /// 是否车架号查询 目前不需要默认传0
        /// </summary>
        public int IsCarVin { get; set; }
        /// <summary>
        /// 品牌型号 不需要传入
        /// </summary>
        public string MoldName { get; set;}
        /// <summary>
        /// 注册日期
        /// </summary>
        public string RegisterDate { get; set; }
        /// <summary>
        ///     是否展示排量
        /// </summary>
        public int CanShowExhaustScale { get; set; }
    }
}
