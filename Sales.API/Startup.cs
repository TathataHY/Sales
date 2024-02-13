using Microsoft.Owin;

[assembly: OwinStartup(typeof(Sales.API.Startup))]
namespace Sales.API
{
    using Owin;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
