namespace MeshIO.Formats;

/// <summary>
/// Gets or sets the content type to use when writing the scene.
/// </summary>
public abstract class SceneWriterOptions
{
	/// <summary>
	/// Gets or sets the media type and parameters for the content associated with this instance.
	/// </summary>
	public ContentType ContentType { get; set; } = ContentType.ASCII;
}
