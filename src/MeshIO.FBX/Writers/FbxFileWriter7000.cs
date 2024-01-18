using System.IO;

namespace MeshIO.FBX.Writers
{
	internal class FbxFileWriter7000 : FbxFileWriterBase
	{
		public FbxFileWriter7000(Scene scene, FbxWriterOptions options, Stream stream) : base(scene, options, stream)
		{
		}
	}
}
