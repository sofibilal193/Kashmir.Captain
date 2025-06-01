using System.Text.Json.Serialization;

namespace Kashmir.Captain.Domain.Common
{
	public abstract class BaseEntity : BaseEntityLogProps
	{

		[JsonIgnore]
		private int? _requestedHashCode;

		private bool _isTest;
		public bool IsTest => _isTest;

		private bool _isDisabled;
		public bool IsDisabled => _isDisabled;

		#region Constructors

		protected BaseEntity()
		{
		}

		#endregion
		public virtual void Disable()
		{
			_isDisabled = true;
		}

		public virtual void Disable<T>()
			where T : BaseEntity
		{
			_isDisabled = true;
		}

		public virtual void Enable()
		{
			_isDisabled = false;
		}

		public virtual void Enable<T>()
			where T : BaseEntity
		{
			_isDisabled = false;
		}

		public virtual void ToggleTest(bool isTest)
		{
			_isTest = isTest;
		}

		public abstract dynamic GetId();

		public abstract bool IsTransient();

		public override bool Equals(object? obj)
		{
			if (obj is not BaseEntity)
				return false;

			if (Object.ReferenceEquals(this, obj))
				return true;

			if (this.GetType() != obj.GetType())
				return false;

			BaseEntity item = (BaseEntity)obj;

			if (item.IsTransient() || this.IsTransient())
				return false;
			else
				return item.GetId() == this.GetId();
		}

#pragma warning disable S2328 // Refactor 'GetHashCode' to not reference mutable fields.
		public override int GetHashCode()
		{
			if (!IsTransient())
			{
				if (!_requestedHashCode.HasValue)
					_requestedHashCode = this.GetId().GetHashCode() ^ 31; // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)

				return _requestedHashCode.Value;
			}
			else
			{
				return GetHashCode();
			}
		}
#pragma warning restore S2328 // Refactor 'GetHashCode' to not reference mutable fields.

#pragma warning disable S3875 // Remove this overload of 'operator =='.
		public static bool operator ==(BaseEntity left, BaseEntity right)
		{
			if (Object.Equals(left, null))
				return Object.Equals(right, null);
			else
				return left.Equals(right);
		}
#pragma warning restore S3875 // Remove this overload of 'operator =='.

		public static bool operator !=(BaseEntity left, BaseEntity right)
		{
			return !(left == right);
		}
	}

	public abstract class BaseEntityLogProps
	{
		public DateTime? CreateDateTimeUtc { get; set; }

		public int? CreateUserId { get; set; }

		public string? CreateUserName { get; set; }

		public string? CreateSource { get; set; }

		public DateTime? ModifyDateTimeUtc { get; set; }

		public int? ModifyUserId { get; set; }

		public string? ModifyUserName { get; set; }

		public string? ModifySource { get; set; }
	}
}
