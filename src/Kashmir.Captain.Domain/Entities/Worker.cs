
using Kashmir.Captain.Domain.Common;

namespace Kashmir.Captain.Domain.Entities
{
	public class Worker : SqlEntity
	{
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? PhoneNumber { get; set; }

		public Worker() { }
	}
}