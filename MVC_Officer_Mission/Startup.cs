using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVC_Officer_Mission.Startup))]
namespace MVC_Officer_Mission
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
