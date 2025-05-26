using Kashmir.Captain.Common.Entities;
using MediatR;

namespace Kashmir.Captain.Application.Identity
{
	public readonly record struct UpsertRoleCommand(int UserId, RoleType Role, bool AssignRole) : IRequest<string>;
}