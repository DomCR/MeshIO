using System;

namespace MeshIO.FBX
{
	public class FbxFileToken
	{
		public const string Version = "Version";
		public const string Count = "Count";

		public const string FBXHeaderExtension = "FBXHeaderExtension";
		public const string FBXHeaderVersion = "FBXHeaderVersion";
		public const string FBXVersion = "FBXVersion";
		public const string EncryptionType = "EncryptionType";
		public const string CreationTimeStamp = "CreationTimeStamp";
		public const string Creator = "Creator";

		public const string GlobalSettings = "GlobalSettings";

		public const string Documents = "Documents";
		public const string Document = "Document";

		public const string References = "References";

		public const string Definitions = "Definitions";
		public const string ObjectType = "ObjectType";

		public const string Objects = "Objects";

		public const string Connections = "Connections";

		public const string Scene = "Scene";
		public const string RootNode = "RootNode";

		public const string Mesh = "Mesh";

		public const string Model = "Model";
		public const string Shading = "Shading";
		public const string CullingOff = "CullingOff";

		public const string Geometry = "Geometry";
		public const string Material = "Material";

		public static string GetPropertiesName(FbxVersion version)
		{
			switch (version)
			{
				case FbxVersion.v2000:
				case FbxVersion.v2001:
				case FbxVersion.v3000:
				case FbxVersion.v3001:
				case FbxVersion.v4000:
				case FbxVersion.v4001:
				case FbxVersion.v4050:
				case FbxVersion.v5000:
				case FbxVersion.v5800:
					throw new NotSupportedException();
				case FbxVersion.v6000:
				case FbxVersion.v6100:
					return "Properties60";
				case FbxVersion.v7000:
				case FbxVersion.v7100:
				case FbxVersion.v7200:
				case FbxVersion.v7300:
				case FbxVersion.v7400:
				case FbxVersion.v7500:
				case FbxVersion.v7600:
				case FbxVersion.v7700:
					return "Properties70";
				default:
					throw new NotSupportedException();
			}
		}
	}
}
