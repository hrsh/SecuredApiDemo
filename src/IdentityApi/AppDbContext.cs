using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityApi
{
    public class AppDbContext : IdentityDbContext<AppUser, AppUserRole, string>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }

    public class Seed
    {
        public static async Task SeedDatabase(
            UserManager<AppUser> userManager,
            RoleManager<AppUserRole> roleManager)
        {
            var user1 = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                AccessFailedCount = 0,
                Email = "user1@email.com",
                EmailConfirmed = true,
                Fullname = "Mazdak Shojaie",
                LockoutEnabled = true,
                PhoneNumber = "09173148953",
                PhoneNumberConfirmed = true,
                UserName = "username1"
            };
            var user2 = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                AccessFailedCount = 0,
                Email = "user2@email.com",
                EmailConfirmed = true,
                Fullname = "Maryam Shojaie",
                LockoutEnabled = true,
                PhoneNumber = "09173154753",
                PhoneNumberConfirmed = true,
                UserName = "username2"
            };

            if (userManager.Users.Any()) return;
            if (roleManager.Roles.Any()) return;

            await userManager.CreateAsync(user1, "Pa$$word@1");
            await userManager.CreateAsync(user2, "Pa$$word@2");

            var role1 = new AppUserRole
            {
                Name = "Admin"
            };
            var role2 = new AppUserRole
            {
                Name = "Teacher"
            };

            await roleManager.CreateAsync(role1);
            await roleManager.CreateAsync(role2);

            await userManager.AddToRoleAsync(user1, "Admin");
            await userManager.AddToRoleAsync(user2, "Teacher");
        }
    }
}
