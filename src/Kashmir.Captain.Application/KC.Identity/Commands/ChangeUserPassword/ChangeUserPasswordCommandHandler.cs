using Kashmir.Captain.Domain.Entities;
using KC.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Kashmir.Captain.Application.Identity
{
	public class ChangeUserPasswordCommandHandler : IRequestHandler<ChangeUserPasswordCommand, string>
	{
		private readonly UserManager<User> _userManager;

		public ChangeUserPasswordCommandHandler(UserManager<User> userManager)
		{
			_userManager = userManager;
		}

		public async Task<string> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
		{

			// Find the user by their ID
			var user = await _userManager.FindByIdAsync($"{request.UserId}");
			if (user == null)
				return "Unauthorized()";

			var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
			if (result.Succeeded)
				return "Password changed successfully.";
			return "BadRequest(ModelState)";

		}
	}
}