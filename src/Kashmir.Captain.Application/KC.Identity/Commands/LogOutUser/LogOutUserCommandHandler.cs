using MediatR;
using Microsoft.AspNetCore.Identity;
using Kashmir.Captain.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace Kashmir.Captain.Application.Identity
{
	public class LogOutUserCommandHandler : IRequestHandler<LogOutUserCommand, bool>
	{
		private readonly UserManager<User> _userManager;
		private readonly IConfiguration _configuration;
		private readonly SignInManager<User> _signInManager;

		public LogOutUserCommandHandler(UserManager<User> userManager,
										SignInManager<User> signInManager, IConfiguration configuration)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_configuration = configuration;
		}

		public async Task<bool> Handle(LogOutUserCommand request, CancellationToken cancellationToken)
		{
			try
			{
				await _signInManager.SignOutAsync();
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
