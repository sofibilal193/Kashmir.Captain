using Kashmir.Captain.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Kashmir.Captain.Application.Identity
{
	public class ResetUserPasswordCommandHandler : IRequestHandler<ResetUserPasswordCommand, string>
	{
		private readonly UserManager<User> _userManager;

		public ResetUserPasswordCommandHandler(UserManager<User> userManager)
		{
			_userManager = userManager;
		}

		public async Task<string> Handle(ResetUserPasswordCommand request, CancellationToken cancellationToken)
		{
			var user = await _userManager.FindByEmailAsync(request.Email);
			if (user == null)
				return "Invalid request.";

			var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
			if (result.Succeeded)
				return "Password has been reset successfully.";

			return "BadRequest(ModelState)";
		}
	}
}