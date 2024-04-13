using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ShopWatch.Hubs
{
    [Microsoft.AspNet.SignalR.Hubs.HubName("chat")]
    public class DemoChat : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }
        public void Message(string message)
        {
            Clients.All.message(message);
        }
        public void CancelOrderAndNotify(int orderId, string tenkhachhang, string avatar, string makhachhang, string noidung, DateTime ngayhuy, string title)
        {
           
                  Clients.All.SendAsync("ReceiveNotification", $"Đã hủy đơn hàng {orderId}. Thông tin khách hàng: {tenkhachhang}, {avatar}, {makhachhang}, {noidung}, {ngayhuy}, {title}");
        }
    }
}