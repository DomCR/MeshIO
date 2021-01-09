using System;
using System.Collections.Generic;
using System.Text;

namespace MeshIO.FBX.Nodes.Connections
{
	public class FbxConnectionColletion : FbxNodeReferenceCollection<FbxConnection>
	{
		public override string ClassName { get { return "Connections"; } }
		public FbxConnectionColletion() : base() { }
		public FbxConnectionColletion(FbxNode node) : base(node) { }

		protected override FbxConnection buildChild(FbxNode node)
		{
			return new FbxConnection(node);
		}
	}
}
