using FluentValidation;
using MediatR;
using System.Text.Json.Serialization;

namespace Kashmir.Captain.Application.Identity
{
	public record ChangeUserEmailCommand : IRequest<string>
	{
		[JsonIgnore]
		public int UserId { get; private set; }
		public string NewEmail { get; init; } = "";
		public string CurrentPassword { get; init; } = "";

		public void setId(int userId)
		{
			UserId = userId;
		}
	}

	public class ChangeUserEmailCommandValidator : AbstractValidator<ChangeUserEmailCommand>
	{
		public ChangeUserEmailCommandValidator()
		{
			RuleFor(user => user.NewEmail)
			.NotEmpty().WithMessage("Email is required.");

			RuleFor(user => user.CurrentPassword)
			.NotEmpty().WithMessage("Password is required.")
			.Length(8, 20).WithMessage("Password must be between 8 and 20 characters.");
		}
	}
}