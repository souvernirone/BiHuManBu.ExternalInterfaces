﻿using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.ReportModel;
using BiHuManBu.ExternalInterfaces.Services.BjdServices.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using log4net;

namespace BiHuManBu.ExternalInterfaces.Services.BjdServices.Achieves
{
    public class SetOrderService : ISetOrderService
    {
        private IOrderRepository _orderRepository;
        private ICarInfoRepository _carInfoRepository;
        private ILog logErr;
        public SetOrderService(IOrderRepository orderRepository, ICarInfoRepository carInfoRepository)
        {
            _carInfoRepository = carInfoRepository;
            _orderRepository = orderRepository;
            logErr = LogManager.GetLogger("ERROR");
        }
        public MyBaoJiaViewModel SetOrder(MyBaoJiaViewModel my, bx_userinfo userInfo)
        {
            CarOrderStatusModel bxCarOrder = _orderRepository.GetOrderStatus(userInfo.Id);
            if (bxCarOrder != null)
            {
                my.HasOrder = 1;
                my.OrderId = bxCarOrder.Id;
                my.OrderStatus = bxCarOrder.OrderStatus.HasValue ? bxCarOrder.OrderStatus.Value : 0;
            }
            else
            {
                my.HasOrder = 0;
                my.OrderId = 0;
                my.OrderStatus = 0;
            }
            if (!string.IsNullOrEmpty(userInfo.LicenseNo))
            {
                my.LicenseNo = userInfo.LicenseNo;
                var carInfo = _carInfoRepository.Find(userInfo.LicenseNo);
                my.PurchasePrice = carInfo != null
                    ? (carInfo.purchase_price.HasValue ? carInfo.purchase_price.Value.ToString() : "0")
                    : "0";
            }
            else
            {
                my.LicenseNo = "";
                my.PurchasePrice = "0";
            }
            return my;
        }
    }
}
