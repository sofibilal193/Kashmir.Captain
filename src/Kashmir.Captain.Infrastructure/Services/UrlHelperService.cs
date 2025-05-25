using Kashmir.Captain.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;

namespace Kashmir.Captain.Infrastructure.Services
{
	public class UrlHelperService : IUrlHelperService
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IUrlHelperFactory _urlHelperFactory;

		public UrlHelperService(IHttpContextAccessor httpContextAccessor, IUrlHelperFactory urlHelperFactory)
		{
			_httpContextAccessor = httpContextAccessor;
			_urlHelperFactory = urlHelperFactory;
		}

		public string GenerateUrl(string action, string controller, object? values = null)
		{
			var httpContext = _httpContextAccessor.HttpContext;
			if (httpContext == null)
			{
				throw new InvalidOperationException("No active HttpContext found.");
			}

			var actionContext = new ActionContext(httpContext, httpContext.GetRouteData(), new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor());
			var urlHelper = _urlHelperFactory.GetUrlHelper(actionContext);

			var scheme = httpContext.Request.Scheme;
			var host = httpContext.Request.Host.Value;

			return urlHelper.Action(action, controller, values, scheme, host) ?? "Can not Create url";
		}
	}
}