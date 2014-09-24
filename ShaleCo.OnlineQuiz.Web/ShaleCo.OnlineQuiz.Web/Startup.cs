using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ShaleCo.OnlineQuiz.Web.Startup))]
namespace ShaleCo.OnlineQuiz.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
