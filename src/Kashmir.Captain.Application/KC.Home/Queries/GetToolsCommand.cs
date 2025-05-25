using Kashmir.Captain.Application.Identity;
using MediatR;

namespace Kashmir.Captain.Application.Home
{
	public readonly record struct GetToolsQuery() : IRequest<List<ToolDto>>;

}