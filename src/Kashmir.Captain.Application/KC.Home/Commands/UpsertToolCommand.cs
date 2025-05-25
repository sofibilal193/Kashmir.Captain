using FluentValidation;
using MediatR;
using System.Text.Json.Serialization;

namespace Kashmir.Captain.Application.Home
{
	public record UpsertToolCommand : IRequest<int>
	{
		[JsonIgnore]
		public int? Id { get; private set; }
		public string Name { get; init; } = "";
		public string Brand { get; init; } = "";
		public string Description { get; init; } = "";

		public void SetId(int? id)
		{
			Id = id;
		}
	}

	public class RegisterUserModelValidator : AbstractValidator<UpsertToolCommand>
	{
		public RegisterUserModelValidator()
		{
			RuleFor(user => user.Name)
			.NotEmpty().WithMessage("Name is required.");

			RuleFor(user => user.Brand)
			.NotEmpty().WithMessage("Brand is required.");

			RuleFor(user => user.Description)
			.NotEmpty().WithMessage("Phone Number is required.")
			.MaximumLength(100);
		}
	}
}