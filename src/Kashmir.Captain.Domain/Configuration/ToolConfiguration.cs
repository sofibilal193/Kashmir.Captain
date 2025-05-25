using Kashmir.Captain.Domain.Entities;
using KC.Infrastructure.Persistance.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kashmir.Captain.Domain
{
	public class ToolConfiguration : IEntityTypeConfiguration<Tool>
	{
		private readonly string _schema;

		public ToolConfiguration(string schema)
		{
			_schema = schema;
		}

		public void Configure(EntityTypeBuilder<Tool> builder)
		{
			builder.ToTable("Tools", _schema);

			builder.HasKey(x => x.Id);

			builder.Property(e => e.Name)
				.HasMaxLength(20)
				.IsRequired();

			builder.Property(e => e.Brand)
				.HasMaxLength(10)
				.IsRequired();

			builder.Property(e => e.Description)
				.HasMaxLength(1000)
				.IsRequired();
		}
	}
}