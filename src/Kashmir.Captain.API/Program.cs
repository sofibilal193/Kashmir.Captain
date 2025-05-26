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

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;
        var env = builder.Environment;

        var logger = LoggerFactory.Create(logging =>
        {
            logging.AddConsole();
        }).CreateLogger("StartupLogger");

        var appSettings = new AppSettings();
        configuration.Bind(appSettings);

        builder.Services.AddApplication(configuration, env, logger);

        builder.Services.AddDbContext<KcDbContext>(options =>
            options.UseSqlServer(appSettings.ConnectionStrings.KcDbContext
            ?? throw new InvalidOperationException("Connection string 'KcDbContext' not found.")));

        builder.Services.AddIdentity<User, Role>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
        })
        .AddEntityFrameworkStores<KcDbContext>()
        .AddDefaultTokenProviders();

        builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(LoggingBehavior<,>).Assembly));

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy(nameof(RoleType.User), policy => policy.RequireRole(nameof(RoleType.SuperAdmin), nameof(RoleType.Admin), nameof(RoleType.User)));
            options.AddPolicy(nameof(RoleType.Admin), policy => policy.RequireRole(nameof(RoleType.SuperAdmin), nameof(RoleType.Admin)));
            options.AddPolicy(nameof(RoleType.SuperAdmin), policy => policy.RequireRole(nameof(RoleType.SuperAdmin)));
        });

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        builder.Services.AddControllers();
        builder.Services.AddAutoMapper(typeof(MappingProfile));

        builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        builder.Services.AddScoped<IUrlHelper>(factory =>
        {
            var actionContext = factory.GetService<IActionContextAccessor>()?.ActionContext ?? new ActionContext();
            return new UrlHelper(actionContext);
        });

        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        builder.Services.AddSingleton<IUrlHelperService, UrlHelperService>();

        builder.Services.AddFluentValidationAutoValidation()
                        .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        builder.Services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

        builder.Services.AddSingleton(serviceProvider =>
        {
            var emailSettings = serviceProvider.GetRequiredService<IOptions<EmailSettings>>().Value;
            return new SmtpClient(emailSettings.SmtpServer)
            {
                Port = emailSettings.SmtpPort,
                Credentials = new NetworkCredential(emailSettings.SenderEmail, emailSettings.SenderPassword),
                EnableSsl = emailSettings.EnableSsl,
            };
        });

        builder.Services.AddScoped<IEmailService, EmailService>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddHttpContextAccessor();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
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
            });
        });

        var app = builder.Build();

        app.UseApplication(env, builder.Services, true);

        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            await InitializeRoles(roleManager);
            await CreateSuperAdminAssignRole(userManager);
        }

        app.Use(async (context, next) =>
        {
            if (context.Request.Path == "/")
            {
                context.Response.Redirect("/swagger");
                return;
            }
            await next();
        });

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors("AllowAll");
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
            });
        }

        app.UseMiddleware<ExceptionMiddleware>();

        await app.RunAsync();
    }

    private static async Task InitializeRoles(RoleManager<Role> roleManager)
    {
        foreach (var roleName in GlobalConstants.Roles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var role = new Role(roleName);
                await roleManager.CreateAsync(role);
            }
        }
    }

    private static async Task CreateSuperAdminAssignRole(UserManager<User> userManager)
    {
        var superAdmin = nameof(RoleType.SuperAdmin);
        var email = "test@test.com";

        var user = await userManager.FindByEmailAsync(email);

        if (user == null)
        {
            user = new User
            {
                UserName = email,
                Email = email,
                FirstName = "Bilal",
                LastName = "Sofi",
                PhoneNumber = "7006698882"
            };
            await userManager.CreateAsync(user, "Password@123");
        }

        if (!await userManager.IsInRoleAsync(user, superAdmin))
        {
            await userManager.AddToRoleAsync(user, superAdmin);
        }
    }
}
