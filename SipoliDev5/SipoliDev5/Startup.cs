using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SipoliDev5.Startup))]
namespace SipoliDev5
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
