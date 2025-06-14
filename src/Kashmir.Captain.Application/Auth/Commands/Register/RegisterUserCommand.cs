using FluentValidation;
using MediatR;
using System.Text.Json.Serialization;

namespace Kashmir.Captain.Application.Auth
{
    public class RegisterUserCommand : IRequest<RegisterResponse>
    {
        [JsonIgnore]
        public int? Id { get; private set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string ConfirmPassword { get; set; } = "";
        public string ProfilePhoto { get; set; } = "";

        public void SetId(int? id)
        {
            Id = id;
        }
    }

    public class RegisterUserModelValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserModelValidator()
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

            RuleFor(user => user.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(user => user.Password)
            .NotEmpty().WithMessage("Password is required.")
            .Length(8, 20).WithMessage("Password must be between 8 and 20 characters.");

            RuleFor(user => user.ConfirmPassword)
            .NotEmpty().WithMessage("Confirm Password is required.")
            .Equal(user => user.Password).WithMessage("Passwords do not match.");
        }
    }
}