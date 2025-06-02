using Kashmir.Captain.Domain.Common;

namespace Kashmir.Captain.Domain.Entities
{
	public class Machine : SqlEntity
	{
		public string? Name { get; private set; }
		public string? Brand { get; private set; }
		public string? Description { get; private set; }
		public string? Version { get; private set; }

		public Machine() { }
	}
}