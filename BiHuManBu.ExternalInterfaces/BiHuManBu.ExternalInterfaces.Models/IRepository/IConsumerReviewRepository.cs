using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Models
{
    public interface IConsumerReviewRepository
    {
        bx_consumer_review Find(int id);
        int AddDetail(bx_consumer_review bxWorkOrderDetail);
        int UpdateDetail(bx_consumer_review bxWorkOrderDetail);
        List<bx_consumer_review> FindDetails(long buid);
        bx_consumer_review FindNewClosedOrder(long buid, int status = 1);
        List<bx_consumer_review> FindNoReadList(int agentId, out int total);
    }
}
