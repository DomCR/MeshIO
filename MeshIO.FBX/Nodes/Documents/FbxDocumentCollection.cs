using MeshIO.FBX.Attributes;
using System;

namespace MeshIO.FBX.Nodes
{
	public class FbxDocumentCollection : FbxNodeReferenceCollection<FbxDocumentInfo>
	{
		public override string ClassName { get { return "Documents"; } }
		[FbxChildNode("Count")]
		public int Count { get { return m_children.Count; } }
		public FbxDocumentCollection() { }
		public FbxDocumentCollection(FbxNode node)
		{
		}

		protected override FbxDocumentInfo buildChild(FbxNode node)
		{
			throw new NotImplementedException();
		}
	}
}
