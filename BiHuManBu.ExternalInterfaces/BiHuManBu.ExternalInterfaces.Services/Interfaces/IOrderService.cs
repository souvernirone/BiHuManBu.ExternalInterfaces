using System.Collections.Generic;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;

namespace BiHuManBu.ExternalInterfaces.Services.Interfaces
{
    public interface IOrderService
    {
        Task<CreateOrderResponse> Create(CreateOrderRequest request, IEnumerable<KeyValuePair<string, string>> pairs);
        Task<UpdateOrderResponse> Update(UpdateOrderRequest request, IEnumerable<KeyValuePair<string, string>> pairs);

        Task<CreateOrderResponse> CreateSelf(CreateOrderRequest request, IEnumerable<KeyValuePair<string, string>> pairs);
        Task<CreateOrderResponse> NewCreateOrder(CreateOrderRequest request, IEnumerable<KeyValuePair<string, string>> pairs);
        Task<CreateOrderResponse> CreateSureOrder(CreateSureOrderRequest request, IEnumerable<KeyValuePair<string, string>> pairs);
        Task<UpdateOrderResponse> UpdateSelf(UpdateOrderRequest request, IEnumerable<KeyValuePair<string, string>> pairs);

        /// <summary>
        /// 预约单详情
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        OrderDetailResponse OrderDetail(OrderDetailRequest request, IEnumerable<KeyValuePair<string, string>> pairs);

        Task<UpdateOrderResponse> ChangeOrderStatus(UpdateOrderRequest request, IEnumerable<KeyValuePair<string, string>> pairs);
        List<CarOrderModel> GetOrders(GetOrdersRequest request,int status, out int TotalCount);
        bx_car_order GetOrder(GetOrderRequest request, IEnumerable<KeyValuePair<string, string>> pairs);

        UpdateOrderResponse UpdateOrder(ModifyOrderRequest request);

        UpdateOrderResponse UpdateImg(long orderId, string openId, string idImgFirs, string idImgSecd);

        CarOrderModel FindCarOrderBy(GetOrderRequest request, IEnumerable<KeyValuePair<string, string>> pairs);
        CarOrderModel GetOrderByBuid(GetOrderByBuidRequest request, IEnumerable<KeyValuePair<string, string>> pairs);

        GetOrderDetailResponse GetOrderDetail(GetOrderRequest request, IEnumerable<KeyValuePair<string, string>> pairs);
    }
}
