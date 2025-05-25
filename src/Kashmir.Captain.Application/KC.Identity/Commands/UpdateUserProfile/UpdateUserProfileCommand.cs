using FluentValidation;
using MediatR;
using System.Text.Json.Serialization;

namespace Kashmir.Captain.Application.Identity
{
	public record UpdateUserProfileCommand : IRequest<string>
	{
		[JsonIgnore]
		public int UserId { get; private set; }
		public string FirstName { get; init; } = "";
		public string LastName { get; init; } = "";
		public string PhoneNumber { get; init; } = "";

		public void setId(int userId)
		{
			UserId = userId;
		}
	}

	public class UpdateUserProfileCommandValidator : AbstractValidator<UpdateUserProfileCommand>
	{
		public UpdateUserProfileCommandValidator()
		{
			RuleFor(user => user.FirstName)
			.NotEmpty().WithMessage("First Name is required.")
			.Matches(@"^[a-zA-Z\s]+$").WithMessage("First Name must contain only letters and spaces.")
			.Length(1, 20).WithMessage("First Name must be between 1 and 50 characters long.");

			RuleFor(user => user.LastName)
			.NotEmpty().WithMessage("Last Name is required.")
			.Matches(@"^[a-zA-Z\s]+$").WithMessage("Last Name must contain only letters.")
			.Length(1, 10).WithMessage("Last Name must be between 1 and 10 characters long.");

			RuleFor(user => user.PhoneNumber)
			.NotEmpty().WithMessage("Phone Number is required.")
			.Length(10).WithMessage("Phone Number must be exactly 10 digits long.")
			.Matches(@"^\d{10}$").WithMessage("Phone Number must contain only numeric digits.");
		}
	}

}