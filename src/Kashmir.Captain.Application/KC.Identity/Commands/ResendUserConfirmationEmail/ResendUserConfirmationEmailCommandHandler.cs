using Kashmir.Captain.Application.Extensions;
using Kashmir.Captain.Application.Services;
using Kashmir.Captain.Domain;
using Kashmir.Captain.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Kashmir.Captain.Application.Identity
{
	public class ResendUserConfirmationEmailCommandHandler : IRequestHandler<ResendUserConfirmationEmailCommand, string>
	{
		private readonly UserManager<User> _userManager;
		private readonly IUrlHelperService _urlHelperService;
		private readonly IEmailService _emailService;

		public ResendUserConfirmationEmailCommandHandler(UserManager<User> userManager, IUrlHelperService urlHelperService, IEmailService emailService)
		{
			_userManager = userManager;
			_urlHelperService = urlHelperService;
			_emailService = emailService;
		}
		public async Task<string> Handle(ResendUserConfirmationEmailCommand request, CancellationToken cancellationToken)
		{
			var user = await _userManager.FindByIdAsync($"{request.UserId}")
			?? throw new NotFoundException(nameof(User), request.UserId);
			if (string.IsNullOrEmpty(user.Email))
			{
				throw new InvalidOperationException("User's email is not set.");
			}

			if (user == null || await _userManager.IsEmailConfirmedAsync(user))
			{
				return "If the email is registered and not confirmed, a confirmation link has been sent.";
			}

			var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
			var confirmEmailLink = _urlHelperService.GenerateUrl("ConfirmEmail", "Account", new { token, email = user.Email });

			var mail = new EmailTemplate()
			{
				To = user.Email,
				Subject = $"Confirm Email Kashmir Captain",
			};
			mail.GetResendUserConfirmationEmailBody(confirmEmailLink);

			await _emailService.SendEmailAsync(mail);

			return "If the email is registered and not confirmed, a confirmation link has been sent.";

		}
	}
}