using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(APP_PUCCOINS_oAUTH_oAUTH.Startup))]
namespace APP_PUCCOINS_oAUTH_oAUTH
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
