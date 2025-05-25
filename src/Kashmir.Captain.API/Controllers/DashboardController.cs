using Kashmir.Captain.API.Common;
using Kashmir.Captain.Application.Home;
using Microsoft.AspNetCore.Mvc;

namespace Kashmir.Captain.Controllers
{
	[Route("api/Dashboards")]
	public class DashboardController : BaseController
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

		/// <summary>
		/// Upsert Worker
		/// </summary>
		/// <param name="id"></param>
		/// <param name="command"></param>
		/// <returns></returns>
		[HttpPut("update")]
		public async Task<IActionResult> UpdatetToolAsync(int id, UpsertToolCommand command)
		{
			command.SetId(id);
			var response = await Mediator.Send(command);
			return Ok(response);
		}

		[HttpGet("tools")]
		public async Task<IActionResult> GetToolsAsync()
		{
			var response = await Mediator.Send(new GetToolsQuery());
			return Ok(response);
		}
	}
}