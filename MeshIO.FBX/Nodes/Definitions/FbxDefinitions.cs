using System;
using System.Collections.Generic;
using System.Text;

namespace MeshIO.FBX.Nodes
{
	public class FbxDefinitions //: FbxNodeReferenceCollection
	{
		//public override string ClassName { get { return "Definitions"; } }
		private FbxNode node;

		public FbxDefinitions(FbxNode node)
		{
			this.node = node;
		}
	}
}
