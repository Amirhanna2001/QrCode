namespace LimitlessCareDrPortal.DB;

public static class Extensions
{
	public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
	{
		foreach (var element in source)
		{
			action(element);
		}

		return source;
	}

	public static DateTime GetDateTime()
	{
		return DateTime.UtcNow.AddHours(3);
	}
}
