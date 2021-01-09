using System;
using System.Collections.Generic;
using System.Text;

namespace MeshIO.FBX.Nodes.Objects.NodeAttributes
{
	public class FbxCamera : FbxNodeAttribute
	{
		protected override FbxObjectType m_objectType => FbxObjectType.Camera;
		public FbxCamera(FbxNode node) : base(node)
		{

		}
	}
}
