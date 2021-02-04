using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.ComponentModel;

namespace MVC_Officer_Mission.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        [DisplayName("الإسم")]
        public string Name { set; get; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("name=mission_db", throwIfV1Schema: false)
        {
        }
        public DbSet<Mission> Missions { get; set; }
        public DbSet<Officer> Officers { get; set; }
        public DbSet<Rank> Ranks { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<BenefitedSide> BenefitedSides { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<OfficerRole> OfficerRole { get; set; }
        public DbSet<Specification> Specifications { get; set; }
        public DbSet<ExternalTournament> ExternalTournaments { get; set; }
        public DbSet<MissionLog> MissionLogs { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}