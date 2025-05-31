using Kashmir.Captain.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Kashmir.Captain.Application.Identity
{
	public class ConfirmUserEmailQueryHandler : IRequestHandler<ConfirmUserEmailQuery, string>
	{
		private readonly UserManager<User> _userManager;

		public ConfirmUserEmailQueryHandler(UserManager<User> userManager)
		{
			_userManager = userManager;

		}
		public async Task<string> Handle(ConfirmUserEmailQuery request, CancellationToken cancellationToken)
		{
			if (string.IsNullOrWhiteSpace(request.Token) || string.IsNullOrWhiteSpace($"{request.UserId}"))
				return "Invalid confirmation link.";

			var user = await _userManager.FindByIdAsync($"{request.UserId}");
			if (user == null)
				return "Invalid confirmation link.";

			var result = await _userManager.ConfirmEmailAsync(user, request.Token);
			if (result.Succeeded)
				return "Email confirmed successfully.";

			return "Email confirmation failed.";
		}
	}
}