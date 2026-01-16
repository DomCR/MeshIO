using System;

namespace MeshIO.Formats.Fbx.Readers.Parsers
{
	internal interface IFbxParser : IDisposable
	{
		FbxRootNode Parse();
	}
}
