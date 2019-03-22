
using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.Interfaces
{
    public interface ISmsService
    {
        SmsResultViewModel SendSms(SmsRequest request, IEnumerable<KeyValuePair<string, string>> pairs);
        GetSmsAccountResponse GetSmsAccount(SmsAccountRequest request, IEnumerable<KeyValuePair<string, string>> pairs);

        GetSmsOrderStatusResponse SmsOrderStatus(SmsOrderStatusRequest request, IEnumerable<KeyValuePair<string, string>> enumerable);

        GetSmsOrderResponse SmsCreateOrder(SmsCreateOrderRequest request, IEnumerable<KeyValuePair<string, string>> enumerable);

        GetSmsAccountResponse CreateAccount(CreateAccountRequest request,
            IEnumerable<KeyValuePair<string, string>> pairs);
        GetSmsOrderDetailResponse GetSmsOrderDetail(GetSmsOrderDetailRequest request,
            IEnumerable<KeyValuePair<string, string>> pairs);
    }
}
