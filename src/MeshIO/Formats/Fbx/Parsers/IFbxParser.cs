using System;

namespace MeshIO.Formats.Fbx.Parsers;

internal interface IFbxParser : IDisposable
{
	FbxRootNode Parse();
}
