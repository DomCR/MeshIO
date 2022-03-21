using MeshIO.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshIO.FBX.Converters
{
	/// <summary>
	/// Converts a fbx scene to a <see cref="FbxRootNode"/>
	/// </summary>
	public interface IFbxConverter
	{
		FbxVersion Version { get; }

		FbxRootNode ToRootNode();
	}
}
