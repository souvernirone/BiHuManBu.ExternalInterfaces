using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR;

namespace BiHuManBu.MessageCenter.API.SignalRDemo
{
    [HubName("ViewDataHub1")]
    public class ViewDataHub1:Hub
    {//this is just called by client and return the value for it .
        public string Hello()
        {
            return "hello";
        }
        //this fucntion will be called by client and the inside function 
        //Clients.Others.talk(message);
        //will be called by clinet javascript function .
        public void SendMessag(string message)
        {
            Clients.Others.talk(message);
        }
    }
}