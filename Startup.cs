using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ExpensesMVC2018.Startup))]
namespace ExpensesMVC2018
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
