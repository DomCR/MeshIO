using CSUtilities.Wpf;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSUtilities.Wpf.ViewModels
{
	internal class WorkProgressViewModel : BaseViewModel
	{
		public string Message
		{
			get { return m_message; }
			set
			{
				m_message = value;
				onPropertyChanged();
			}
		}
		private string m_message;
	}
}
