using System.ComponentModel.DataAnnotations;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class UpdateOrderRequest
    {
        //[Range(1,2100000000)]
        public long OrderId { get; set; }
        [Range(-1000,1000)]
        public int OrderStatus { get; set; }
        [Range(-1000, 1000)]
        public int PayStatus { get; set; }
        //[Range(1, 1000000)]
        public int AgentId { get; set; }
        //[Required]
        //[StringLength(32, MinimumLength = 32)]
        public string SecCode { get; set; }
        //[Required]
        public string OpenId { get; set; }
    }
}
