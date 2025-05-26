using KC.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using MediatR;
using Kashmir.Captain.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Kashmir.Captain.Common.Extensions;

namespace Kashmir.Captain.Application.Identity
{
	public class DeleteUserRoleCommandHandler : IRequestHandler<DeleteUserRoleCommand, string>
	{
		private readonly UserManager<User> _userManager;
		private readonly RoleManager<Role> _roleManager;

		public DeleteUserRoleCommandHandler(UserManager<User> userManager, RoleManager<Role> roleManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
		}

		public async Task<string> Handle(DeleteUserRoleCommand request, CancellationToken cancellationToken)
		{
			if (string.IsNullOrEmpty(request.UserId.ToString()))
			{
				throw new BadHttpRequestException("The request data is invalid.");
			}

			var user = await _userManager.FindByIdAsync(request.UserId.ToString())
					   ?? throw new NotFoundException(nameof(User), request.UserId);

			var UserRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault()
					?? throw new NotFoundException(nameof(Role), request.UserId);

			var removeResult = await _userManager.RemoveFromRoleAsync(user, UserRole);

			return removeResult.Succeeded ? "Role Removed Successfully" : $"{removeResult.Errors.First().Description}";
		}
	}
}