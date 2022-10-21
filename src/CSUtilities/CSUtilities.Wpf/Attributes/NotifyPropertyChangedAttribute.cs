using System;
using System.Collections.Generic;
using System.Text;

namespace CSUtilities.Wpf.Attributes
{
	[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
	public sealed class NotifyPropertyChangedAttribute : Attribute
	{
		public NotifyPropertyChangedAttribute()
		{
		}
	}
}
