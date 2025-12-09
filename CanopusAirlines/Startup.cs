using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CanopusAirlines.Startup))]
namespace CanopusAirlines
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
