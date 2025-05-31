using Kashmir.Captain.Common.Extensions;
using Kashmir.Captain.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Kashmir.Captain.Application.Identity
{
	public class UserRepository : IUserRepository
	{
		private readonly UserManager<User> _userManager;
		private readonly RoleManager<Role> _roleManager;
		public UserRepository(UserManager<User> userManager, RoleManager<Role> roleManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
		}
		public async Task<PagedList<User>> GetUsersAsync(int page, int pageSize, string? search, string sortExpression, CancellationToken cancellationToken)
		{
			_ = _roleManager;
			var query = _userManager.Users;

			if (!string.IsNullOrEmpty(search))
			{
				query = query.Where(d => (d.FirstName ?? "").Contains(search) || (d.LastName ?? "").Contains(search)
										|| (d.Email ?? "").Contains(search) || (d.PhoneNumber ?? "").Contains(search));
			}

			return await query.OrderBy(sortExpression).ToPagedListAsync(page, pageSize, cancellationToken);
		}
	}
}