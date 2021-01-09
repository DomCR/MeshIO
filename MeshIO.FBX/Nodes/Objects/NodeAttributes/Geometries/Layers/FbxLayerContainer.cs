using System;
using System.Collections.Generic;
using System.Text;

namespace MeshIO.FBX.Nodes.Objects.NodeAttributes.Geometries.Layers
{
	public class FbxLayerContainer : IFbxNodeReference
	{
		/// <summary>
		/// Constructor by fbx node.
		/// </summary>
		/// <param name="parentNode">Node of the object that contains the layers.</param>
		public FbxLayerContainer(FbxNode parentNode)
		{

		}

		public FbxNode ToFbxNode()
		{
			throw new NotImplementedException();
		}

		public FbxNode ToFbxNode(FbxVersion version)
		{
			throw new NotImplementedException();
		}
	}
}
