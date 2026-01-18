using System;

namespace MeshIO.Formats;

public static class FileExtensions
{
	public const string Fbx = "fbx";
	public const string Gltf = "gltf";
	public const string Glb = "glb";
	public const string Obj = "obj";
	public const string Stl = "stl";

	/// <summary>
	/// Returns the corresponding file format type for the specified file extension.
	/// </summary>
	/// <param name="extension">The file extension to evaluate. May include or omit the leading period (e.g., ".fbx" or "fbx"). The comparison is
	/// case-insensitive.</param>
	/// <returns>A value of the <see cref="FileFormatType"/> enumeration that represents the file format associated with the
	/// specified extension.</returns>
	/// <exception cref="NotSupportedException">Thrown if the specified <paramref name="extension"/> does not correspond to a supported file format.</exception>
	public static FileFormatType FromExtension(string extension)
	{
		if (extension.StartsWith("."))
		{
			extension = extension.Substring(1);
		}

		switch (extension.ToLower())
		{
			case Fbx:
				return FileFormatType.Fbx;
			case Gltf:
				return FileFormatType.Gltf;
			case Glb:
				return FileFormatType.Glb;
			case Obj:
				return FileFormatType.Obj;
			case Stl:
				return FileFormatType.Stl;
			default:
				throw new NotSupportedException();
		}
	}
}