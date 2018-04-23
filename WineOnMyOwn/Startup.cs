using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WineOnMyOwn.Startup))]
namespace WineOnMyOwn
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
