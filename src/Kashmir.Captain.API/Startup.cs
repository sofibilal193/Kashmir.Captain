using Microsoft.OpenApi.Models;
using Kashmir.Captain.Comman;
using Kashmir.Captain.Application;
using Kashmir.Captain.Common;
using Kashmir.Captain.Common.Extensions;
using Kashmir.Captain.Common.Crypto;
using ICryptoProvider = Kashmir.Captain.Common.Crypto.ICryptoProvider;

namespace Kashmir.Captain.API
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

            var appSettings = new AppSettings();
            Configuration.Bind(appSettings);

            services.AddApplication(Configuration, Environment, _logger);

            services.AddControllers();
            // services.AddScoped<IDbContext>(x => x.GetRequiredService<TContext>());

            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));

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

            services.Configure<CryptoOptions>(Configuration.GetSection("CryptoOptions"));

            services.AddSingleton<ICryptoProvider, CryptoProvider>();
            services.AddScoped<ICurrentUser, HttpCurrentUser>();
            services.AddTransient<IDateTime, UtcDateTime>();

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
        }
    }
}