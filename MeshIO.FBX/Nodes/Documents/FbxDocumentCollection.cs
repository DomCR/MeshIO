using MeshIO.FBX.Attributes;
using System;

namespace MeshIO.FBX.Nodes
{
	public class FbxDocumentCollection: FbxNodeReferenceCollection<FbxDocument>
	{
		public override string ClassName { get { return "Documents"; } }
		[FbxChildNode]
		public int Count { get { return m_children.Count; } }
		public FbxDocumentCollection() { }
		public FbxDocumentCollection(FbxNode node) 
		{
		}

		protected override FbxDocument buildChild(FbxNode node)
		{
			throw new NotImplementedException();
		}
	}
}
