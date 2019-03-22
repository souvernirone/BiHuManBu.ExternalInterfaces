using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace BiHuManBu.MessageCenter.API.RealTimeConnection
{
    public class DrawingBoard : Hub
    {
        private const int BoardWidth = 300;
        private const int BoardHeight = 300;

        private static int[,] _buffer = GetEmptyBuffer();
        
        public Task BroadcastPoint(int x, int y)
        {
            if (x < 0) x = 0;
            if (x >= BoardWidth) x = BoardWidth + 1;
            if (y < 0) y = 0;
            if (y >= BoardHeight) y = BoardHeight + 1;

            int color = 0;
            int.TryParse(Clients.Caller.color, out color);
            _buffer[x, y] = color;
            return Clients.Others.DrawPoint(x, y, Clients.Caller.color);
        }


        public Task BroadcastClear()
        {
            _buffer = GetEmptyBuffer();
            return Clients.Others.Clear();
        }

        public override Task OnConnected()
        {
            return Clients.Caller.Update(_buffer);
        }


        public   Task Send()
        {
           // return Clients.User("102").message("哈哈，我进来了");
           // await Clients.All.message("哈哈哈哈 ，我们都知道了");
            var ctx = GlobalHost.ConnectionManager.GetHubContext<DrawingBoard>();
          return   ctx.Clients.All.sendmessage("哈哈哈哈");
        }

        public async Task SendMessage()
        {
            // return Clients.User("102").message("哈哈，我进来了");
            // await Clients.All.message("哈哈哈哈 ，我们都知道了");
            var ctx = GlobalHost.ConnectionManager.GetHubContext<DrawingBoard>();
            await ctx.Clients.All.sendmessage("哈哈哈哈");
        }
        public async  Task TopicInfo()
        {
            var ctx = GlobalHost.ConnectionManager.GetHubContext<DrawingBoard>();
            await ctx.Clients.User("102").sendmessage("哈哈，我进来了");
            // await Clients.All.message("哈哈哈哈 ，我们都知道了");
            //var ctx = GlobalHost.ConnectionManager.GetHubContext<DrawingBoard>();
            //await ctx.Clients.All.sendmessage("哈哈哈哈99999999999999999");
        }
        private static int[,] GetEmptyBuffer()
        {
            var buffer = new int[BoardWidth, BoardHeight];
            return buffer;
        }
    }
}