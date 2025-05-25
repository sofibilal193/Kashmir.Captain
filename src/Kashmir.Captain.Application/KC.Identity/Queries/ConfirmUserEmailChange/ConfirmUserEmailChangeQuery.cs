using MediatR;

namespace Kashmir.Captain.Application.Identity
{
	public readonly record struct ConfirmUserEmailChangeQuery(int UserId, string NewEmail, string Token) : IRequest<string>;
}