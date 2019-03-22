using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace BiHuManBu.MessageCenter.API.SignalRDemo
{
    [HubName("ViewDataHub2")]
    public class ViewDataHub2:Hub
    {
        //this is just called by client and return the value for it .
        public string Hello(string msg)
        {

            string res = "sorry ,i don't know";
            if (msg.Contains("hello"))
            {
                res = "hello ,guy";
            }
            if (msg.Contains("how"))
            {
                res = "how are you ~";
            }
            if (msg.Contains("love"))
            {
                res = "don't fall in love with me ~";
            }
            return res;
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