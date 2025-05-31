using FluentValidation;
using MediatR;
using Kashmir.Captain.Common.Extensions;

namespace Kashmir.Captain.Application.Identity
{
	public record LogOutUserCommand() : IRequest<bool>;
}