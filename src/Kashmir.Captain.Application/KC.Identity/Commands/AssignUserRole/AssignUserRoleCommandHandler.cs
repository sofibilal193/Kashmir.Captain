using Microsoft.AspNetCore.Identity;
using MediatR;
using Kashmir.Captain.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Kashmir.Captain.Common.Extensions;
using Kashmir.Captain.Common.Entities;
using KC.Domain;

namespace Kashmir.Captain.Application.Identity
{
	public class UpsertRoleCommandHandler : IRequestHandler<UpsertRoleCommand, string>
	{
		private readonly UserManager<User> _userManager;
		private readonly RoleManager<Role> _roleManager;
		private readonly IdDbContext _context;

		public UpsertRoleCommandHandler(UserManager<User> userManager, RoleManager<Role> roleManager, IdDbContext context)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_context = context;
		}

		public async Task<string> Handle(UpsertRoleCommand request, CancellationToken cancellationToken)
		{
			if (string.IsNullOrEmpty(request.UserId.ToString()) && !Enum.IsDefined(typeof(RoleType), request.Role))
			{
				throw new BadHttpRequestException("The request data is invalid.");
			}
			var roleName = request.Role.ToString();

			if (!await _roleManager.RoleExistsAsync(roleName))
			{
				throw new NotFoundException(nameof(Role), roleName);
			}

			var user = await _userManager.FindByIdAsync(request.UserId.ToString())
					   ?? throw new NotFoundException(nameof(User), request.UserId);

			// var UserRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

			if (request.AssignRole == true) //Add Role
			{
				return await AddRoleToUser(user, roleName);
			}
			else //Remove Role
			{
				return await RemoveRoleOfUser(user, roleName);
			}
			// else //Update Role
			// {
			// 	return UserRole is not null ? await UpdateRoleOfUser(user, roleName, UserRole) : "User not in any Role";
			// }
		}

		private async Task<string> RemoveRoleOfUser(User user, string roleName)
		{
			var removeResult = await _userManager.RemoveFromRoleAsync(user, roleName);
			return removeResult.Succeeded ? "Role removed successfully." : $"{removeResult.Errors.First().Description}";
		}

		private async Task<string> AddRoleToUser(User user, string roleName)
		{
			var addResult = await _userManager.AddToRoleAsync(user, roleName);
			return addResult.Succeeded ? "Role assigned successfully." : $"{addResult.Errors.First().Description}";
		}

		// private async Task<string> UpdateRoleOfUser(User user, string roleName, string UserRole)
		// {
		// 	var removeResult = await _userManager.RemoveFromRoleAsync(user, UserRole);
		// 	if (removeResult.Succeeded)
		// 	{
		// 		var addResult = await _userManager.AddToRoleAsync(user, roleName);
		// 		return addResult.Succeeded ? "Role Updated successfully." : $"{addResult.Errors.First().Description}.";
		// 	}
		// 	return $"{removeResult.Errors.First().Description}";
		// }
	}
}