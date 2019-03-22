using System;
using System.Collections.Generic;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace BiHuManBu.MessageCenter.API.SignalRDemo
{
    [HubName("viewDataHub3")]
    public class ViewDataHub3:Hub
    {
        [HubMethodName("refresh")]
         public List<Stock> Refresh()
         {
             return Stock.GetAll();
         }
 
         [HubMethodName("RefreshClients")]
         public void RefreshClients()
         {
             Clients.All.myrefresh(Stock.GetAll());
         }
    }

    public class Stock
    {
        private string opendoor;

        public string Opendoor
        {
            get { return opendoor; }
            set { opendoor = value; }
        }

        private double price;

        public double Price
        {
            get { return price; }
            set { price = value; }
        }

        private DateTime opentiem = System.DateTime.Now;

        public DateTime Opentiem
        {
            get { return opentiem; }
            set { opentiem = value; }
        }


        public static List<Stock> GetAll()
        {
            Random rand = new Random();
            List<Stock> list = new List<Stock>()
            {
                new Stock {Opendoor = "Door1", Price = rand.NextDouble()*100},
                new Stock {Opendoor = "Door2", Price = rand.NextDouble()*100},
                new Stock {Opendoor = "Door3", Price = rand.NextDouble()*100}
            };
            return list;
        }
    }
}