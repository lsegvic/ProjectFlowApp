using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using PracenjeTijekaProjekta.Models;

[assembly: OwinStartupAttribute(typeof(PracenjeTijekaProjekta.Startup))]
namespace PracenjeTijekaProjekta
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
           
        }
    }
}
