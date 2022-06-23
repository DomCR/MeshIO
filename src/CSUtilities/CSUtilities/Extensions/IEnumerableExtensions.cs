using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSUtilities.Extensions
{
	/// <summary>
	/// Estensions for <see cref="IEnumerable{T}"/>
	/// </summary>
	internal static class IEnumerableExtensions
	{
		/// <summary>
		/// Return true if the collection is empty.
		/// </summary>
		/// <param name="enumerable"></param>
		/// <returns></returns>
		public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
		{
			return enumerable.GetEnumerator() == null;
		}

		/// <summary>
		/// Transforms an enumerable into a Queue.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="enumerable"></param>
		/// <returns></returns>
		public static Queue<T> ToQueue<T>(this IEnumerable<T> enumerable)
		{
			return new Queue<T>(enumerable);
		}

		public static IEnumerable<T> RemoveLastEquals<T>(this IEnumerable<T> enumerable, T element)
		{
			List<T> lst = new List<T>(enumerable);
			while (lst.Last().Equals(element))
			{
				lst.RemoveAt(lst.Count - 1);
			}

			return lst;
		}
	}
}
