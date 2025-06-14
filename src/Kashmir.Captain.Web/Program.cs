using Kashmir.Captain.Web.Components;
using Kashmir.Captain.Application;
using Kashmir.Captain.Application.Behaviors;
using MediatR;
using Kashmir.Captain.Shared;
using Kashmir.Captain.Infrastructure;
using Kashmir.Captain.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Kashmir.Captain.Domain;
using Kashmir.Captain.Domain.Entities;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

var appSettings = new AppSettings();
configuration.Bind(appSettings);

builder.Services.AddDbContext<IdDbContext>(options => options.UseSqlServer(appSettings.ConnectionStrings.IdDbContext
           ?? throw new InvalidOperationException("Connection string 'IdDbContext' not found."),
       sqlOptions => sqlOptions.MigrationsAssembly("Kashmir.Captain.Domain"))
);

builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<IdDbContext>()
    .AddDefaultTokenProviders();

// builder.Services.AddScoped<IIdentityDbContext>(provider => provider.GetRequiredService<IdDbContext>());

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMediatR(config =>
    config.RegisterServicesFromAssembly(typeof(ApplicationAssemblyMarker).Assembly));

// add Mediatr
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(LoggingBehavior<,>).Assembly));

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
// builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
