using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Kashmir.Captain.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Kashmir.Captain.Domain;

namespace KC.Domain
{
	public class IdDbContext : IdentityDbContext<User, Role, int>
	{
		private readonly string _idSchema;

		public IdDbContext(DbContextOptions<IdDbContext> options, IConfiguration configuration)
			: base(options)
		{
			_idSchema = configuration.GetValue<string>("IdentitySchema") ?? "id";
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<User>(entity => entity.ToTable("Users", _idSchema));
			builder.Entity<Role>(entity => entity.ToTable("Roles", _idSchema));
			builder.Entity<IdentityUserRole<int>>(entity =>
			{
				entity.ToTable("UserRoles", _idSchema);
				entity.HasKey(e => new { e.UserId, e.RoleId });
				entity.HasIndex(e => e.UserId).IsUnique();
			});
			builder.Entity<IdentityUserClaim<int>>(entity => entity.ToTable("UserClaims", _idSchema));
			builder.Entity<IdentityUserLogin<int>>(entity => entity.ToTable("UserLogins", _idSchema));
			builder.Entity<IdentityRoleClaim<int>>(entity => entity.ToTable("RoleClaims", _idSchema));
			builder.Entity<IdentityUserToken<int>>(entity => entity.ToTable("UserTokens", _idSchema));
		}
	}

}

