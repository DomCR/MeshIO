using MeshIO.Core;
using System;

namespace MeshIO.FBX.Converters
{
	public abstract class ConverterBase
	{
		public event NotificationEventHandler OnNotification;

		protected void notify(string message, NotificationType notificationType = NotificationType.Information, Exception ex = null)
		{
			this.OnNotification?.Invoke(this, new NotificationEventArgs(message, notificationType, ex));
		}
	}
}
