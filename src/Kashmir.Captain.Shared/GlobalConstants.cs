using Kashmir.Captain.Shared.Entities;

namespace Kashmir.Captain.Shared
{
	public static class GlobalConstants
	{
		public const string ProjectName = "Kashmir Captain";

		public static readonly List<string> Roles =
			Enum.GetValues(typeof(RoleType))
				.Cast<RoleType>()
				.Select(v => v.ToString())
				.ToList();

		public const string IdSchema = "id";
		public const string UtilsSchema = "utils";

		public const int MaxPageSize = 50;
		public const int DefaultPageSize = 10;

		public const string DateFormat = "yyyy-MM-dd";
		public const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
	}
}
