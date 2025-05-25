using Kashmir.Captain.Application.Extensions;
using Kashmir.Captain.Domain.Entities;

namespace Kashmir.Captain.Application.Identity
{
	public interface IUserRepository
	{
		Task<PagedList<User>> GetUsersAsync(int page, int pageSize, string? search, string sortExpression, CancellationToken cancellationToken);
	}
}