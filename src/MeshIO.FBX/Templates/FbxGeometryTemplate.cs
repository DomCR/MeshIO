#if NETFRAMEWORK
using CSUtilities.Extensions;
#endif
using CSMath;
using MeshIO.Entities.Geometries;
using MeshIO.Entities.Geometries.Layers;
using MeshIO.FBX.Extensions;
using MeshIO.FBX.Readers;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MeshIO.FBX.Templates
{
	internal abstract class FbxGeometryTemplate<T> : FbxObjectTemplate<T>
		where T : Geometry
	{
		public override string FbxObjectName { get { return FbxFileToken.Geometry; } }

		protected FbxGeometryTemplate(T geometry) : base(geometry) { }

		protected FbxGeometryTemplate(FbxNode node, T geometry) : base(node, geometry) { }

		public override void Build(FbxFileBuilderBase builder)
		{
			base.Build(builder);

			readLayers();
		}

		protected override void addProperties(Dictionary<string, FbxProperty> properties)
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

			base.addProperties(properties);
		}

		protected void readLayers()
		{
			FbxNode node = FbxNode;
			var geometry = _element;

			if (node.TryGetNode("LayerElementNormal", out FbxNode layerElementNormal))
			{
				geometry.Layers.Add(mapLayerElementNormal(layerElementNormal));
			}

			if (node.TryGetNode("LayerElementBinormal", out FbxNode layerElementBinormal))
			{
				geometry.Layers.Add(mapLayerElementBinormal(layerElementBinormal));
			}

			if (node.TryGetNode("LayerElementTangent", out FbxNode layerElementTangent))
			{
				geometry.Layers.Add(mapLayerElementTangent(layerElementTangent));
			}

			if (node.TryGetNode("LayerElementMaterial", out FbxNode layerElementMaterial))
			{
				geometry.Layers.Add(BuildLayerElementMaterial(layerElementMaterial));
			}

			if (node.TryGetNode("LayerElementUV", out FbxNode layerElementUV))
			{
				geometry.Layers.Add(mapLayerElementUV(layerElementUV));
			}

			if (node.TryGetNode("LayerElementSmoothing", out FbxNode layerElementSmoothing))
			{
				geometry.Layers.Add(mapLayerElementSmoothing(layerElementSmoothing));
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

		private LayerElement mapLayerElementNormal(FbxNode node)
		{
			LayerElementNormal layer = new LayerElementNormal();

			mapCommonLayer(layer, node);

			if (node.TryGetNode("Normals", out FbxNode normals))
			{
				layer.Normals = arrToXYZ(arrToDoubleArray(normals.Value as IEnumerable));
			}

			if (node.TryGetNode("NormalsW", out FbxNode normalsw))
			{
				layer.Weights.AddRange(arrToDoubleArray(normalsw.Value as IEnumerable));
			}

			return layer;
		}

		private LayerElement mapLayerElementBinormal(FbxNode node)
		{
			LayerElementBinormal layer = new LayerElementBinormal();

			mapCommonLayer(layer, node);

			if (node.TryGetNode("Binormals", out FbxNode normals))
			{
				layer.Normals = arrToXYZ(arrToDoubleArray(normals.Value as IEnumerable));
			}

			if (node.TryGetNode("BinormalsW", out FbxNode normalsw))
			{
				layer.Weights.AddRange(arrToDoubleArray(normalsw.Value as IEnumerable));
			}

			return layer;
		}

		private LayerElement mapLayerElementTangent(FbxNode node)
		{
			LayerElementTangent layer = new LayerElementTangent();

			mapCommonLayer(layer, node);

			if (node.TryGetNode("Tangents", out FbxNode normals))
			{
				layer.Tangents = arrToXYZ(arrToDoubleArray(normals.Value as IEnumerable));
			}

			if (node.TryGetNode("TangentsW", out FbxNode normalsw))
			{
				layer.Weights.AddRange(arrToDoubleArray(normalsw.Value as IEnumerable));
			}

			return layer;
		}

		private LayerElement mapLayerElementUV(FbxNode node)
		{
			LayerElementUV layer = new LayerElementUV();

			mapCommonLayer(layer, node);

			if (node.TryGetNode("UV", out FbxNode uv))
			{
				layer.UV = arrToXY(arrToDoubleArray(uv.Value as IEnumerable));
			}

			if (node.TryGetNode("UVIndex", out FbxNode indices))
			{
				layer.Indexes.AddRange(toArr<int>(indices.Value as IEnumerable));
			}

			return layer;
		}

		private LayerElement mapLayerElementSmoothing(FbxNode node)
		{
			LayerElementSmoothing layer = new LayerElementSmoothing();

			mapCommonLayer(layer, node);

			if (node.TryGetNode("Smoothing", out FbxNode smooth))
			{
				layer.Smoothing.AddRange(toArr<int>(smooth.Value as IEnumerable));
			}

			return layer;
		}

		private LayerElement BuildLayerElementMaterial(FbxNode node)
		{
			LayerElementMaterial layer = new LayerElementMaterial();

			mapCommonLayer(layer, node);

			if (node.TryGetNode("Materials", out FbxNode materials))
			{
				layer.Indexes.AddRange(toArr<int>(materials.Value as IEnumerable));
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
}
