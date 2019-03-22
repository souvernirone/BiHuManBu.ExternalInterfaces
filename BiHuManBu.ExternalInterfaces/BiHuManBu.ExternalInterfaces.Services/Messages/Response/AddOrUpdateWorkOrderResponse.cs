
namespace BiHuManBu.ExternalInterfaces.Services.Messages.Response
{
    public class AddOrUpdateWorkOrderResponse:BaseResponse
    {
        public int WorkOrderId { get; set; }
        public int AdvAgentId { get; set; } 
    }

    public class AddOrUpdateWorkOrderDetailResponse : BaseResponse
    {
        public int WorkOrderDetailId { get; set; }
    }
}
