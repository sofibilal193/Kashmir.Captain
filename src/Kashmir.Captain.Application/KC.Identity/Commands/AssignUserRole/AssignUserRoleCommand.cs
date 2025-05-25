using Kashmir.Captain.Domain.Entities;
using KC.Infrastructure.Persistance.Entities;
using MediatR;
using System.Text.Json.Serialization;

namespace Kashmir.Captain.Application.Identity
{
	public readonly record struct UpsertRoleCommand(int UserId, RoleType Role, bool AssignRole) : IRequest<string>;
}