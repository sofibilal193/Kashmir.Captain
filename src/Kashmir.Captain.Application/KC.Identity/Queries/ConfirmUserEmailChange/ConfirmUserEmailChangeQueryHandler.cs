using Kashmir.Captain.Domain.Entities;
using KC.Infrastructure.Persistance.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Kashmir.Captain.Application.Identity
{
	public class ConfirmUserEmailChangeQueryHandler : IRequestHandler<ConfirmUserEmailChangeQuery, string>
	{
		private readonly UserManager<User> _userManager;

		public ConfirmUserEmailChangeQueryHandler(UserManager<User> userManager)
		{
			_userManager = userManager;
		}
		public async Task<string> Handle(ConfirmUserEmailChangeQuery request, CancellationToken cancellationToken)
		{

			if (string.IsNullOrWhiteSpace(request.Token) || string.IsNullOrWhiteSpace($"{request.UserId}"))
				return "Invalid confirmation link.";

			var user = await _userManager.FindByIdAsync($"{request.UserId}");
			if (user == null)
				return "Invalid confirmation link.";

			var result = await _userManager.ChangeEmailAsync(user, $"{request.NewEmail}", request.Token);
			if (result.Succeeded)
				return "Email address changed successfully.";

			return "Email change failed.";
		}
	}
}