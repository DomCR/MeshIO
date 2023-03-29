using MeshIO.Core;
using MeshIO.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshIO.FBX.Converters
{
	/// <summary>
	/// Converts a <see cref="FbxRootNode"/> into a <see cref="Scene"/>
	/// </summary>
	public interface INodeConverter
	{
		FbxVersion Version { get; }

		Scene ConvertScene();
	}
}
