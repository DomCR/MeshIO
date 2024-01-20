using System;
using System.IO;

namespace MeshIO.FBX.Writers
{
	[Obsolete]
	public class FbxWriterBase
	{
		public static IFbxWriter GetWriter(FbxRootNode root, Stream stream, FbxFileFormat fbxFileFormat)
		{
			switch (fbxFileFormat)
			{
				case FbxFileFormat.Binary:
					return new FbxBinaryWriter(root, stream);
				case FbxFileFormat.ASCII:
					return new FbxAsciiWriter(root, stream);
				default:
					throw new NotSupportedException($"File format not supported {fbxFileFormat}");
			}
		}
	}
}
