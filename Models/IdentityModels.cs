using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DeviceMS.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        public virtual ICollection<Device> Devices { get; set; }
        
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        //Add Tables
        public DbSet<Device> Devices { get; set; }
        public DbSet<Software> Softwares { get; set; }
        public DbSet<DeviceToUser> DevicesToUsers { get; set; }
        public DbSet<SoftwareToDevice> SoftwaresToDevices { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        //public System.Collections.IEnumerable ApplicationUsers { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{

        //    ////// many to many
        //    ////modelBuilder.Entity<Device>()
        //    ////    .HasMany(c => c.ApplicationUsers)
        //    ////    .WithMany(i => i.Devices)
        //    ////    .Map(t => t.MapLeftKey("DeviceId")
        //    ////    .MapRightKey("Id").ToTable("DeviceApplicationUser"));

        //    ////modelBuilder.Entity<Device>()
        //    ////    .HasMany(c => c.Softwares).WithMany(i => i.Devices)
        //    ////    .Map(t => t.MapLeftKey("DeviceId")
        //    ////        .MapRightKey("SoftwareId")
        //    ////        .ToTable("DeviceSoftwares"));

        //    base.OnModelCreating(modelBuilder);
        //}

    }
}

