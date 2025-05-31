using Kashmir.Captain.Common.Extensions;
using Kashmir.Captain.Domain;
using Kashmir.Captain.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Kashmir.Captain.Common.Entities;
using Kashmir.Captain.Common.Validations;
using Kashmir.Captain.Common;
using Kashmir.Captain.Comman;

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
			var user = new User(request.Email, request.FirstName, request.LastName, request.PhoneNumber);

			var createdResult = await _userManager.CreateAsync(user, request.Password);
			if (createdResult.Succeeded)
			{
				var addRoleResult = await _userManager.AddToRoleAsync(user, nameof(RoleType.User));
				if (addRoleResult.Succeeded)
				{
					var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

					await SendEmail(user, token);

					return new ApiResponse<string>(true, ValidationMessages.User_Registered_Success);
				}
				else // RollBack
				{
					await _userManager.DeleteAsync(user);
					return new ApiResponse<string>(false, addRoleResult.Errors.First().Description.ToString());

				}
			}
			return new ApiResponse<string>(false, createdResult.Errors.First().Description.ToString());
		}

		private async Task SendEmail(User user, string token)
		{
			var confirmEmailLink = _urlHelperService.GenerateUrl("ConfirmEmail", "Account", new { token, userId = user.Id });

			var mail = new Comman.EmailTemplate()
			{
				To = user.Email!,
				Subject = ValidationMessages.Confirm_Email_Address,
			};
			mail.GetEmailRegistrationBody(user.FirstName, user.LastName, confirmEmailLink);

			await _emailService.SendEmailAsync(mail);
		}
	}
}
