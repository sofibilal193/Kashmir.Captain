using Kashmir.Captain.Application.Extensions;
using MediatR;

namespace Kashmir.Captain.Application.Identity
{
	/// <summary>
	/// Get All Users
	/// </summary>
	/// <param name="Page">Page Number</param>
	/// <param name="PageSize">Page Size</param>
	/// <param name="Sort">Sort on Email or First Name</param>
	/// <param name="search"></param> <summary>
	/// 
	/// </summary>
	public readonly record struct GetUsersQuery(int Page, int PageSize, string? Sort, string? search) : IRequest<PagedList<UserDto>>;
}