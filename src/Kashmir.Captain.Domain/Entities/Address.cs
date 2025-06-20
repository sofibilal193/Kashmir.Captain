using System.ComponentModel.DataAnnotations;

namespace Kashmir.Captain.Domain.Entities
{
	public class Address
	{
		[Key]
		public int Id { get; set; }
		public AddressType AddressType { get; set; }
		public string Name { get; set; } = "";
		public string Address1 { get; set; } = "";
		public string Address2 { get; set; } = "";
		public string City { get; set; } = "";
		public string State { get; set; } = "";
		public string Country { get; set; } = "";
		public string ZipCode { get; set; } = "";

		public Address() { }
	}
}