using MeshIO.FBX.Nodes.Objects.NodeAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeshIO.FBX.Nodes.Objects
{
	public abstract class FbxGeometry : FbxNodeAttribute
	{
		public override string ClassName { get { return "Geometry"; } }
		public FbxGeometry(FbxNode node) : base(node) { }
		public static FbxGeometry Create(FbxNode node)
		{
			string attrType = (string)node.Properties.Last();

			if (!Enum.TryParse<FbxObjectType>(attrType, out FbxObjectType objectType))
				return null;

			switch (objectType)
			{
				case FbxObjectType.Unknown:
					return null;
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
					break;
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
					break;
				case FbxObjectType.ObjectMetaData:
					break;
				default:
					break;
			}

			return null;
		}
	}
}
