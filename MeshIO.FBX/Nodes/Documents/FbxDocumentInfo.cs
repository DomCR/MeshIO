using MeshIO.FBX.Nodes.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeshIO.FBX.Nodes
{
	/*
	 	Document: 1932821987968, "", "Scene" {
		Properties70:  {
			P: "SourceObject", "object", "", ""
			P: "ActiveAnimStackName", "KString", "", "", ""
		}
		RootNode: 0
	}
	 */
	public class FbxDocumentInfo : FbxObject
	{
		public override string ClassName { get { return "Document"; } }
		public int RootNode { get; set; }
		public FbxDocumentInfo()
		{
		}
		//****************************************************************
		public override FbxNode ToFbxNode()
		{
			FbxNode node = base.ToFbxNode();

			node.Nodes.Add(new FbxNode("RootNode", RootNode));

			return node;
		}
	}
}
