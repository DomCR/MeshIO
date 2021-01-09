using MeshIO.FBX.Attributes;
using MeshIO.FBX.Nodes.Objects;
using MeshIO.FBX.Nodes.Objects.NodeAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MeshIO.FBX.Nodes
{
	internal static class FbxNodeBuilder
	{
		public static FbxObject CreateFbxObject(FbxNode node)
		{
			switch (node.Name)
			{
				case "Model":
					return new FbxModel(node);
				case "Material":
					return new FbxMaterial(node);
				case "Geometry":
					return CreateGeometry(node);
				default:
					return null;
			}
		}
		public static FbxObjectType GetNodeType(FbxNode node)
		{
			string strType = node.Properties.Last() as string;
			if (Enum.TryParse<FbxObjectType>(strType, out FbxObjectType type))
			{
				return type;
			}
			else
				return FbxObjectType.Unknown;
		}
		public static FbxObject CreateGeometry(FbxNode node)
		{
			FbxObjectType type = GetNodeType(node);

			switch (type)
			{
				case FbxObjectType.Null:
					break;
				case FbxObjectType.Marker:
					break;
				case FbxObjectType.Skeleton:
					break;
				case FbxObjectType.Mesh:
					return new FbxMesh(node);
				case FbxObjectType.Nurbs:
					break;
				case FbxObjectType.Patch:
					break;
				case FbxObjectType.Camera:
					return new FbxCamera(node);
				case FbxObjectType.CameraStereo:
					break;
				case FbxObjectType.CameraSwitcher:
					break;
				case FbxObjectType.Light:
					break;
				case FbxObjectType.OpticalReference:
					break;
				case FbxObjectType.OpticalMarker:
					break;
				case FbxObjectType.NurbsCurve:
					break;
				case FbxObjectType.TrimNurbsSurface:
					break;
				case FbxObjectType.Boundary:
					break;
				case FbxObjectType.NurbsSurface:
					break;
				case FbxObjectType.Shape:
					break;
				case FbxObjectType.LODGroup:
					break;
				case FbxObjectType.SubDiv:
					break;
				case FbxObjectType.CachedEffect:
					break;
				case FbxObjectType.Line:
					return new FbxLine(node);
				case FbxObjectType.ObjectMetaData:
					break;
				case FbxObjectType.Unknown:
				default:
					return null;
			}

			return null;
		}
		public static FbxObject CreateFbxObject(FbxNode node, FbxVersion version)
		{
			throw new NotImplementedException();
		}
		public static Dictionary<string, PropertyInfo> CreateReferenceMap(Type referenceType)
		{
			Dictionary<string, PropertyInfo> map = new Dictionary<string, PropertyInfo>();

			//Setup the fbx children
			foreach (PropertyInfo prop in referenceType.GetProperties())
			{
				FbxChildNodeAttribute att = prop.GetCustomAttribute<FbxChildNodeAttribute>();
				if (att == null)
					continue;

				map.Add(att.Name, prop);
			}

			return map;
		}
	}
}
