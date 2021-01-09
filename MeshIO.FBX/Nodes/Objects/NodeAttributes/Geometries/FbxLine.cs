using System;
using System.Collections.Generic;
using System.Text;

namespace MeshIO.FBX.Nodes.Objects.NodeAttributes
{
	public class FbxLine : FbxGeometry
	{
		protected override FbxObjectType m_objectType => FbxObjectType.Line;
		private FbxNode node;

		public FbxLine(FbxNode node) : base(node)
		{
			this.node = node;
		}

	}
}
