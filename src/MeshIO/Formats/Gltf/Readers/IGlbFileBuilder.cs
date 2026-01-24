namespace MeshIO.Formats.Gltf.Readers;

internal interface IGlbFileBuilder
{
	event NotificationEventHandler OnNotification;

	public Scene Build();
}
