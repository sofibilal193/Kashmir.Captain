using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Logging;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using Kashmir.Captain.Common;
using Kashmir.Captain.Common.Extensions;

namespace Kashmir.Captain.Domain.Common
{
    public abstract class BaseSqlDbContext<TContext> : BaseDbContext<TContext>
        where TContext : DbContext
    {
        protected BaseSqlDbContext(DbContextOptions<TContext> options, ILogger<BaseSqlDbContext<TContext>> logger, ICryptoProvider cryptoProvider, ICurrentUser currentUser, IDateTime dateTime, IMediator mediator)
            : base(options, logger, cryptoProvider, currentUser, dateTime, mediator)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            Logger.LogInformation("BaseSqlDbContext:Assembly1: {Assembly}", GetType().Assembly);
            Logger.LogInformation("BaseSqlDbContext:Assembly2: {Assembly}", Assembly.GetExecutingAssembly());

            // apply configurations for all types that inherit from SqlEntity
            modelBuilder.ApplyConfigurationsFromAssembly(
                GetType().Assembly,
                t => t.GetInterfaces().ToList().Exists(i =>
                            i.IsGenericType &&
                            i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>) &&
                            typeof(SqlEntity).IsAssignableFrom(i.GenericTypeArguments[0])));

            // InitEncryptionValueConverter(modelBuilder);
            modelBuilder.HasDefaultSchema(schema: DefaultSchema);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            // optionsBuilder.ReplaceService<IMigrationsSqlGenerator, CustomMigrationsSqlGenerator>();
            // optionsBuilder.ReplaceService<IRelationalAnnotationProvider, CustomAnnotationProvider>();
        }
    }
}
