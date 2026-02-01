using System;

namespace MeshIO.Formats.Fbx;

public class FbxFileToken
{
	public const string Camera = "Camera";

	public const string Connections = "Connections";

	public const string Count = "Count";

	public const string CreationTimeStamp = "CreationTimeStamp";

	public const string Creator = "Creator";

	public const string Culling = "Culling";

	public const string CullingOff = "CullingOff";

	public const string Definitions = "Definitions";

	public const string Document = "Document";

	public const string Documents = "Documents";

	public const string Edges = "Edges";

	public const string EncryptionType = "EncryptionType";

	public const string FBXHeaderExtension = "FBXHeaderExtension";

	public const string FBXHeaderVersion = "FBXHeaderVersion";

	public const string FBXVersion = "FBXVersion";

	public const string Geometry = "Geometry";

	public const string GeometryVersion = "GeometryVersion";

	public const string GlobalSettings = "GlobalSettings";

	public const string Layer = "Layer";

	public const string LayerElementBinormal = "LayerElementBinormal";

	public const string LayerElementMaterial = "LayerElementMaterial";

	public const string LayerElementNormal = "LayerElementNormal";

	public const string LayerElementSmoothing = "LayerElementSmoothing";

	public const string LayerElementTangent = "LayerElementTangent";

	public const string LayerElementUV = "LayerElementUV";

	public const string Light = "Light";

	public const string LookAt = "LookAt";

	public const string Material = "Material";

	public const string Mesh = "Mesh";

	public const string Model = "Model";

	public const string NodeAttribute = "NodeAttribute";

	public const string NodeAttributeName = "NodeAttributeName";

	public const string Objects = "Objects";

	public const string ObjectType = "ObjectType";

	public const string PolygonVertexIndex = "PolygonVertexIndex";

	public const string Position = "Position";

	public const string Properties60 = "Properties60";

	public const string Properties70 = "Properties70";

	public const string References = "References";

	public const string RootNode = "RootNode";

	public const string Scene = "Scene";

	public const string Shading = "Shading";

	public const string TypeFlags = "TypeFlags";

	public const string Up = "Up";

	public const string Version = "Version";

	public const string Vertices = "Vertices";

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
				return Properties60;
			case FbxVersion.v7000:
			case FbxVersion.v7100:
			case FbxVersion.v7200:
			case FbxVersion.v7300:
			case FbxVersion.v7400:
			case FbxVersion.v7500:
			case FbxVersion.v7600:
			case FbxVersion.v7700:
				return Properties70;
			default:
				throw new NotSupportedException();
		}
	}
}