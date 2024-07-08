using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DroidShare.Startup))]
namespace DroidShare
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
