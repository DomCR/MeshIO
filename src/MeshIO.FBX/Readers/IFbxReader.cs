using System;

namespace MeshIO.FBX
{
    public interface IFbxReader : IDisposable
	{
		/// <summary>
		/// Checks the version of the file
		/// </summary>
		/// <returns></returns>
		FbxVersion GetVersion();

		/// <summary>
		/// Parse the document into a node structure.
		/// </summary>
		/// <returns></returns>
		FbxRootNode Parse();

		/// <summary>
		/// Read the file into an fbx scene.
		/// </summary>
		/// <returns></returns>
		Scene Read();
	}
}
