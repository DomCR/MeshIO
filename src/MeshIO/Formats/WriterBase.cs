using System;

namespace MeshIO.Formats
{
	public abstract class WriterBase : IDisposable
	{
		public event NotificationEventHandler OnNotification;

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
