#nullable enable
using System.Collections.Generic;
using System.Linq;

namespace NecroMacro.Core.Extensions
{
	public static class EnumerableExtension
	{
		public static bool IsFilled<T>(this IEnumerable<T>? collection)
		{
			return collection?.Any() == true;
		}

		public static bool IsEmpty<T>(this IEnumerable<T>? collection)
		{
			return collection == null || !collection.Any();
		}
	}
}
