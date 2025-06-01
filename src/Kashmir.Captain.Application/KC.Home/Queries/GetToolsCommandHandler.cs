using MediatR;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Kashmir.Captain.Application.Identity;
using KC.Domain;
using KC.Domain.Context;

namespace Kashmir.Captain.Application.Home
{
	public class GetToolsCommandHandler : IRequestHandler<GetToolsQuery, List<ToolDto>>
	{
		private readonly InventoryDbContext _context;
		private readonly IMapper _mapper;

		public GetToolsCommandHandler(InventoryDbContext context, IMapper mapper)
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