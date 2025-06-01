
using System.ComponentModel.DataAnnotations;

namespace Kashmir.Captain.Domain.Common
{
	public abstract class SqlEntity : BaseEntity, IAggregateRoot
	{
		[Key]
		public virtual int Id { get; }

		public virtual byte[]? Timestamp { get; }

		protected SqlEntity()
		{
		}

		public override dynamic GetId()
		{
			return Id;
		}

		public override bool IsTransient()
		{
			return Id == default;
		}
	}
}
