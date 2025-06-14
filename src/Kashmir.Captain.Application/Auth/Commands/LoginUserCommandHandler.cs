using MediatR;

namespace Kashmir.Captain.Application.Auth
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginResponse>
    {
        public async Task<LoginResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            return new LoginResponse(true, "Test", Token: "TestToken", UserId: "TestUserId");
        }
    }
}