using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EmailTemplate.Startup))]
namespace EmailTemplate
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
