
using Kashmir.Captain.Domain.Common;

namespace Kashmir.Captain.Domain.Entities
{
	public class Worker : SqlEntity
	{
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? PhoneNumber { get; set; }
		public List<Skills>? Skills { get; set; }

		public Worker() { }
	}

	public class Skills
	{
		public string Name { get; private set; } = "";
		public int Experience { get; private set; }
		public string? Description { get; private set; }

		public Skills() { }
	}
}