namespace Kashmir.Captain.Application.Identity
{
	public class UserDto
	{
		public int Id { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? Email { get; set; }
		public string? PhoneNumber { get; set; }
		public List<string>? Role { get; set; }
		public string? ProfilePhoto { get; init; }
	}
}