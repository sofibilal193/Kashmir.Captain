using System.Reflection.PortableExecutable;
using Kashmir.Captain.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Machine = Kashmir.Captain.Domain.Entities.Machine;

namespace Kashmir.Captain.Domain
{
	public class MachineConfiguration : IEntityTypeConfiguration<Machine>
	{
		private readonly string _schema;

		public MachineConfiguration(string schema)
		{
			_schema = schema;
		}

		public void Configure(EntityTypeBuilder<Machine> builder)
		{
			builder.ToTable("Machines", _schema);

			builder.HasKey(x => x.Id);

			builder.Property(e => e.Name)
				.HasMaxLength(20)
				.IsRequired();

			builder.Property(e => e.Brand)
				.HasMaxLength(10)
				.IsRequired();

			builder.Property(e => e.Description)
				.HasMaxLength(10)
				.IsRequired();
			builder.Property(e => e.Version)
				.HasMaxLength(10)
				.IsRequired();
		}
	}
}