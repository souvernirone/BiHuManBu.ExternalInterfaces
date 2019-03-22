using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;

namespace BiHuManBu.ExternalInterfaces.Services.Interfaces
{
    public interface IMessageService
    {
        AddMessageResponse AddMessage(AddMessageRequest request, IEnumerable<KeyValuePair<string, string>> pairs);

        ReadMessageResponse ReadMessage(ReadMessageRequest request, IEnumerable<KeyValuePair<string, string>> pairs);

        MessageListResponse MessageList(MessageListRequest request,IEnumerable<KeyValuePair<string, string>> pairs);
        MessageListResponse MessageListNew(MessageListRequest request, IEnumerable<KeyValuePair<string, string>> pairs);
        
        /// <summary>
        /// 添加消息
        /// 0:系统消息,1:到期通知,2:回访通知,3:工单提醒,4:账号审核提醒,5管理日报，6:分配通知,7:出单通知
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        int AddMessage(AddMessageRequest request);

        /// <summary>
        /// 已出单详情
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        MsgClosedOrderResponse MsgClosedOrder(MsgClosedOrderRequest request,IEnumerable<KeyValuePair<string, string>> pairs);

        /// <summary>
        /// 统计昨天续保量
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        LastDayReInfoTotalResponse LastDayReInfoTotal(LastDayReInfoTotalRequest request,IEnumerable<KeyValuePair<string, string>> pairs);
    }
}
