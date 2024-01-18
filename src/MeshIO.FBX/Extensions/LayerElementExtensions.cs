using MeshIO.Entities.Geometries.Layers;
using System;

namespace MeshIO.FBX.Extensions
{
	internal static class LayerElementExtensions
	{
		public static string GetFbxName(this LayerElement layer)
		{
			switch (layer)
			{
				case LayerElementNormal:
					return "LayerElementNormal";
				case LayerElementUV:
					return "LayerElementUV";
				default:
					throw new NotImplementedException($"Unknown LayerElement Type : {layer.GetType().FullName}");
			}
		}

		public static string GetFbxName(this MappingMode mappingMode)
		{
			switch (mappingMode)
			{
				case MappingMode.ByVertex:
					return "ByVertice";
				case MappingMode.ByPolygonVertex:
					return "ByPolygonVertex";
				case MappingMode.ByPolygon:
					return "ByPolygon";
				case MappingMode.ByEdge:
					return "ByEdge";
				case MappingMode.AllSame:
					return "AllSame";
				default:
					throw new ArgumentException($"Unknown MappingMode : {mappingMode}", nameof(mappingMode));
			}
		}

		public static string GetFbxName(this ReferenceMode referenceMode)
		{
			switch (referenceMode)
			{
				case ReferenceMode.Direct:
					return "Direct";
				case ReferenceMode.Index:
					return "Index";
				case ReferenceMode.IndexToDirect:
					return "IndexToDirect";
				default:
					throw new ArgumentException($"Unknown ReferenceMode : {referenceMode}", nameof(referenceMode));
			}
		}
	}
}
