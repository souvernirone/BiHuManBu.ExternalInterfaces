﻿using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.Mapper
{
    public static class SmsOrderMapper
    {
        public static SmsOrderModel ConverToViewModel(this bx_sms_order smsOrder)
        {
            SmsOrderModel vm = new SmsOrderModel();
            if (smsOrder != null)
                vm = new SmsOrderModel
                {
                    Id = smsOrder.Id,
                    AgentId = smsOrder.AgentId,
                    RechargeAmount = smsOrder.RechargeAmount,
                    PayType = smsOrder.PayType,
                    OrderStatus = smsOrder.OrderStatus,
                    Deleted =smsOrder.Deleted,
                    CreateTime = smsOrder.CreateTime,
                    SmsQuantity = smsOrder.SmsQuantity,
                    PayDateTime = smsOrder.PayDateTime,
                    OrderNum = smsOrder.OrderNum,
                    ThirdPartOrderNum = smsOrder.ThirdPartOrderNum
                };
            return vm;
        }
    }
}