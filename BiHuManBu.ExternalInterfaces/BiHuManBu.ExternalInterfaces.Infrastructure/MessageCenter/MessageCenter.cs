using System;
using BiHuManBu.ExternalInterfaces.Models;
using log4net;
using ServiceStack.Text;

namespace BiHuManBu.ExternalInterfaces.Infrastructure.MessageCenter
{
    public class MessageCenter : IMessageCenter
    {
        private readonly ILog log = LogManager.GetLogger("MSG");

        public MessageResult SendToMessageCenter(string data, string msgUrl, string queueName = "")
        {
            MessageResult message = new MessageResult();
            string url = string.Format("{0}/SetBusinessMessage", msgUrl);
            string postData = string.Format("data={0}&queueName={1}", data, queueName);
            string result;
            try
            {
                int ret = HttpWebAsk.Post(url, postData, out result);
                message = result.FromJson<MessageResult>();
                log.Info(string.Format("消息发送SendToMessageCenter: data:{0},url:{1}，响应结果：{2}", postData, url, result));
            }
            catch (Exception ex)
            {
                log.Info("消息发送SendToMessageCenter: data:" + postData + ",url:" + url + "\n 异常信息:" + ex.StackTrace + " \n " + ex.Message);
                throw new Exception("消息发送异常",new MessageException());
            }
            return message;
        }

        public int NotifyMessageCenter(long msgId, string msgUrl, int status)
        {
            log.Info(string.Format("消息发送NotifyMessageCenter: msgId:{0},msgUrl:{1},status:{2}", msgId, msgUrl, status));
            string ret = string.Empty;
            int errCode = 0;
            try
            {
                HttpWebAsk.Post(msgUrl + "/UpdateMessageStatus", string.Format("msgId={0}&status={1}", msgId, (int)status), out ret);
            }
            catch (Exception)
            {
                errCode = -1;
            }
            return errCode;
        }
    }
}
