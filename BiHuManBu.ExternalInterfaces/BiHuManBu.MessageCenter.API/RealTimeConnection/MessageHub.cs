
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace BiHuManBu.MessageCenter.API.RealTimeConnection
{
    [HubName("MessageHub")]
    public class MessageHub : Hub
    {
        public string GetList()
        {
            return "hello";
        }

        public void Send(string name,string message)
        {
            Clients.Others.talk(string.Format("{0}说:{1}",name,message));
        }
    }
}