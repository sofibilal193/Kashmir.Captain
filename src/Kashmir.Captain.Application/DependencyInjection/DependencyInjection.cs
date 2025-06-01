using System.Globalization;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using FluentValidation;
using MediatR;
using Kashmir.Captain.Application.Behaviors;
using Microsoft.EntityFrameworkCore;
using Kashmir.Captain.Common.Entities;
using Microsoft.AspNetCore.Identity;
using Kashmir.Captain.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.Cookies;
using KC.Common;
using Kashmir.Captain.Common;
using FluentValidation.AspNetCore;
using Kashmir.Captain.Application.Identity;
using System.Net;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using Kashmir.Captain.Comman;
using Kashmir.Captain.Infrastructure;
using KC.Domain;
using KC.Domain.Context;

namespace Kashmir.Captain.Application
{
    public static class DependencyInjection
    {
        private static readonly AppSettings _settings = new();

        public static IServiceCollection AddApplication(this IServiceCollection services,
            IConfiguration configuration, IHostEnvironment env, ILogger logger)
        {
            services.AddInfrastructure(configuration);

            services.AddSingleton(configuration);
            services.Configure<AppSettings>(configuration);
            configuration.Bind(_settings);

            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");

            services.AddDbContext<IdDbContext>(options =>
     options.UseSqlServer(
         _settings.ConnectionStrings.IdDbContext
             ?? throw new InvalidOperationException("Connection string 'IdDbContext' not found."),
         sqlOptions => sqlOptions.MigrationsAssembly("Kashmir.Captain.Domain"))
 );

            services.AddDbContext<InventoryDbContext>(options =>
                options.UseSqlServer(
                    _settings.ConnectionStrings.InventoryDbContext
                        ?? throw new InvalidOperationException("Connection string 'InventoryDbContext' not found."),
                    sqlOptions => sqlOptions.MigrationsAssembly("Kashmir.Captain.Domain"))
            );


            services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
            })
            .AddEntityFrameworkStores<IdDbContext>()
            .AddDefaultTokenProviders();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(nameof(RoleType.User), policy =>
                    policy.RequireRole(nameof(RoleType.SuperAdmin), nameof(RoleType.Admin), nameof(RoleType.User)));
                options.AddPolicy(nameof(RoleType.Admin), policy =>
                    policy.RequireRole(nameof(RoleType.SuperAdmin), nameof(RoleType.Admin)));
                options.AddPolicy(nameof(RoleType.SuperAdmin), policy =>
                    policy.RequireRole(nameof(RoleType.SuperAdmin)));
            });



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
                    ValidIssuer = _settings.Jwt.Issuer,
                    ValidAudience = _settings.Jwt.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Jwt.Key))
                };
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Cookie.Name = "KcCookies";
            });

            services.AddAutoMapper(typeof(MappingProfile));

            // Cors
            services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });



            services.AddFluentValidationAutoValidation()
                .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // add Mediatr
            services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(LoggingBehavior<,>).Assembly));

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachePipelineBehavior<,>));
            // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

            // configure cookie options
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = _ => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddSingleton(serviceProvider =>
            {
                var emailSettings = serviceProvider.GetRequiredService<IOptions<EmailSettings>>().Value;
                return new SmtpClient(emailSettings.SmtpServer)
                {
                    Port = emailSettings.SmtpPort,
                    Credentials = new NetworkCredential(emailSettings.SenderEmail, emailSettings.SenderPassword),
                    EnableSsl = emailSettings.EnableSsl,
                };
            });

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddEndpointsApiExplorer();

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

            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly != null)
            {
                var hcs = from t in entryAssembly.GetTypes()
                          where typeof(IHealthCheck).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract
                          select ActivatorUtilities.CreateInstance(sb, t) as IHealthCheck;

                if (hcs?.Any() == true)
                {
                    var tags = new[] { "custom" };
                    foreach (var hc in hcs)
                    {
                        hcBuilder.AddCheck(
                            hc.GetType().Name.ToLowerInvariant().Replace("healthcheck", ""),
                            hc,
                            HealthStatus.Degraded,
                            tags);
                    }
                }
            }

            return services;
        }

        public static IApplicationBuilder UseApplication(this IApplicationBuilder app, IHostEnvironment env, IServiceCollection services, bool enableHttpLogging)
        {
            app.UseInfrastructure(env, services, true);

            if (env.IsDevelopment())
                IdentityModelEventSource.ShowPII = true;

            if (!env.IsDevelopment() && !env.IsEnvironment("Test"))
            {
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

            app.UseHttpsRedirection();

            app.UseRouting();

            // UseCors with named policy
            app.UseCors("AllowFrontend");

            // authentication & authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
            });

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                });
            }

            app.UseMiddleware<ExceptionMiddleware>();

            return app;
        }

        private static IApplicationBuilder UseXForwardedHeaders(this IApplicationBuilder app)
        {
            var options = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            };

            options.KnownNetworks.Clear();
            options.KnownProxies.Clear();

            app.UseForwardedHeaders(options);

            app.Use(async (context, next) =>
            {
                if (context.Request.Headers.TryGetValue("X-Forwarded-PathBase", out var pathBases))
                {
                    context.Request.PathBase = pathBases[0];
                }
                await next();
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
