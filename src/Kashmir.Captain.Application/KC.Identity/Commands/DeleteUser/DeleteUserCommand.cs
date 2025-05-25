using MediatR;
using System.Text.Json.Serialization;

namespace Kashmir.Captain.Application.Identity
{
	public class DeleteUserCommand : IRequest<string>
	{
		[JsonIgnore]
		public int? UserId { get; private set; }

		public void setId(int? userId)
		{
			UserId = userId;
		}
	}
}