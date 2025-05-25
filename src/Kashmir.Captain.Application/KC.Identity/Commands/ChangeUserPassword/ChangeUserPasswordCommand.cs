using FluentValidation;
using MediatR;
using System.Text.Json.Serialization;

namespace Kashmir.Captain.Application.Identity
{
	public record ChangeUserPasswordCommand : IRequest<string>
	{
		[JsonIgnore]
		public int UserId { get; private set; }
		public string CurrentPassword { get; init; } = "";
		public string NewPassword { get; init; } = "";
		public string ConfirmPassword { get; init; } = "";

		public void setId(int userId)
		{
			UserId = userId;
		}
	}

	public class ChangeUserPasswordCommandValidator : AbstractValidator<ChangeUserPasswordCommand>
	{
		public ChangeUserPasswordCommandValidator()
		{
			RuleFor(user => user.CurrentPassword)
			.NotEmpty().WithMessage("Password is required.")
			.Length(8, 20).WithMessage("Password must be between 8 and 20 characters.");

			RuleFor(user => user.NewPassword)
			.NotEmpty().WithMessage("Password is required.")
			.Length(8, 20).WithMessage("Password must be between 8 and 20 characters.");

			RuleFor(user => user.ConfirmPassword)
			.NotEmpty().WithMessage("Confirm Password is required.")
			.Equal(user => user.NewPassword).WithMessage("Passwords do not match.");
		}
	}

}