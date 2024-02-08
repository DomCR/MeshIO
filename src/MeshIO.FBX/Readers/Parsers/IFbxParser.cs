using System;

namespace MeshIO.FBX.Readers.Parsers
{
	internal interface IFbxParser : IDisposable
	{
		FbxRootNode Parse();
	}
}
