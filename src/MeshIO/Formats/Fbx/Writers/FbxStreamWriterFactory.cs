using System.IO;

namespace MeshIO.Formats.Fbx.Writers;

internal static class FbxStreamWriterFactory
{
	public static IFbxStreamWriter Create(FbxWriterOptions options, FbxRootNode root, Stream stream)
	{
		if (options.ContentType == ContentType.Binary)
		{
			return new FbxBinaryWriter(root, stream);
		}
		else
		{
			return new FbxAsciiWriter(root, stream);
		}
	}
}
