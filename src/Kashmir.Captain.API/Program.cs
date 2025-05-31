using Kashmir.Captain.Common;
using Kashmir.Captain.Common.Entities;
using Kashmir.Captain.Domain.Entities;
using KC.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ODL.Mono.API
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var host = args.CreateHostBuilder<Startup>()
                .AddAppConfig()
                .Build();

            await host.MigrateDbContextAsync<IdDbContext>(async (context, services) =>
                {
                    await SeedDefaultRolesAsync(services);
                    await SeedSuperAdminAsync(context, services);
                });
            host.Run();
        }

        public static async Task<IHost> MigrateDbContextAsync<T>(
            this IHost host,
            Func<T, IServiceProvider, Task> seeder) where T : DbContext
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<T>();

            try
            {
                await context.Database.MigrateAsync();
                await seeder(context, services);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<T>>();
                logger.LogError(ex, $"An error occurred migrating the database used by context {typeof(T).Name}");
                throw;
            }

            return host;
        }

        public static async Task SeedSuperAdminAsync(IdDbContext context, IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<Role>>();
            var userManager = services.GetRequiredService<UserManager<User>>();

            string superAdminRoleName = RoleType.SuperAdmin.ToString();
            string superAdminEmail = "test@test.com";
            string superAdminPassword = "Password@123";

            if (!await roleManager.RoleExistsAsync(superAdminRoleName))
            {
                var role = new Role { Name = superAdminRoleName };
                var roleResult = await roleManager.CreateAsync(role);
                if (!roleResult.Succeeded)
                {
                    throw new Exception($"Failed to create role {superAdminRoleName}: " +
                        string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                }
            }

            // Create SuperAdmin user if it doesn't exist
            var superAdminUser = await userManager.FindByEmailAsync(superAdminEmail);
            if (superAdminUser == null)
            {
                superAdminUser = new User(superAdminEmail, "Super", "Admin", "7006698882");
                superAdminUser.EmailConfirmed = true;
                superAdminUser.PhoneNumberConfirmed = true;

                var userResult = await userManager.CreateAsync(superAdminUser, superAdminPassword);
                if (!userResult.Succeeded)
                {
                    throw new Exception($"Failed to create SuperAdmin user: " +
                        string.Join(", ", userResult.Errors.Select(e => e.Description)));
                }
            }

            // Assign SuperAdmin role to user if not already assigned
            if (!await userManager.IsInRoleAsync(superAdminUser, superAdminRoleName))
            {
                var addToRoleResult = await userManager.AddToRoleAsync(superAdminUser, superAdminRoleName);
                if (!addToRoleResult.Succeeded)
                {
                    throw new Exception($"Failed to add SuperAdmin user to role: " +
                        string.Join(", ", addToRoleResult.Errors.Select(e => e.Description)));
                }
            }
        }

        public static async Task SeedDefaultRolesAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<Role>>();

            var roles = GlobalConstants.Roles;

            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var role = new Role(roleName);
                    var result = await roleManager.CreateAsync(role);
                    if (!result.Succeeded)
                    {
                        throw new Exception($"Failed to create role '{roleName}': " +
                            string.Join(", ", result.Errors.Select(e => e.Description)));
                    }
                }
            }
        }

    }
}
