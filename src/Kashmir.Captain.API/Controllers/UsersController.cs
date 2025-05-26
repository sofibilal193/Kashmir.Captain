using Kashmir.Captain.API.Common;
using Kashmir.Captain.Application.Identity;
using Kashmir.Captain.Common.Entities;
using Kashmir.Captain.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kashmir.Captain.Controllers
{
    [Route("api/users")]
    public class UsersController : BaseController
    {

        /// <summary>
        /// Get User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId}")]
        [Authorize(Policy = nameof(RoleType.User))]
        public async Task<IActionResult> GetUserAsync(int userId)
        {
            var response = await Mediator.Send(new GetUserQuery(userId));
            return Ok(response);
        }

        /// <summary>
        /// Get all Users
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="sort"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllUserAsync(int page, int pageSize, string? sort, string? search)
        {
            var response = await Mediator.Send(new GetUsersQuery(page, pageSize, sort, search));
            return Ok(response);
        }
    }
}