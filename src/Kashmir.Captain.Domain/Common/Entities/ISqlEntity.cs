namespace Kashmir.Captain.Domain.Common
{
	public interface ISqlEntity
	{
		int Id { get; set; }
		byte[]? Timestamp { get; set; }
		dynamic GetId();
		bool IsTransient();
	}

}