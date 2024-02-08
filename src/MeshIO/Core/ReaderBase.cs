using System;
using System.IO;

namespace MeshIO.Core
{
	public delegate void NotificationEventHandler(object sender, NotificationEventArgs e);

	public abstract class ReaderBase : IDisposable
	{
		public event NotificationEventHandler OnNotification;

		protected readonly Stream _stream;

		protected ReaderBase(Stream stream)
		{
			if (stream == null)
				throw new ArgumentNullException(nameof(stream));

			if (!stream.CanSeek)
				throw new ArgumentException("The stream must support seeking");

			this._stream = stream;
		}

		public abstract Scene Read();

		/// <inheritdoc/>
		public virtual void Dispose()
		{
			this._stream.Dispose();
		}

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
