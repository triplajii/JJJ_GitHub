using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TaskService.Startup))]
namespace TaskService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
