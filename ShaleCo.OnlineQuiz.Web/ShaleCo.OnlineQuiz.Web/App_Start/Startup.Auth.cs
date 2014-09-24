using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using ShaleCo.OnlineQuiz.Web.Models;
using System;
using System.Web.Configuration;

namespace ShaleCo.OnlineQuiz.Web
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });
            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            //Create UserRoles if they do not exist yet
            var database = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(database));

            var roles = Enum.GetValues(typeof(UserRoles));

            foreach(var role in roles)
            {
                if (!roleManager.RoleExists<IdentityRole>(role.ToString()))
                {
                    roleManager.Create(new IdentityRole(role.ToString()));
                }
            }

            //Create Admin Account
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(database));
            var adminUserName = WebConfigurationManager.AppSettings["AdminUserName"];
            var adminPassword = WebConfigurationManager.AppSettings["AdminPassword"];

            if(userManager.FindByName(adminUserName) == null)
            {
                userManager.Create(new ApplicationUser() { UserName = adminUserName }, adminPassword);
            }

            database.SaveChanges();


            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            //app.UseGoogleAuthentication();
        }
    }
}