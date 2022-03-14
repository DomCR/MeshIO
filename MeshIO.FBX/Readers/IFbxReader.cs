using MeshIO.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshIO.FBX
{
	public interface IFbxReader : IDisposable
	{
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
