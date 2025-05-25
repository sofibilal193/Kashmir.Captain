using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Kashmir.Captain.Domain.Entities
{
	public class User : IdentityUser<int>
	{
		[Key]
		public override int Id { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public override string? PhoneNumber { get; set; }
		public byte[] ProfilePhoto { get; set; } = [];

		public User() { }

		public User(string email, string? firstName, string? lastName, string? phoneNumber, byte[] profilePhoto)
		{
			UserName = email;
			Email = email;
			FirstName = firstName;
			LastName = lastName;
			PhoneNumber = phoneNumber;
			ProfilePhoto = profilePhoto;
		}

		public void Update(string? firstName, string? lastName, string? phoneNumber, byte[] profilePhoto)
		{
			FirstName = firstName;
			LastName = lastName;
			PhoneNumber = phoneNumber;
			ProfilePhoto = profilePhoto;
		}
	}
}