using Kashmir.Captain.Domain.Entities;
using KC.Infrastructure.Persistance.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kashmir.Captain.Domain
{
	public class WorkerConfiguration : IEntityTypeConfiguration<Worker>
	{
		private readonly string _schema;

		public WorkerConfiguration(string schema)
		{
			_schema = schema;
		}

		public void Configure(EntityTypeBuilder<Worker> builder)
		{
			builder.ToTable("Workers", _schema);

			builder.HasKey(x => x.Id);

			builder.Property(e => e.FirstName)
				.HasMaxLength(20)
				.IsRequired();

			builder.Property(e => e.LastName)
				.HasMaxLength(10)
				.IsRequired();

			builder.Property(e => e.PhoneNumber)
				.HasMaxLength(10)
				.IsRequired();
		}
	}
}