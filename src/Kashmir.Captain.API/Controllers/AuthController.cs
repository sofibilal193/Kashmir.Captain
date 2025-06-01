using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;
using Kashmir.Captain.Application.Identity;
using Kashmir.Captain.Common;

namespace Kashmir.Captain.Controllers
{
    [Route("api/auth")]
    public class AuthController : BaseController
    {
        #region RegisterUserAsync
        /// <summary>
        /// Register User
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUserAsync(RegisterUserCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }
        #endregion

        /// <summary>
        /// Login User
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginUserCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        /// <summary>
        /// Log out User
        /// </summary>
        /// <returns></returns> 
        [HttpPost("logout")]
        public async Task<ActionResult<bool>> LogoutAsync()
        {
            return await Mediator.Send(new LogOutUserCommand());
        }
    }
}