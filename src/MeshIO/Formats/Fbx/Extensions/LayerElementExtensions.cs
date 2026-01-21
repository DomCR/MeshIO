using MeshIO.Entities.Geometries.Layers;
using System;

namespace MeshIO.Formats.Fbx.Extensions;

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

	public static bool TryParseMappingMode(string value, out MappingMode mappingMode)
	{
		switch (value)
		{
			case "ByVertice":
				mappingMode = MappingMode.ByVertex;
				return true;
			case "ByPolygonVertex":
				mappingMode = MappingMode.ByPolygonVertex;
				return true;
			case "ByPolygon":
				mappingMode = MappingMode.ByPolygon;
				return true;
			case "ByEdge":
				mappingMode = MappingMode.ByEdge;
				return true;
			case "AllSame":
				mappingMode = MappingMode.AllSame;
				return true;
			default:
				mappingMode = default;
				return false;
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

	public static bool TryParseReferenceMode(string value, out ReferenceMode referenceMode)
	{
		switch (value)
		{
			case "Direct":
				referenceMode = ReferenceMode.Direct;
				return true;
			case "Index":
				referenceMode = ReferenceMode.Index;
				return true;
			case "IndexToDirect":
				referenceMode = ReferenceMode.IndexToDirect;
				return true;
			default:
				referenceMode = default;
				return false;
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
