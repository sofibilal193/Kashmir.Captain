using Kashmir.Captain.Application;
using Kashmir.Captain.Common;
using KC.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ODL.Mono.API
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            args.CreateHostBuilder<Startup>()
                .AddAppConfig()
                .Build()
                .MigrateDbContext<KcDbContext>((_, _) => { })

                .Run();
        }

        public static IHost MigrateDbContext<T>(this IHost host, Action<T, IServiceProvider> seeder) where T : DbContext
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<T>();

            try
            {
                context.Database.Migrate();
                seeder(context, services);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<T>>();
                logger.LogError(ex, $"An error occurred migrating the database used by context {typeof(T).Name}");
                throw;
            }

            return host;
        }

    }

}