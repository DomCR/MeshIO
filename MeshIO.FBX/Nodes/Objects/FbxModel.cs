using MeshIO.FBX.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeshIO.FBX.Nodes.Objects
{
	//Model: 2561475601696, "Model::RPC Tree - Deciduous Honey Locust - 25' [947379]", "Null" {
	//	Version: 232
	//	Properties70:  { }
	//	Shading: T
	//	Culling: "CullingOff"
	//}
	public class FbxModel : FbxObject
	{
		public override string ClassName { get { return "Model"; } }
		public bool Shading { get; set; }
		public string Culling { get; set; }
		public FbxModel() : base()
		{
			Shading = true;
			Culling = "CullingOff";
		}
		public FbxModel(FbxNode node) : base(node)
		{
			Shading = (char)node["Shading"]?.Value == 'T';
			Culling = (string)node["Culling"]?.Value;
		}
		public FbxModel(GElement element) : this()
		{
			Name = element.Name;
		}

		public override FbxNode ToFbxNode()
		{
			FbxNode node = base.ToFbxNode();

			node.Nodes.Add(new FbxNode("Culling", Culling));
			node.Nodes.Add(new FbxNode("Shading", Shading ? 'T' : 'F'));

			return node;
		}
	}
}
