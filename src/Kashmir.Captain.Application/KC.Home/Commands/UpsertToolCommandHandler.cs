using Kashmir.Captain.Common.Extensions;
using Kashmir.Captain.Domain.Entities;
using KC.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kashmir.Captain.Application.Home
{
	public class UpsertToolCommandHandler : IRequestHandler<UpsertToolCommand, int>
	{
		private readonly KcDbContext _contex;
		public UpsertToolCommandHandler(KcDbContext contex)
		{
			_contex = contex;
		}

		public async Task<int> Handle(UpsertToolCommand request, CancellationToken cancellationToken)
		{
			Tool? tool = null;
			if (request.Id.HasValue)
			{
				tool = await _contex.Tools.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken) ?? throw new NotFoundException(nameof(Tool), request.Id);
				_contex.Update(tool);
			}
			else
			{
				tool = new Tool(request.Name, request.Brand, request.Description);
				_contex.Add(tool);
			}

			await _contex.SaveChangesAsync(cancellationToken);

			return tool.Id;
		}
	}
}