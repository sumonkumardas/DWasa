using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AplombTech.WMS.Site.Startup))]
namespace AplombTech.WMS.Site
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
