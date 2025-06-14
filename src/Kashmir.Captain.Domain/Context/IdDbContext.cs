using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Kashmir.Captain.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Kashmir.Captain.Domain.Interfaces;

namespace Kashmir.Captain.Domain
{
	public class IdDbContext : IdentityDbContext<User, Role, int>
	{
		private readonly string _idSchema = "id";

		public IdDbContext(DbContextOptions<IdDbContext> options, IConfiguration configuration)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<User>(entity =>
			{
				entity.ToTable("Users", _idSchema);
				entity.OwnsMany(x => x.Addresses, address =>
						{
							address.Property(a => a.Id).HasColumnOrder(0);
						});
			});

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

