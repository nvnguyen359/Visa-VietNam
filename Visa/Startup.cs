using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Visa.Startup))]
namespace Visa
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
