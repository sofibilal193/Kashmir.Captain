using Kashmir.Captain.Domain.Interfaces;
using MediatR;

namespace Kashmir.Captain.Application.Auth
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterResponse>
    {
        private readonly IIdentityDbContext _identityDbContext;
        public RegisterUserCommandHandler(IIdentityDbContext identityDbContext)
        {
            _identityDbContext = identityDbContext;
        }
        public async Task<RegisterResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            _ = _identityDbContext.Users;
            // Your registration logic here
            return new RegisterResponse(true, "Test", 1);
        }
    }
}
