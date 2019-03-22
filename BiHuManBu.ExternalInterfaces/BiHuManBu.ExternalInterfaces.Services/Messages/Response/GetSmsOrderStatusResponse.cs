using System.ComponentModel.DataAnnotations;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Response
{
    public class GetSmsOrderStatusResponse : BaseResponse
    {
        public int Count { get; set; }
    }
}
