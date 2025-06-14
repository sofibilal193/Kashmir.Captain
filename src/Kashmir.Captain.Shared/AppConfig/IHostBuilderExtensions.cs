using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Kashmir.Captain.Shared
{
    [ExcludeFromCodeCoverage]
    public static class IHostBuilderExtensions
    {
        public static IHostBuilder CreateHostBuilder<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors
            | DynamicallyAccessedMemberTypes.PublicMethods)] TStartup>(this string[] args)
                where TStartup : class
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureLogging((ctx, logging) =>
                {
                    var env = ctx.HostingEnvironment;
                    AddLogging(env, logging);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(serverOptions =>
                    {
                        // Set properties and call methods on options
                        serverOptions.AddServerHeader = false;
                    })
                    .UseStartup<TStartup>();
                });
        }

        public static void AddLogging(this IHostEnvironment env, ILoggingBuilder loggingBuilder)
        {
            if (!env.IsDevelopment() && !env.IsEnvironment("Test"))
            {
                // loggingBuilder.AddAzureWebAppDiagnostics();
                // loggingBuilder.AddApplicationInsights();
            }
            else
            {
                loggingBuilder.AddDebug();
                loggingBuilder.AddConsole();
            }
        }

        public static IHostBuilder AddAppConfig(this IHostBuilder builder)
        {
            return builder.ConfigureAppConfiguration((builder) =>
            { });
        }
    }
}