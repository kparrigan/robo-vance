using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoboVance.Web
{
    [assembly: OwinStartup(typeof(RoboVance.Web.Startup))]
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}