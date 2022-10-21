using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace CSUtilities.Wpf
{
	/// <summary>
	/// Base view model with the default methods for <see cref="INotifyPropertyChanged"/>.
	/// </summary>
	public abstract class BaseViewModel : INotifyPropertyChanged
	{
		/// <summary>
		/// The event that is fired when any child property changes its value.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;
		/// <summary>
		/// Base property changed method.
		/// </summary>
		/// <param name="propertyName"></param>
		protected void onPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
