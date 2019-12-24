using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FinalSite.Startup))]
namespace FinalSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
