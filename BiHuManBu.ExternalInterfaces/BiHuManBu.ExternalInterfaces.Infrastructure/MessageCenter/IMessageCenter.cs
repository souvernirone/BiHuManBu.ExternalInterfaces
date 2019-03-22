using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Models;

namespace BiHuManBu.ExternalInterfaces.Infrastructure.MessageCenter
{
    public  interface IMessageCenter
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="data"></param>
        /// <param name="msgUrl"></param>
        /// <param name="queueName"></param>
        /// <returns></returns>
        MessageResult SendToMessageCenter(string data, string msgUrl, string queueName = "");
        /// <summary>
        /// 通知消息中心
        /// </summary>
        /// <param name="msgId">消息id</param>
        /// <param name="status">处理状态,1=处理成功，2=处理失败</param>
        int NotifyMessageCenter(long msgId, string msgUrl, int status);
    }
}
