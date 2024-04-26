using Microsoft.Owin;
using Owin;
using System;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(ShopWatch.Startup))]

namespace ShopWatch
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
          
            app.MapSignalR();
            
        }
    }
}
