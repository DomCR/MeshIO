using System;

namespace MeshIO
{
	public delegate void NotificationEventHandler(object sender, NotificationEventArgs e);

	public class NotificationEventArgs : EventArgs
	{
		public string Message { get; }

		public NotificationType NotificationType { get; }

		public Exception Exception { get; }

		public NotificationEventArgs(string message, NotificationType notificationType = NotificationType.Information, Exception exception = null)
		{
			this.Message = message;
			this.NotificationType = notificationType;
			this.Exception = exception;
		}
	}
}
