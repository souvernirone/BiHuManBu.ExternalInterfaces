using System.ComponentModel.DataAnnotations;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class GetOrderRequest:BaseRequest
    {
        [Range(1, 2100000000)]
        public long OrderId { get; set; }
        public string OpenId { get; set; }
    }
}
