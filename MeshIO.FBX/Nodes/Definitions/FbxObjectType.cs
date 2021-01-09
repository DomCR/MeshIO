using MeshIO.FBX.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeshIO.FBX.Nodes.Definitions
{
	public class FbxObjectType : FbxNodeReference
	{
		public override string ClassName { get { return "ObjectType"; } }

		[FbxChildNode]
		public int Count { get; }
		[FbxChildNode]
		public FbxPropertyTemplate PropertyTemplate { get; set; }
	}
}
