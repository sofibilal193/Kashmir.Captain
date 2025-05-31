using Kashmir.Captain.Comman;
using Kashmir.Captain.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Kashmir.Captain.Infrastructure
{
    public static class DependencyInjection
    {
        private static readonly AppSettings _settings = new();

        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<IEmailService, EmailService>();
            services.AddSingleton<IUrlHelperService, UrlHelperService>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(factory =>
            {
                var actionContext = factory.GetService<IActionContextAccessor>()?.ActionContext ?? new ActionContext();
                return new UrlHelper(actionContext);
            });

            // services.AddScoped<IUserRepository, UserRepository>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            return services;
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app, IHostEnvironment env, IServiceCollection services, bool enableHttpLogging)
        {
            return app;
        }
    }
}