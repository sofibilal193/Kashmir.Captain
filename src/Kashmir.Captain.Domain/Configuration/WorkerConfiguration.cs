using Kashmir.Captain.Domain.Common;
using Kashmir.Captain.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kashmir.Captain.Domain
{
	public class WorkerConfiguration : SqlEntityConfiguration<Worker>
	{
		private readonly string _schema;

		public WorkerConfiguration(string schema)
		{
			_schema = schema;
		}

		public override void Configure(EntityTypeBuilder<Worker> builder)
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

			builder.OwnsMany(x => x.Skills, Skills =>
			{
				Skills.ToTable("Skills", _schema);
				Skills.Property(s => s.Name).IsRequired().HasMaxLength(50);
				Skills.Property(s => s.Experience).IsRequired();
				Skills.Property(s => s.Description).HasMaxLength(200);
			});
		}
	}
}