using Kashmir.Captain.Domain.Common;

namespace Kashmir.Captain.Domain.Entities
{
	public class Machine : SqlEntity
	{
		public string? Name { get; set; }
		public string? Brand { get; set; }
		public string? Description { get; set; }
		public string? Version { get; set; }

		public Machine() { }
	}
}