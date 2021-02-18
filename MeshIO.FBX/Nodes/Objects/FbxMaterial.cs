using MeshIO.FBX.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeshIO.FBX.Nodes.Objects
{
	public class FbxMaterial : FbxObject
	{
		public override string ClassName { get { return "Material"; } }
		public string ShadingModel { get; set; } = "unknown";
		public int MultiLayer { get; set; }

		public FbxMaterial() : base()
		{
			ShadingModel = "unknown";
			MultiLayer = 0;
		}
		public FbxMaterial(FbxNode node) : base(node)
		{
			ShadingModel = (string)node["ShadingModel"]?.Value;
			MultiLayer = Convert.ToInt32(node["MultiLayer"]?.Value);
		}
		public FbxMaterial(Material material)
		{
			Name = material.Name;
		}
		//******************************************************************************
		public override FbxNode ToFbxNode()
		{
			FbxNode node = base.ToFbxNode();

			node.Nodes.Add(new FbxNode("ShadingModel", ShadingModel));
			node.Nodes.Add(new FbxNode("MultiLayer", MultiLayer));

			return node;
		}
	}
}
