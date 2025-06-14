using MediatR;

namespace Kashmir.Captain.Application.Auth
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterResponse>
    {
        public async Task<RegisterResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            // Your registration logic here
            return new RegisterResponse(true, "Test", 1);
        }
    }
}
