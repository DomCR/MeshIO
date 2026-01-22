#if NETFRAMEWORK
using CSUtilities.Extensions;
#endif
using CSMath;
using MeshIO.Entities.Geometries;
using MeshIO.Entities.Geometries.Layers;
using MeshIO.Formats.Fbx.Extensions;
using MeshIO.Formats.Fbx.Readers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MeshIO.Formats.Fbx.Builders;

internal abstract class FbxGeometryBuilder<T> : FbxObjectBuilder<T>
	where T : Geometry
{
	public override string FbxObjectName { get { return FbxFileToken.Geometry; } }

	protected FbxGeometryBuilder(T geometry) : base(geometry) { }

	protected FbxGeometryBuilder(FbxNode node, T geometry) : base(node, geometry) { }

	public override void Build(FbxFileBuilderBase builder)
	{
		base.Build(builder);

		readLayers(builder.Version);
	}

	protected override void buildProperties(Dictionary<string, FbxProperty> properties)
	{
		if (properties.Remove("Primary Visibility", out FbxProperty isVisible))
		{
			_element.IsVisible = (bool)isVisible.ToProperty().Value;
		}

		if (properties.Remove("Casts Shadows", out FbxProperty castShadows))
		{
			_element.CastShadows = (bool)castShadows.ToProperty().Value;
		}

		if (properties.Remove("Receive Shadows", out FbxProperty receiveShadows))
		{
			_element.ReceiveShadows = (bool)receiveShadows.ToProperty().Value;
		}

		base.buildProperties(properties);
	}

	protected void readLayers(FbxVersion version)
	{
		FbxNode node = FbxNode;
		var geometry = _element;

		if (node.TryGetNode("LayerElementNormal", out FbxNode layerElementNormal))
		{
			geometry.Layers.Add(mapLayerElementNormal(version, layerElementNormal));
		}

		if (node.TryGetNode("LayerElementBinormal", out FbxNode layerElementBinormal))
		{
			geometry.Layers.Add(mapLayerElementBinormal(version, layerElementBinormal));
		}

		if (node.TryGetNode("LayerElementTangent", out FbxNode layerElementTangent))
		{
			geometry.Layers.Add(mapLayerElementTangent(version, layerElementTangent));
		}

		if (node.TryGetNode("LayerElementMaterial", out FbxNode layerElementMaterial))
		{
			geometry.Layers.Add(BuildLayerElementMaterial(version, layerElementMaterial));
		}

		if (node.TryGetNode("LayerElementUV", out FbxNode layerElementUV))
		{
			geometry.Layers.Add(mapLayerElementUV(version, layerElementUV));
		}

		if (node.TryGetNode("LayerElementSmoothing", out FbxNode layerElementSmoothing))
		{
			geometry.Layers.Add(mapLayerElementSmoothing(version, layerElementSmoothing));
		}
	}

	private void mapCommonLayer(LayerElement layer, FbxNode node)
	{
		if (node.TryGetNode("Name", out FbxNode name))
		{
			layer.Name = name.Value as string;
		}

		if (node.TryGetNode("MappingInformationType", out FbxNode mappingInformationType))
		{
			if (LayerElementExtensions.TryParseMappingMode((string)mappingInformationType.Value, out MappingMode mappingMode))
			{
				layer.MappingMode = mappingMode;
			}
		}

		if (node.TryGetNode("ReferenceInformationType", out FbxNode referenceInformationType))
		{
			if (LayerElementExtensions.TryParseReferenceMode((string)referenceInformationType.Value, out ReferenceMode referenceMode))
			{
				layer.ReferenceMode = referenceMode;
			}
		}
	}

	private LayerElement mapLayerElementNormal(FbxVersion version, FbxNode node)
	{
		LayerElementNormal layer = new LayerElementNormal();

		mapCommonLayer(layer, node);

		if (node.TryGetNode("Normals", out FbxNode normals))
		{
			layer.Normals = arrToXYZ(arrToDoubleArray(getArrayValue(version, normals)));
		}

		if (node.TryGetNode("NormalsW", out FbxNode normalsw))
		{
			layer.Weights.AddRange(arrToDoubleArray(getArrayValue(version, normalsw)));
		}

		return layer;
	}

	protected IEnumerable getArrayValue(FbxVersion version, FbxNode node)
	{
		if (version < FbxVersion.v7000)
		{
			return node.Properties;
		}
		else
		{
			return node.Value as IEnumerable;
		}
	}

	private LayerElement mapLayerElementBinormal(FbxVersion version, FbxNode node)
	{
		LayerElementBinormal layer = new LayerElementBinormal();

		mapCommonLayer(layer, node);

		if (node.TryGetNode("Binormals", out FbxNode normals))
		{
			layer.Normals = arrToXYZ(arrToDoubleArray(getArrayValue(version, normals)));
		}

		if (node.TryGetNode("BinormalsW", out FbxNode normalsw))
		{
			layer.Weights.AddRange(arrToDoubleArray(getArrayValue(version, normalsw)));
		}

		return layer;
	}

	private LayerElement mapLayerElementTangent(FbxVersion version, FbxNode node)
	{
		LayerElementTangent layer = new LayerElementTangent();

		mapCommonLayer(layer, node);

		if (node.TryGetNode("Tangents", out FbxNode normals))
		{
			layer.Tangents = arrToXYZ(arrToDoubleArray(getArrayValue(version, normals)));
		}

		if (node.TryGetNode("TangentsW", out FbxNode normalsw))
		{
			layer.Weights.AddRange(arrToDoubleArray(getArrayValue(version, normalsw)));
		}

		return layer;
	}

	private LayerElement mapLayerElementUV(FbxVersion version, FbxNode node)
	{
		LayerElementUV layer = new LayerElementUV();

		mapCommonLayer(layer, node);

		if (node.TryGetNode("UV", out FbxNode uv))
		{
			layer.UV = arrToXY(arrToDoubleArray(getArrayValue(version, uv)));
		}

		if (node.TryGetNode("UVIndex", out FbxNode indices))
		{
			layer.Indexes.AddRange(toArr<int>(getArrayValue(version, indices)));
		}

		return layer;
	}

	private LayerElement mapLayerElementSmoothing(FbxVersion version, FbxNode node)
	{
		LayerElementSmoothing layer = new LayerElementSmoothing();

		mapCommonLayer(layer, node);

		if (node.TryGetNode("Smoothing", out FbxNode smooth))
		{
			layer.Smoothing.AddRange(toArr<int>(getArrayValue(version, smooth)));
		}

		return layer;
	}

	private LayerElement BuildLayerElementMaterial(FbxVersion version, FbxNode node)
	{
		LayerElementMaterial layer = new LayerElementMaterial();

		mapCommonLayer(layer, node);

		if (node.TryGetNode("Materials", out FbxNode materials))
		{
			layer.Indexes.AddRange(toArr<int>(getArrayValue(version, materials)));
		}

		return layer;
	}

	protected IEnumerable<T> toArr<T>(IEnumerable arr)
	{
		foreach (var item in arr)
		{
			yield return (T)Convert.ChangeType(item, typeof(T));
		}
	}

	protected double[] arrToDoubleArray(IEnumerable arr)
	{
		List<double> converted = new List<double>();
		foreach (object item in arr)
		{
			converted.Add(Convert.ToDouble(item));
		}

		return converted.ToArray();
	}

	protected List<XY> arrToXY(double[] arr)
	{
		List<XY> xy = new List<XY>();

		//Check for null value
		if (arr == null || arr.Length == 1)
			return xy;

		if (arr.Length % 2 != 0)
			throw new ArgumentOutOfRangeException("2D point array with odd length");

		//Create the vertices
		for (int i = 1; i < arr.Length; i += 2)
		{
			XY v = new XY(arr[i - 1], arr[i]);
			xy.Add(v);
		}

		return xy;
	}

	protected List<XYZ> arrToXYZ(double[] arr)
	{
		List<XYZ> xyz = new List<XYZ>();

		if (arr == null || arr.Length == 1)
			return xyz;

		if (arr.Length % 3 != 0)
			throw new ArgumentOutOfRangeException("3D point array length is not multiple of 3");

		for (int i = 2; i < arr.Length; i += 3)
		{
			XYZ v = new XYZ(arr[i - 2], arr[i - 1], arr[i]);
			xyz.Add(v);
		}

		return xyz;
	}
}
