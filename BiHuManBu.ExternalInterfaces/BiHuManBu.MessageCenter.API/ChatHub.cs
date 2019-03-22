using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace BiHuManBu.MessageCenter.API
{
    public class ChatHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }

        public void Send(string name)
        {
            Clients.All.addNewMessageToPage(name);
            // Clients.User("102").addNewMessageToPage(name);
        }


        public Task SendMessage(string message)
        {
          return  Clients.All.alert(message);
        } 
    }
}