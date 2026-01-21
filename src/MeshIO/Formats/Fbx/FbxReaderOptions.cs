namespace MeshIO.Formats.Fbx;

public class FbxReaderOptions : SceneReaderOptions
{
	public ErrorLevel ErrorLevel { get; set; } = ErrorLevel.Permissive;
}