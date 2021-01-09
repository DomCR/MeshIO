using MeshIO.FBX.Attributes;
using System;

namespace MeshIO.FBX.Nodes
{
	/*
	FBXHeaderExtension:  {
		FBXHeaderVersion: 1003
		FBXVersion: 7400
		CreationTimeStamp:  { }
		Creator: "FBX SDK/FBX Plugins version 2020.0.1"
		SceneInfo: "SceneInfo::GlobalInfo", "UserData" { }
	}
	 */
	public class FbxHeader : FbxNodeReference
	{
		public override string ClassName { get { return "FBXHeaderExtension"; } }
		[FbxChildNode("FBXHeaderVersion")]
		public int FBXHeaderVersion { get; set; }
		//[FbxChildNode("FBXVersion")]
		public FbxVersion FBXVersion { get; set; } = FbxVersion.v7200;
		[FbxChildNode("Creator")]
		public string Creator { get; set; } = "MeshIO.FBX";
		public FbxHeader() : base() { }
		public FbxHeader(FbxNode node) : base(node)
		{
			FBXHeaderVersion = (int)(node["FBXHeaderVersion"]?.Value);
			FBXVersion = (FbxVersion)Enum.ToObject(typeof(FbxVersion), node["FBXVersion"]?.Value);
			Creator = (string)(node["Creator"]?.Value);
		}
	}
}
