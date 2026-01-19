using MeshIO.Formats.Fbx;
using MeshIO.Formats.Gltf;
using MeshIO.Formats.Obj;
using MeshIO.Formats.Stl;
using System;
using System.IO;

namespace MeshIO.Formats;

public static class FileFormat
{
	public const string Fbx = "fbx";

	public const string Glb = "glb";

	public const string Gltf = "gltf";

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

	/// <summary>
	/// Returns the standard file extension associated with the specified file format type.
	/// </summary>
	/// <param name="type">The file format type for which to retrieve the corresponding file extension.</param>
	/// <returns>A string containing the file extension for the specified file format type, including the leading period (for
	/// example, ".fbx").</returns>
	/// <exception cref="NotSupportedException">Thrown if <paramref name="type"/> is <see cref="FileFormatType.Unknown"/> or an unsupported value.</exception>
	public static string GetExtension(FileFormatType type)
	{
		switch (type)
		{
			case FileFormatType.Fbx:
				return Fbx;
			case FileFormatType.Gltf:
				return Gltf;
			case FileFormatType.Glb:
				return Glb;
			case FileFormatType.Obj:
				return Obj;
			case FileFormatType.Stl:
				return Stl;
			case FileFormatType.Unknown:
			default:
				throw new NotSupportedException();
		}
	}

	/// <summary>
	/// Creates an appropriate scene reader for the specified file path based on the file's extension.
	/// </summary>
	/// <remarks>Supported file formats include FBX, GLB, GLTF, OBJ, and STL. The method selects the appropriate
	/// reader implementation based on the file extension. If the file format is not supported, a <see
	/// cref="NotSupportedException"/> is thrown.</remarks>
	/// <param name="path">The path to the scene file to be read. The file extension determines the reader type. Cannot be null or empty.</param>
	/// <param name="notification">An optional event handler for receiving notifications during the reading process. May be null if no notifications
	/// are required.</param>
	/// <returns>An <see cref="ISceneReader"/> instance capable of reading the specified file format.</returns>
	/// <exception cref="NotSupportedException">Thrown if the file extension is not recognized or supported.</exception>
	public static ISceneReader GetReader(string path, NotificationEventHandler notification = null)
	{
		var type = FileFormat.FromExtension(Path.GetExtension(path));
		switch (type)
		{
			case FileFormatType.Fbx:
				return new FbxReader(path, notification);
			case FileFormatType.Glb:
			case FileFormatType.Gltf:
				return new GltfReader(path, notification);
			case FileFormatType.Obj:
				return new Obj.ObjReader(path, notification);
			case FileFormatType.Stl:
				return new Stl.StlReader(path, notification);
			case FileFormatType.Unknown:
			default:
				throw new NotSupportedException();
		}
	}

	/// <summary>
	/// Creates an appropriate scene reader for the specified file format and input stream.
	/// </summary>
	/// <param name="type">The file format type to read from the stream. Must be a supported value of <see cref="FileFormatType"/>.</param>
	/// <param name="stream">The input stream containing the scene data to be read. The stream must be readable and positioned at the start of
	/// the data.</param>
	/// <param name="notification">An optional event handler for receiving notifications during the reading process. May be <see langword="null"/> if
	/// notifications are not required.</param>
	/// <returns>An <see cref="ISceneReader"/> instance capable of reading scene data from the specified stream in the given
	/// format.</returns>
	/// <exception cref="NotSupportedException">Thrown if <paramref name="type"/> is <see cref="FileFormatType.Unknown"/> or not supported.</exception>
	public static ISceneReader GetReader(FileFormatType type, Stream stream, NotificationEventHandler notification = null)
	{
		switch (type)
		{
			case FileFormatType.Fbx:
				return new FbxReader(stream, notification);
			case FileFormatType.Glb:
			case FileFormatType.Gltf:
				return new GltfReader(stream, notification);
			case FileFormatType.Obj:
				return new ObjReader(stream, notification);
			case FileFormatType.Stl:
				return new StlReader(stream, notification);
			case FileFormatType.Unknown:
			default:
				throw new NotSupportedException();
		}
	}

	public static ISceneWriter GetWriter(string path, Scene scene, SceneWriterOptions options = null, NotificationEventHandler notification = null)
	{
		var type = FileFormat.FromExtension(Path.GetExtension(path));
		switch (type)
		{
			case FileFormatType.Fbx:
				return new FbxWriter(path, scene, options as FbxWriterOptions, notification);
			case FileFormatType.Glb:
			case FileFormatType.Gltf:
				throw new NotImplementedException();
			//return new GltfWriter(path, notification);
			case FileFormatType.Obj:
				throw new NotImplementedException();
			//return new Obj.ObjWriter(path, notification);
			case FileFormatType.Stl:
				return new Stl.StlWriter(path, scene, options as StlWriterOptions, notification);
			case FileFormatType.Unknown:
			default:
				throw new NotSupportedException();
		}
	}

	public static ISceneWriter GetWriter(FileFormatType type, Stream stream, Scene scene, NotificationEventHandler notification = null)
	{
		switch (type)
		{
			case FileFormatType.Fbx:
				return new FbxWriter(stream, scene, notification: notification);
			case FileFormatType.Glb:
			case FileFormatType.Gltf:
				throw new NotImplementedException();
			//return new GltfWriter(path, notification);
			case FileFormatType.Obj:
				throw new NotImplementedException();
			//return new Obj.ObjWriter(path, notification);
			case FileFormatType.Stl:
				return new StlWriter(stream, scene, notification: notification);
			case FileFormatType.Unknown:
			default:
				throw new NotSupportedException();
		}
	}

	public static SceneWriterOptions GetWriterOptions(string path)
	{
		var type = FileFormat.FromExtension(Path.GetExtension(path));
		return GetWriterOptions(type);
	}

	public static SceneWriterOptions GetWriterOptions(FileFormatType type)
	{
		switch (type)
		{
			case FileFormatType.Fbx:
				return new Fbx.FbxWriterOptions();
			case FileFormatType.Glb:
			case FileFormatType.Gltf:
				throw new NotImplementedException();
			//return new GltfWriter(path, notification);
			case FileFormatType.Obj:
				throw new NotImplementedException();
			//return new Obj.ObjWriter(path, notification);
			case FileFormatType.Stl:
				return new Stl.StlWriterOptions();
			case FileFormatType.Unknown:
			default:
				throw new NotSupportedException();
		}
	}
}