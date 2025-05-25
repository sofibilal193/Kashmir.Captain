using FluentValidation;
using MediatR;
using System.Text.Json.Serialization;

namespace Kashmir.Captain.Application.Identity
{
	public record ResetUserPasswordCommand : IRequest<string>
	{
		public string Email { get; init; } = "";
		public string Token { get; init; } = "";
		public string NewPassword { get; init; } = "";
		public string ConfirmPassword { get; init; } = "";
	}

	public class ResetUserPasswordCommandValidator : AbstractValidator<ResetUserPasswordCommand>
	{
		public ResetUserPasswordCommandValidator()
		{
			RuleFor(user => user.Email)
			.NotEmpty().WithMessage("Email is required.")
			.EmailAddress().WithMessage("Invalid email format.");

			RuleFor(user => user.Token)
			.NotEmpty().WithMessage("Token is required.");

			RuleFor(user => user.NewPassword)
			.NotEmpty().WithMessage("Password is required.")
			.Length(8, 20).WithMessage("Password must be between 8 and 20 characters.");

			RuleFor(user => user.ConfirmPassword)
			.NotEmpty().WithMessage("Confirm Password is required.")
			.Equal(user => user.NewPassword).WithMessage("Passwords do not match.");
		}
	}

}