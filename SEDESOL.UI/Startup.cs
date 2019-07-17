using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SEDESOL.UI.Startup))]
namespace SEDESOL.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
