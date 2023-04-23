using System;

namespace MeshIO.Core
{
	public delegate void NotificationEventHandler(object sender, NotificationEventArgs e);

	public abstract class ReaderBase : IDisposable
	{
		public event NotificationEventHandler OnNotification;

		/// <inheritdoc/>
		public abstract void Dispose();

		protected void triggerNotification(string message, NotificationType notificationType, Exception ex = null)
		{
			this.onNotificationEvent(this, new NotificationEventArgs(message, notificationType, ex));
		}

		protected void onNotificationEvent(object sender, NotificationEventArgs e)
		{
			this.OnNotification?.Invoke(this, e);
		}
	}
}
