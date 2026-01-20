namespace MeshIO.Formats;

/// <summary>
/// Specifies the supported file format types for 3D model files.
/// </summary>
/// <remarks>Use this enumeration to indicate or determine the format of a 3D model file when loading, saving, or
/// processing 3D assets. The <see
/// cref="FileFormatType.Unknown"/> value can be used when the file format is not recognized or has not been
/// specified.</remarks>
public enum FileFormatType
{
	Unknown,
	Fbx,
	Gltf,
	Glb,
	Obj,
	Stl
}
