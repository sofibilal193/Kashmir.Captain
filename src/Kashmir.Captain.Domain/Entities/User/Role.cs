using Microsoft.AspNetCore.Identity;
namespace Kashmir.Captain.Domain.Entities
{
	public class Role : IdentityRole<int>
	{
		public Role(string roleName) : base(roleName)
		{
			Name = roleName;
		}
		public Role()
		{
		}
	}
}
