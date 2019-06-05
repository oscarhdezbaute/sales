using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(sales.Backend.Startup))]
namespace sales.Backend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
