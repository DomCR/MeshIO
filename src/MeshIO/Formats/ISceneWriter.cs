using System;

namespace MeshIO.Formats
{
	/// <summary>
	/// Defines a contract for writing scene data and receiving notifications during the write process.
	/// </summary>
	/// <remarks>Implementations of this interface are responsible for persisting or exporting scene information.
	/// The interface supports notification events to inform subscribers of progress or issues during the write operation.
	/// Implementers should ensure proper resource management by disposing of any unmanaged resources when no longer
	/// needed.</remarks>
	public interface ISceneWriter : IDisposable
	{
		public event NotificationEventHandler OnNotification;

		public void Write();
	}
}