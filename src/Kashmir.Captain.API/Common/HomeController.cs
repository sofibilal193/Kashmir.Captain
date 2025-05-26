#pragma warning disable CA1822

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kashmir.Captain.API.Common
{
    // [OpenApiIgnore]
    [AllowAnonymous]
    [Route("Home")]
    public class HomeController : BaseController
    {
        // GET: /<controller>/
        [HttpGet]
        public IActionResult Index()
        {
            return new RedirectResult("~/swagger");
        }
    }
}
