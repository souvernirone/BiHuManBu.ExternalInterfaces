using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Response
{
    public class OrderDetailResponse:BaseResponse
    {
        public CarOrder CarOrder { get; set; }
        public UserInfo UserInfo { get; set; }
        public List<ClaimDetail> ClaimDetail { get; set; }
        public PrecisePrice PrecisePrice { get; set; }
    }

    public class GetOrderDetailResponse : BaseResponse
    {
        public bx_car_order CarOrder { get; set; }
    }

}
