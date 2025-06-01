using Kashmir.Captain.Common;
using Kashmir.Captain.Application.Identity;
using Kashmir.Captain.Common.Entities;
using Kashmir.Captain.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kashmir.Captain.Controllers
{
    [Route("api/roles")]
    public class RolesController : BaseController
    {

        /// <summary>
		/// Upsert role to User
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="role"></param>
		/// <param name="AssignRole"> True: Add-Role, False: Delete-Role</param>
		/// <returns></returns>
		[HttpPost("Upsertrole")]
        public async Task<IActionResult> UpsertRoleAsync(int userId, RoleType role, bool AssignRole)
        {
            var response = await Mediator.Send(new UpsertRoleCommand(userId, role, AssignRole));
            return Ok(response);
        }

        /// <summary>
		/// Remove role to User
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		[HttpDelete("Deleterole")]
        public async Task<IActionResult> DeleteRoleAsync(int userId)
        {
            var response = await Mediator.Send(new DeleteUserRoleCommand(userId));
            return Ok(response);
        }
    }
}