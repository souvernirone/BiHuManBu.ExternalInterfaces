
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.Interfaces
{
    public interface IWorkOrderService
    {
        AddOrUpdateWorkOrderResponse AddOrUpdateWorkOrder(AddOrUpdateWorkOrderRequest request,
            IEnumerable<KeyValuePair<string, string>> pairs);

        AddOrUpdateWorkOrderDetailResponse AddOrUpdateWorkOrderDetail(AddOrUpdateWorkOrderDetailRequest request,
            IEnumerable<KeyValuePair<string, string>> pairs);
        WorkOrderDetailListResponse WorkOrderDetailList(WorkOrderDetailListRequest request,
            IEnumerable<KeyValuePair<string, string>> pairs);

        Task<GetReInfoNewViewModel> GetReInfo(GetReInfoRequest request,
            IEnumerable<KeyValuePair<string, string>> pairs, Uri uri);

        ChangeReInfoAgentResponse ChangeReInfoAgent(ChangeReInfoAgentRequest request,
            IEnumerable<KeyValuePair<string, string>> pairs);

        BaseResponse AdditionalReInfo(AdditionalReInfoRequest request,
            IEnumerable<KeyValuePair<string, string>> pairs);

        ReVisitedListViewModel WorkOrderList(long buid);

        /// <summary>
        /// 添加回访记录
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        BaseViewModel AddReVisited(AddReVisitedRequest request);
    }
}
