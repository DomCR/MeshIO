using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace CSUtilities.Wpf
{
	/// <summary>
	/// A basic command that runs an action.
	/// </summary>
	public class RelayCommand : ICommand
	{
		/// <summary>
		/// The event thats fired when the <see cref="CanExecute(object)"/> value has changed
		/// </summary>
		public event EventHandler CanExecuteChanged = (sender, e) => { };
		private Action<object> m_action;
		/// <summary>
		/// Default constructor.
		/// </summary>
		public RelayCommand(Action<object> action)
		{
			m_action = action;
		}
		//************************************************************************************
		/// <summary>
		/// A relay command can always execute.
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		public bool CanExecute(object parameter)
		{
			return true;
		}
		/// <summary>
		/// Execute the assigned action.
		/// </summary>
		/// <param name="parameter"></param>
		public void Execute(object parameter)
		{
			m_action(parameter);
		}
	}
}
