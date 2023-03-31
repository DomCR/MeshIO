using System;

namespace MeshIO.Core
{
	public class NotificationEventArgs : EventArgs
	{
		public string Message { get; }

		public NotificationType NotificationType { get; }

		public Exception Exception { get; }

		public NotificationEventArgs(string message, NotificationType notificationType = NotificationType.None, Exception exception = null)
		{
			this.Message = message;
			this.NotificationType = notificationType;
			this.Exception = exception;
		}
	}
}
