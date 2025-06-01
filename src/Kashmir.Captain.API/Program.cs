using Kashmir.Captain.Common;
using Kashmir.Captain.Common.Entities;
using Kashmir.Captain.Domain.Entities;
using KC.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using KC.Domain.Context;

namespace ODL.Mono.API
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var host = args.CreateHostBuilder<Startup>()
                .AddAppConfig()
                .Build();

            // Migrate IdDbContext and seed roles & super admin
            await host.MigrateDbContextAsync<IdDbContext>(async (context, services) =>
            {
                await SeedDefaultRolesAsync(services);
                await SeedSuperAdminAsync(context, services);
            });

            // Migrate InventoryDbContext without seeding
            await host.MigrateDbContextAsync<InventoryDbContext>();

            host.Run();
        }

        /// <summary>
        /// Migrates the specified DbContext and optionally runs a seeder.
        /// </summary>
        public static async Task<IHost> MigrateDbContextAsync<T>(this IHost host, Func<T, IServiceProvider, Task>? seeder = default) where T : DbContext
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<T>();

            try
            {
                await context.Database.MigrateAsync();

                if (seeder != null)
                {
                    await seeder(context, services);
                }
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<T>>();
                logger.LogError(ex, $"An error occurred migrating the database used by context {typeof(T).Name}");
                throw;
            }

            return host;
        }

        /// <summary>
        /// Seeds the Super Admin user and assigns the SuperAdmin role.
        /// </summary>
        public static async Task SeedSuperAdminAsync(IdDbContext context, IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<Role>>();
            var userManager = services.GetRequiredService<UserManager<User>>();

            string superAdminRoleName = RoleType.SuperAdmin.ToString();
            string superAdminEmail = "test@test.com";
            string superAdminPassword = "Password@123";

            // Create role if it doesn't exist
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

            // Create user if it doesn't exist
            var superAdminUser = await userManager.FindByEmailAsync(superAdminEmail);
            if (superAdminUser == null)
            {
                superAdminUser = new User(superAdminEmail, "Super", "Admin", "7006698882")
                {
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };

                var userResult = await userManager.CreateAsync(superAdminUser, superAdminPassword);
                if (!userResult.Succeeded)
                {
                    throw new Exception($"Failed to create SuperAdmin user: " +
                        string.Join(", ", userResult.Errors.Select(e => e.Description)));
                }
            }

            // Assign role to user if not already assigned
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

        /// <summary>
        /// Seeds default roles from global constants.
        /// </summary>
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
