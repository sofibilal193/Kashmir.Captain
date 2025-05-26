using MediatR;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using KC.Domain.Entities;
using Kashmir.Captain.Application.Identity;

namespace Kashmir.Captain.Application.Home
{
	public class GetToolsCommandHandler : IRequestHandler<GetToolsQuery, List<ToolDto>>
	{
		private readonly KcDbContext _context;
		private readonly IMapper _mapper;

		public GetToolsCommandHandler(KcDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}
		public async Task<List<ToolDto>> Handle(GetToolsQuery request, CancellationToken cancellationToken)
		{
			var tools = await _context.Tools.ToListAsync(cancellationToken);
			return _mapper.Map<List<ToolDto>>(tools);
		}
	}
}