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
		public FbxMaterial(FbxNode node) : base(node)
		{
			ShadingModel = (string)node["ShadingModel"]?.Value;
			MultiLayer = Convert.ToInt32(node["MultiLayer"]?.Value);
		}
	}
}
