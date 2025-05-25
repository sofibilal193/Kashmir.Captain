using Kashmir.Captain.Domain.Common;

namespace Kashmir.Captain.Domain.Entities
{
	public class Tool : SqlEntity
	{
		public string? Name { get; set; }
		public string? Brand { get; set; }
		public string? Description { get; set; }
		public Tool(string name, string? brand, string? description)
		{
			Name = name;
			Brand = brand;
			Description = description;
		}
	}
}