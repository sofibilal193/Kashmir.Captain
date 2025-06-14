using FluentValidation;
using MediatR;

namespace Kashmir.Captain.Application.Auth
{
    public record LoginUserCommand : IRequest<LoginResponse>
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public bool RememberMe { get; set; }
    }

    public class LoginUserModelValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserModelValidator()
        {
            RuleFor(user => user.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(user => user.Password)
            .NotEmpty().WithMessage("Password is required.")
            .Length(8, 20).WithMessage("Password must be between 8 and 20 characters.");
        }
    }
}