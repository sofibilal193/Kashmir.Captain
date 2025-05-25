using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Kashmir.Captain.Application.Behaviors;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ODL.Application.Common
{
    [ExcludeFromCodeCoverage]
    public static class DependencyInjection
    {
        // private static readonly AppSettings _settings = new();

        public static IServiceCollection AddApplication(this IServiceCollection services,
            IConfiguration configuration, IHostEnvironment env, ILogger logger)
        {
            // services.AddSingleton(configuration);
            // services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
            // configuration.GetSection("AppSettings").Bind(_settings);

            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");

            services.AddHttpContextAccessor();

            // Configure authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                };
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                // Configure cookie options if needed
                options.Cookie.Name = "KcCookies";
            });


            // services
            //     .AddControllers(options => options.Filters.Add(typeof(HttpGlobalExceptionFilter)))
            //     .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true)
            //     .AddJsonOptions(options =>
            //     {
            //         options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            //         options.JsonSerializerOptions.Converters.Add(new TrimStringJsonConverter());
            //     });

            // services.AddScoped<ICurrentUser, HttpCurrentUser>();

            // https://andrewlock.net/handling-web-api-exceptions-with-problemdetails-middleware/

            // add Mediatr
            services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(LoggingBehavior<,>).Assembly));


            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachePipelineBehavior<,>));
            // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

            // configure cookie options
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = _ => true;
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
            });

            // add HTTP logging
            services.AddHttpLogging(options =>
            {
                options.LoggingFields = HttpLoggingFields.All;
                // log header values needed to debug reverse proxy issues
                options.RequestHeaders.Add("X-Forwarded-Host");
                options.RequestHeaders.Add("X-ORIGINAL-HOST");
                options.RequestHeaders.Add("X-Forwarded-PathBase");
                options.RequestHeaders.Add("X-Original-URL");
                options.RequestHeaders.Add("X-Azure-ClientIP");
                options.RequestHeaders.Add("X-Azure-SocketIP");
                options.RequestHeaders.Add("X-Forwarded-For");
                options.RequestHeaders.Add("X-Original-For");
                options.RequestHeaders.Add("X-Client-IP");
                options.RequestHeaders.Add("Client-IP");
            });



            return services;
        }

        public static IServiceCollection AddAppHealthChecks(this IServiceCollection services)
        {
            var hcBuilder = services.AddHealthChecks();
            hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy());

            var sb = services.BuildServiceProvider();

            var hcs = from t in Assembly.GetEntryAssembly()?.GetTypes()
                      where t.GetInterfaces().Contains(typeof(IHealthCheck))
                      select ActivatorUtilities.CreateInstance(sb, t) as IHealthCheck;
            if (hcs?.Count() > 0)
            {
                var tags = new[] { "custom" };
                foreach (var hc in hcs)
                {
                    hcBuilder.AddCheck(hc.GetType().Name.ToLowerInvariant().Replace("healthcheck", ""),
                        hc, HealthStatus.Degraded, tags);
                }
            }
            return services;
        }


        public static IApplicationBuilder UseApplication(this IApplicationBuilder app, IHostEnvironment env, IServiceCollection services, bool enableHttpLogging)
        {
            if (env.IsDevelopment())
                IdentityModelEventSource.ShowPII = true;


            if (!env.IsDevelopment() && !env.IsEnvironment("Test"))
            {
                // when in azure, ensure headers are forwarded to prevent too many multiple redirects issue
                // https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-2.2&tabs=visual-studio
                app.UseXForwardedHeaders();
            }

            if (enableHttpLogging)
            {
                app.UseHttpLogging();
            }

            if (!env.IsProduction())
            {
                app.UseDeveloperExceptionPage();
                RegisteredServicesPage(app, services);
            }

            app.UseRouting();

            // UseCors with default policy.
            app.UseCors();

            // authentication & authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
            });

            return app;
        }


        private static IApplicationBuilder UseXForwardedHeaders(this IApplicationBuilder app)
        {
            var options = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            };

            // Only loopback proxies are allowed by default.
            // Clear that restriction because forwarders are being enabled by explicit configuration.
            // https://stackoverflow.com/a/56469499/5358985
            options.KnownNetworks.Clear();
            options.KnownProxies.Clear();

            app.UseForwardedHeaders(options);

            app.Use((context, next) =>
            {
                if (context.Request.Headers.TryGetValue("X-Forwarded-PathBase", out var pathBases))
                {
                    context.Request.PathBase = pathBases[0];
                }
                return next();
            });

            return app;
        }


        private static void RegisteredServicesPage(IApplicationBuilder app, IServiceCollection services)
        {
            app.Map("/services", builder => builder.Run(async context =>
            {
                var sb = new StringBuilder();
                sb.Append("<h1>Registered Services</h1>");
                sb.Append("<table><thead>");
                sb.Append("<tr><th>Type</th><th>Lifetime</th><th>Instance</th></tr>");
                sb.Append("</thead><tbody>");
                foreach (var svc in services)
                {
                    sb.Append("<tr>");
                    sb.Append("<td>").Append(svc.ServiceType.FullName).Append("</td>");
                    sb.Append("<td>").Append(svc.Lifetime).Append("</td>");
                    sb.Append("<td>").Append(svc.ImplementationType?.FullName).Append("</td>");
                    sb.Append("</tr>");
                }
                sb.Append("</tbody></table>");
                await context.Response.WriteAsync(sb.ToString());
            }));
        }
    }
}