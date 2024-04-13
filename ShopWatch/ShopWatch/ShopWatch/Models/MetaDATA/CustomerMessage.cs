using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopWatch.Models.MetaDATA
{
  
    public class CustomerMessage
        {
        public int id;
        public string name;
        public string avatar;
        public string  chat;

        public CustomerMessage(int id,string name, string avatar, string chat)
        {
            this.id = id;
            this.avatar = avatar;
            this.name = name;
            this.chat = chat;

        }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Avatar { get; set; }
        public string Message { get; set; }
        /* public string Name)*/
    }
}