using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Team8_MVCApplication.Startup))]
namespace Team8_MVCApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
