using MediatR;

namespace Kashmir.Captain.Application.Identity
{
	public readonly record struct GetUserQuery(int UserId) : IRequest<UserDto>;
}