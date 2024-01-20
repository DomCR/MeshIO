using System;

namespace MeshIO.FBX.Writers
{
	[Obsolete]
	public interface IFbxWriter : IDisposable
	{
		void Write();
	}
}
