using Microsoft.AspNetCore.Identity;
namespace Kashmir.Captain.Domain.Entities
{
	public class Role : IdentityRole<int>
	{
		public Role(string roleName)
		{
			Name = roleName;
		}
		public Role()
		{
		}
	}
}
