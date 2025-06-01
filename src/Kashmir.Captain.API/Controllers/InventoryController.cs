using Kashmir.Captain.API.Common;
using Kashmir.Captain.Application.Home;
using Microsoft.AspNetCore.Mvc;

namespace Kashmir.Captain.Controllers
{
    [Route("api/inventory")]
    public class InventoryController : BaseController
    {
        /// <summary>
        /// Create Tool
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("add")]
        public async Task<IActionResult> AddToolAsync(UpsertToolCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }
    }
}