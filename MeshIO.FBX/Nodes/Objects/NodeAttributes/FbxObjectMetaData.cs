using System;
using System.Collections.Generic;
using System.Text;

namespace MeshIO.FBX.Nodes.Objects.NodeAttributes
{
	public class FbxObjectMetaData : FbxNodeAttribute
	{
		public override string ClassName { get { return "ObjectMetaData"; } }
		protected override FbxObjectType m_objectType => FbxObjectType.ObjectMetaData;
		public FbxObjectMetaData(FbxNode node) : base(node) { }
	}
}
