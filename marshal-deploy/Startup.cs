using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(marshal_deploy.Startup))]
namespace marshal_deploy
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
