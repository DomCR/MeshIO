namespace MeshIO.Formats.Stl;

/// <summary>
/// Provides options for configuring the behavior of an STL file writer.
/// </summary>
/// <remarks>Use this class to specify settings when exporting 3D scenes to the STL (Stereolithography) file
/// format. Inherits common options from <see cref="SceneWriterOptions"/>.</remarks>
public class StlWriterOptions : SceneWriterOptions
{
	public bool MergeMeshes { get; set; } = false;
}
