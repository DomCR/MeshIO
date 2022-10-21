using CSUtilities.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CSUtilities.Extensions
{
	internal static class EnumExtensions
	{
		[Obsolete("Use Type.GetValues()")]
		public static IEnumerable<T> GetValues<T>()
		{
			return Enum.GetValues(typeof(T)).Cast<T>();
		}

		[Obsolete("Use Type.GetNames()")]
		public static IEnumerable<string> GetValuesNames<T>()
		{
			return Enum.GetValues(typeof(T)).Cast<T>().Select(o => o.ToString());
		}

		public static T GetValueByName<T>(string name)
		{
			return Enum.GetValues(typeof(T)).Cast<T>().FirstOrDefault(o => o.ToString() == name);
		}

		/// <summary>
		/// Gets a string value for a particular enum value.
		/// </summary>
		/// <param name="value">enum value</param>
		/// <returns>String Value associated via a <see cref="StringValueAttribute"/> attribute, or null if not found.</returns>
		public static string GetStringValue<T>(this T value) where T : Enum
		{
			Type type = value.GetType();

			FieldInfo fi = type.GetField(value.ToString());
			return fi.GetCustomAttribute<StringValueAttribute>()?.Value;
		}
	}
}