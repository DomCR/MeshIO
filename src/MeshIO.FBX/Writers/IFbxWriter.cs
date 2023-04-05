using System;

namespace MeshIO.FBX.Writers
{
	public interface IFbxWriter : IDisposable
	{
		void Write();
	}
}
