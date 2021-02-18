using MeshIO.FBX.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeshIO.FBX.Nodes
{
	public class FbxObjectTypeNode : FbxEmitter
	{
		public override string ClassName { get { return "ObjectType"; } }
		public string TypeName { get; set; }
		[FbxChildNode("Count")]
		public int Count { get; set; }
		[FbxChildNode("PropertyTemplate")]
		public FbxPropertyTemplate PropertyTemplate { get; set; }
		public FbxObjectTypeNode() : base()
		{
			Count = 1;
		}

		public override FbxNode ToFbxNode()
		{
			var node = base.ToFbxNode();

			node.Properties.Add(TypeName);

			return node;
		}
	}
}
