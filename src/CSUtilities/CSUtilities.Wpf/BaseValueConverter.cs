using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;

namespace CSUtilities.Wpf
{
	/// <summary>
	/// A base value converter that allows dirext XAML usage.
	/// </summary>
	/// <typeparam name="T">Type of the converter</typeparam>
	public abstract class BaseValueConverter<T> : MarkupExtension, IValueConverter
		where T : class, new()
	{
		/// <summary>
		/// A single static instance of this value converter.
		/// </summary>
		private static T m_converter = null;

		/// <summary>
		/// Provide a static instance of the value converter.
		/// </summary>
		/// <param name="serviceProvider"></param>
		/// <returns></returns>
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return m_converter ?? (m_converter = new T());
		}
		/// <summary>
		/// The method to convert one type to another.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);
		/// <summary>
		/// The method to convert a value back to it's source.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
	}
}
