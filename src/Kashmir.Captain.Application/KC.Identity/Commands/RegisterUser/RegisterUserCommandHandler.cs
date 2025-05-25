using Kashmir.Captain.Application.Extensions;
using Kashmir.Captain.Application.Services;
using Kashmir.Captain.Domain;
using Kashmir.Captain.Domain.Entities;
using KC.Infrastructure.Persistance.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Kashmir.Captain.Application.Identity
{
	public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ApiResponse<string>>
	{
		private readonly UserManager<User> _userManager;
		private readonly IEmailService _emailService;
		private readonly IUrlHelperService _urlHelperService;

		public RegisterUserCommandHandler(UserManager<User> userManager, IEmailService emailService, IUrlHelperService urlHelperService)
		{
			_userManager = userManager;
			_emailService = emailService;
			_urlHelperService = urlHelperService;
		}

		public async Task<ApiResponse<string>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
		{
			byte[] imageBytes = Convert.FromBase64String(request.ProfilePhoto);
			var user = new User(request.Email, request.FirstName, request.LastName, request.PhoneNumber, imageBytes);

			var createdResult = await _userManager.CreateAsync(user, request.Password);
			if (createdResult.Succeeded)
			{
				var addRoleResult = await _userManager.AddToRoleAsync(user, nameof(RoleType.User));
				if (addRoleResult.Succeeded)
				{
					var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
					var confirmEmailLink = _urlHelperService.GenerateUrl("ConfirmEmail", "Account", new { token, userId = user.Id });

					var mail = new EmailTemplate()
					{
						To = user.Email!,
						Subject = $"Confirm Email Kashmir Captain",
					};
					mail.GetEmailRegistrationBody(user.FirstName, user.LastName, confirmEmailLink);

					await _emailService.SendEmailAsync(mail);

					return new ApiResponse<string>(true, "User registered successfully. Please check your email to confirm your account.");
				}
				else // RollBack
				{
					await _userManager.DeleteAsync(user);
					return new ApiResponse<string>(false, addRoleResult.Errors.First().ToString());

				}
			}
			return new ApiResponse<string>(false, createdResult.Errors.First().ToString());
		}
	}
}
