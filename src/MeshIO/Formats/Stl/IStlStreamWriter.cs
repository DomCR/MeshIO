namespace MeshIO.Formats.Stl;

internal interface IStlStreamWriter
{
	public event NotificationEventHandler OnNotification;

	public void Write();
}