using Microsoft.AspNet.Identity.EntityFramework;

namespace ShaleCo.OnlineQuiz.Web.Models
{
    public enum UserRoles
    {
        Admin,
        Teacher,
        Student
    }
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string Teacher { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }
    }
}