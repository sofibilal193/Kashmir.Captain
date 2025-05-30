using FluentValidation;
using FluentValidation.AspNetCore;
using KC.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using System.Net;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Kashmir.Captain.Domain.Entities;
using KC.Common;
using Kashmir.Captain.Application.Services;
using Kashmir.Captain.Infrastructure.Services;
using Kashmir.Captain.Application.Identity;
using Kashmir.Captain.Domain;
using Kashmir.Captain.Application.Behaviors;
using ODL.Application.Common;
using Kashmir.Captain.Application;
using Kashmir.Captain.API;
using Kashmir.Captain.Common.Entities;
using Kashmir.Captain.Common;

namespace ODL.Mono.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostEnvironment Environment { get; }
        private IServiceCollection _services = new ServiceCollection();

        private readonly ILogger _logger;

        public Startup(IConfiguration configuration,
            IHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
            _logger = LoggerFactory.Create(logging =>
                    {
                        logging.AddConsole();
                    }).CreateLogger("StartupLogger");
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication(Configuration, Environment, _logger);

            var appSettings = new AppSettings();
            Configuration.Bind(appSettings);

            services.AddApplication(Configuration, Environment, _logger);

            services.AddDbContext<KcDbContext>(options =>
                options.UseSqlServer(appSettings.ConnectionStrings.KcDbContext
                ?? throw new InvalidOperationException("Connection string 'KcDbContext' not found."),

                sqlOptions => sqlOptions.MigrationsAssembly("Kashmir.Captain.Domain")));

            services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
            })
            .AddEntityFrameworkStores<KcDbContext>()
            .AddDefaultTokenProviders();

            services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(LoggingBehavior<,>).Assembly));

            services.AddAuthorization(options =>
            {
                options.AddPolicy(nameof(RoleType.User), policy => policy.RequireRole(nameof(RoleType.SuperAdmin), nameof(RoleType.Admin), nameof(RoleType.User)));
                options.AddPolicy(nameof(RoleType.Admin), policy => policy.RequireRole(nameof(RoleType.SuperAdmin), nameof(RoleType.Admin)));
                options.AddPolicy(nameof(RoleType.SuperAdmin), policy => policy.RequireRole(nameof(RoleType.SuperAdmin)));
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                    {
                        policy.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            services.AddControllers();
            services.AddAutoMapper(typeof(MappingProfile));

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(factory =>
            {
                var actionContext = factory.GetService<IActionContextAccessor>()?.ActionContext ?? new ActionContext();
                return new UrlHelper(actionContext);
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IUrlHelperService, UrlHelperService>();

            services.AddFluentValidationAutoValidation()
                            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));

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

            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddHttpContextAccessor();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
                }
                );
            });

            // add health checks
            services.AddAppHealthChecks();

            _services = services;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment env, IConfiguration config)
        {
            app.UseApplication(env, _services, true);

            app.Use(async (context, next) =>
       {
           if (context.Request.Path == "/")
           {
               context.Response.Redirect("/swagger");
               return;
           }
           await next();
       });

            app.UseCors("AllowFrontend");
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
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


        }
    }
}