using AutoMapper;
using Kashmir.Captain.Common.Extensions;
using Kashmir.Captain.Domain.Entities;
using KC.Domain.Entities;
using MediatR;

namespace Kashmir.Captain.Application.Identity
{
	public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, PagedList<UserDto>>
	{
		private readonly IUserRepository _data;
		private readonly IMapper _mapper;
		public GetUsersQueryHandler(IUserRepository data, IMapper mapper)
		{
			_data = data;
			_mapper = mapper;
		}
		public async Task<PagedList<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
		{
			var sortExpression = request.Sort?.TrimStart('-')?.ToLower();
			var prefix = request.Sort?.StartsWith('-') == true ? "-" : "";
			if (sortExpression == nameof(UserDto.Email).ToLower())
			{
				sortExpression = prefix + nameof(User.Email);
			}
			else if (sortExpression == nameof(UserDto.FirstName).ToLower())
			{
				sortExpression = prefix + nameof(User.FirstName);
			}
			else
			{
				sortExpression = nameof(User.Id);
			}

			var users = await _data.GetUsersAsync(request.Page, request.PageSize, request.search, sortExpression, cancellationToken);


			return _mapper.Map<PagedList<UserDto>>(users);
		}
	}
}