using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Kashmir.Captain.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Kashmir.Captain.Domain;

namespace KC.Domain.Entities
{
	public class KcDbContext : IdentityDbContext<User, Role, int>
	{
		private readonly string _IdSchema;
		private readonly string _UtilSchema;

		public KcDbContext(DbContextOptions<KcDbContext> options, IConfiguration configuration)
			: base(options)
		{
			_IdSchema = configuration.GetValue<string>("IdentitySchema") ?? "id";
			_UtilSchema = configuration.GetValue<string>("_UtilSchema") ?? "utils";
		}

		public DbSet<Worker> Workers { get; set; }
		public DbSet<Machine> Machines { get; set; }
		public DbSet<Tool> Tools { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<User>(entity =>
			{
				entity.ToTable("Users", schema: _IdSchema);
			});
			builder.Entity<Role>(entity => entity.ToTable(name: "Roles", schema: _IdSchema));
			builder.Entity<IdentityUserRole<int>>(entity =>
   					{
						   entity.ToTable("UserRoles", schema: _IdSchema);
						   entity.HasIndex(e => e.UserId).IsUnique();
						   entity.HasKey(e => new { e.UserId, e.RoleId });
   					});
			builder.Entity<IdentityUserClaim<int>>(entity => entity.ToTable("UserClaims", schema: _IdSchema));
			builder.Entity<IdentityUserLogin<int>>(entity => entity.ToTable("UserLogins", schema: _IdSchema));
			builder.Entity<IdentityRoleClaim<int>>(entity => entity.ToTable("RoleClaims", schema: _IdSchema));
			builder.Entity<IdentityUserToken<int>>(entity => entity.ToTable("UserTokens", schema: _IdSchema));

			//Utils
			builder.ApplyConfiguration(new WorkerConfiguration(_UtilSchema));
			builder.ApplyConfiguration(new MachineConfiguration(_UtilSchema));
			builder.ApplyConfiguration(new ToolConfiguration(_UtilSchema));
		}
	}
}

