using System;
using System.IO;

namespace MeshIO.FBX.Writers
{
	internal interface IFbxWriter : IDisposable
	{
		void Write();
	}

	internal static class FbxWriterFactory
	{
		public static IFbxWriter Create(FbxWriterOptions options, FbxRootNode root, Stream stream)
		{
			if (options.IsBinaryFormat)
			{
				return new FbxBinaryWriter(root, stream);
			}
			else
			{
				return new FbxAsciiWriter(root, stream);
			}
		}
	}
}
