using System.ComponentModel.DataAnnotations;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class GetRecordNewCarRequest:BaseRequest
    {
        /// <summary>
        /// 车架号
        /// </summary>
        [Required]
        [StringLength(20, MinimumLength = 5)]
        public string CarVin { get; set; }
        /// <summary>
        /// 发动机号
        /// </summary>
        public string EngineNo { get; set; }
    }
}
