using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using ShopWatch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopWatch
{
    [HubName("nottification")]
    public class MyHub1 : Hub
    {
        private DHEntities db = new DHEntities();
        public void Hello()
        {
            Clients.All.hello();
        }
        public void SendNotification(NOTIFICATION message)
        {

            db.NOTIFICATIONs.Add(message);
            db.SaveChanges();
            Clients.All.displayNotification(message);
             
        }
        public void SendNotificationToUser(string email, string message)
        {
            Clients.User(email).displayNotification(message);
        
        }
    }
}