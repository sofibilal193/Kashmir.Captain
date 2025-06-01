using Kashmir.Captain.Comman;
using Kashmir.Captain.Common.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Kashmir.Captain.Common
{
    [ApiController]
    [Authorize(Policy = nameof(RoleType.User))]
    [EnableCors("AllowFrontend")]
    [ApiConventionType(typeof(ApiConventions))]
    public abstract class BaseController : ControllerBase
    {
        private IMediator? _mediator;
        private IHostEnvironment? _env;
        private AppSettings? _settings;
        private ILogger? _logger;
        private ICurrentUser? _user;

        protected ILogger GetLogger<T>()
            where T : class
        {
            return _logger ??= HttpContext.RequestServices.GetRequiredService<ILogger<T>>();
        }

        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();

        protected IHostEnvironment HostEnv => _env ??= HttpContext.RequestServices.GetRequiredService<IHostEnvironment>();

        protected AppSettings AppSettings => _settings ??= HttpContext.RequestServices.GetRequiredService<IOptionsMonitor<AppSettings>>().CurrentValue;

        protected ICurrentUser CurrentUser => _user ??= HttpContext.RequestServices.GetRequiredService<ICurrentUser>();

        protected static string? OperationId => System.Diagnostics.Activity.Current?.RootId;

        protected string BaseUrl => string.Format("{0}://{1}",
            Request.Scheme, Request.Host);
    }
}
