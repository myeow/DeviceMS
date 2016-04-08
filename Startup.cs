using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DeviceMS.Startup))]
namespace DeviceMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
