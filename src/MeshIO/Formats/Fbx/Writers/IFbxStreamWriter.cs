using System;

namespace MeshIO.Formats.Fbx.Writers;

internal interface IFbxStreamWriter : IDisposable
{
	void Write();
}
