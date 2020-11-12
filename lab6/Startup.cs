using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(lab6.Startup))]
namespace lab6
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
