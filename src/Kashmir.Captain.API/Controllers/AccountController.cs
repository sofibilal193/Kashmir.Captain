using Kashmir.Captain.API.Common;
using Kashmir.Captain.Application.Identity;
using Kashmir.Captain.Common.Entities;
using Kashmir.Captain.Domain.Entities;
using KC.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Kashmir.Captain.API.Controllers
{
	[Route("api/Account")]
	public class AccountController : BaseController
	{
		/// <summary>
		/// Confirm EMail Call from Email
		/// </summary>
		/// <param name="token"></param>
		/// <param name="userId"></param>
		/// <returns></returns>
		[HttpGet("ConfirmEmail")]
		public async Task<IActionResult> ConfirmEmailAsync(string token, int userId)
		{
			var response = await Mediator.Send(new ConfirmUserEmailQuery(token, userId));
			return Ok(response);
		}

		/// <summary>
		/// Change Password 
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="command"></param>
		/// <returns></returns>
		[HttpPost("ChangePassword")]
		public async Task<IActionResult> ChangePasswordAsync(int userId, ChangeUserPasswordCommand command)
		{
			command.setId(userId);
			var response = await Mediator.Send(command);
			return Ok(response);
		}


		/// <summary>
		/// Forgot Password (Sends token to Email)
		/// </summary>
		/// <returns></returns>
		[HttpPost("ForgotPassword")]
		public async Task<IActionResult> ForgotPasswordAsync([FromBody] ForgotUserPasswordCommand command)
		{
			var response = await Mediator.Send(command);
			return Ok(response);
		}

		/// <summary>
		/// Internal : Reset Password (Get Token from Email and Call this API )
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns> 
		[HttpPost("ResetPassword")]
		public async Task<IActionResult> ResetPasswordAsync(ResetUserPasswordCommand command)
		{
			var response = await Mediator.Send(command);
			return Ok(response);
		}

		/// <summary>
		/// Resend Email Confirmation
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		[HttpPost("ResendConfirmationEmail")]
		public async Task<IActionResult> ResendConfirmationEmailAsync([FromBody] int userId)
		{
			var command = new ResendUserConfirmationEmailCommand();
			command.setId(userId);
			var response = await Mediator.Send(command);
			return Ok(response);
		}

		/// <summary>
		/// Confirm Email Change : Called from Email
		/// </summary>
		/// <param name="token"></param>
		/// <param name="userId"></param>
		/// <param name="newEmail"></param>
		/// <returns></returns>
		[HttpGet("ConfirmEmailChange")]
		public async Task<IActionResult> ConfirmEmailChangeAsync(int userId, string newEmail, string token)
		{
			var response = await Mediator.Send(new ConfirmUserEmailChangeQuery(userId, newEmail, token));
			return Ok(response);
		}

		/// <summary>
		/// Update User Profile
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="command"></param>
		/// <returns></returns>
		[HttpPost("UpdateProfile")]
		public async Task<IActionResult> UpdateProfileAsync(int userId, [FromBody] UpdateUserProfileCommand command)
		{
			command.setId(userId);
			var response = await Mediator.Send(command);
			return Ok(response);
		}

		/// <summary>
		/// Change Email Address
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="command"></param>
		/// <returns></returns>
		[HttpPost("ChangeEmail")]
		public async Task<IActionResult> ChangeEmailAsync(int userId, ChangeUserEmailCommand command)
		{
			command.setId(userId);
			var response = await Mediator.Send(command);
			return Ok(response);
		}

		/// <summary>
		/// Delete a User
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		[HttpDelete("DeleteUser")]
		public async Task<IActionResult> DeleteUserAsync(int userId)
		{
			var command = new DeleteUserCommand();
			command.setId(userId);
			var response = await Mediator.Send(command);
			return Ok(response);
		}
	}
}