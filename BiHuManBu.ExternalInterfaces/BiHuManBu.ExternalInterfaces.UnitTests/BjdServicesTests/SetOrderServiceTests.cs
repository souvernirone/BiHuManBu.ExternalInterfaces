using System.Collections.Generic;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services;
using BiHuManBu.ExternalInterfaces.Services.AgentChannelService.Implementations;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using NSubstitute;
using NUnit.Framework;
using BiHuManBu.ExternalInterfaces.Services.ValidateService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using System.Net;
using BiHuManBu.ExternalInterfaces.Services.BjdServices.Achieves;
using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using System;
using BiHuManBu.ExternalInterfaces.Models.ReportModel;

namespace BiHuManBu.ExternalInterfaces.UnitTests.BjdServicesTests
{
    [TestFixture]
    public class SetOrderServiceTests
    {
        private static IOrderRepository _orderRepository = Substitute.For<IOrderRepository>();
        private static ICarInfoRepository _carInfoRepository = Substitute.For<ICarInfoRepository>();
        SetOrderService _setOrderService = new SetOrderService(_orderRepository, _carInfoRepository);

        [Test]
        public void SetOrder_OrderStatusNull_CarinfoNull()
        {
            _carInfoRepository.Find(Arg.Any<string>()).Returns(x => null);
            _orderRepository.GetOrderStatus(Arg.Any<long>()).Returns(x => null);
            var result = _setOrderService.SetOrder(new MyBaoJiaViewModel(), new bx_userinfo() { Id = 1 });
            Assert.AreEqual(0, result.HasOrder);
            Assert.AreEqual(0, result.OrderId);
            Assert.AreEqual(0, result.OrderStatus);
            Assert.AreEqual("", result.LicenseNo);
            Assert.AreEqual("0", result.PurchasePrice);
        }
        [Test]
        public void SetOrder_OrderStatusNotNull_CarinfoNull()
        {
            var _setOrderService = new SetOrderService(_orderRepository, _carInfoRepository);
            _carInfoRepository.Find(Arg.Any<string>()).Returns(x => null);
            _orderRepository.GetOrderStatus(Arg.Any<long>()).Returns(x => new CarOrderStatusModel() { Id = 2, OrderStatus = 3 });
            var result = _setOrderService.SetOrder(new MyBaoJiaViewModel(), new bx_userinfo() { Id = 1 });
            Assert.AreEqual(1, result.HasOrder);
            Assert.AreEqual(2, result.OrderId);
            Assert.AreEqual(3, result.OrderStatus);
            Assert.AreEqual("", result.LicenseNo);
            Assert.AreEqual("0", result.PurchasePrice);
        }
        [Test]
        public void SetOrder_OrderStatusNull_CarinfoNotNull()
        {
            var _setOrderService = new SetOrderService(_orderRepository, _carInfoRepository);
            _carInfoRepository.Find(Arg.Any<string>()).Returns(x => new bx_carinfo() { purchase_price = 0.1m });
            _orderRepository.GetOrderStatus(Arg.Any<long>()).Returns(x => null);
            var result = _setOrderService.SetOrder(new MyBaoJiaViewModel(), new bx_userinfo() { Id = 1, LicenseNo = "京A12345" });
            Assert.AreEqual(0, result.HasOrder);
            Assert.AreEqual(0, result.OrderId);
            Assert.AreEqual(0, result.OrderStatus);
            Assert.AreEqual("京A12345", result.LicenseNo);
            Assert.AreEqual("0.1", result.PurchasePrice);
        }
        [Test]
        public void SetOrder_OrderStatusNotNull_CarinfoNotNull()
        {
            ICarInfoRepository _carInfoRepository = Substitute.For<ICarInfoRepository>();
            var _setOrderService = new SetOrderService(_orderRepository, _carInfoRepository);
            _carInfoRepository.Find(Arg.Any<string>()).Returns(x => new bx_carinfo() { purchase_price = 0.1m });
            _orderRepository.GetOrderStatus(Arg.Any<long>()).Returns(x => new CarOrderStatusModel() { Id = 2, OrderStatus = 3 });
            var result = _setOrderService.SetOrder(new MyBaoJiaViewModel(), new bx_userinfo() { Id = 1, LicenseNo = "京A12345" });
            Assert.AreEqual(1, result.HasOrder);
            Assert.AreEqual(2, result.OrderId);
            Assert.AreEqual(3, result.OrderStatus);
            Assert.AreEqual("京A12345", result.LicenseNo);
            Assert.AreEqual("0.1", result.PurchasePrice);
        }
    }
}
