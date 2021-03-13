using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace IdentityApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using var scope = host.Services.CreateScope();
            using var context = scope
                .ServiceProvider
                .GetRequiredService<AppDbContext>();
            context.Database.Migrate();

            var userManagerService = scope
                .ServiceProvider
                .GetRequiredService<UserManager<AppUser>>();

            var roleManagerService = scope
                .ServiceProvider
                .GetRequiredService<RoleManager<AppUserRole>>();

            await Seed.SeedDatabase(userManagerService, roleManagerService);

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
