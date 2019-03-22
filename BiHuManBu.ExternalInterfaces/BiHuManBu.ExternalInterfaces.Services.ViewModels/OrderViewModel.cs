using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Models;

namespace BiHuManBu.ExternalInterfaces.Services.ViewModels
{
    public class OrderViewModel : BaseViewModel
    {
        public CarOrderModel CarOrder { get; set; }
    }

    public class OrdersViewModel : BaseViewModel
    {
        public int TotalCount { get; set; }
        public List<CarOrderModel> CarOrders { get; set; }
    }

}
