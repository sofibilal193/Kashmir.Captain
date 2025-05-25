using MediatR;

namespace Kashmir.Captain.Application.Identity
{
	public class ForgotUserPasswordCommand : IRequest<string>
	{
		public string Email { get; init; } = "";
	}
}