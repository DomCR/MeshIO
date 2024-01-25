#if NETFRAMEWORK
using CSUtilities.Extensions;
#endif
using CSMath;
using MeshIO.Entities.Geometries;
using MeshIO.Entities.Geometries.Layers;
using MeshIO.FBX.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MeshIO.FBX.Readers.Templates
{
	internal abstract class FbxGeometryTemplate<T> : FbxObjectTemplate<T>
		where T : Geometry
	{
		public override string FbxObjectName { get { return FbxFileToken.Geometry; } }

		protected FbxGeometryTemplate(FbxNode node, T geometry) : base(node, geometry)
		{
		}

		public override void Build(FbxFileBuilderBase builder)
		{
			base.Build(builder);

			this.readLayers();
		}

		protected override void addProperties(Dictionary<string, FbxProperty> properties)
		{
			if (properties.Remove("Primary Visibility", out FbxProperty isVisible))
			{
				this.Element.IsVisible = (bool)isVisible.ToProperty().Value;
			}

			if (properties.Remove("Casts Shadows", out FbxProperty castShadows))
			{
				this.Element.CastShadows = (bool)castShadows.ToProperty().Value;
			}

			if (properties.Remove("Receive Shadows", out FbxProperty receiveShadows))
			{
				this.Element.ReceiveShadows = (bool)receiveShadows.ToProperty().Value;
			}

			base.addProperties(properties);
		}

		protected void readLayers()
		{
			FbxNode node = this.FbxNode;
			var geometry = this.Element;

			if (node.TryGetNode("LayerElementNormal", out FbxNode layerElementNormal))
			{
				geometry.Layers.Add(this.mapLayerElementNormal(layerElementNormal));
			}

			if (node.TryGetNode("LayerElementBinormal", out FbxNode layerElementBinormal))
			{
				geometry.Layers.Add(this.mapLayerElementBinormal(layerElementBinormal));
			}

			if (node.TryGetNode("LayerElementTangent", out FbxNode layerElementTangent))
			{
				geometry.Layers.Add(this.mapLayerElementTangent(layerElementTangent));
			}

			if (node.TryGetNode("LayerElementMaterial", out FbxNode layerElementMaterial))
			{
				geometry.Layers.Add(this.BuildLayerElementMaterial(layerElementMaterial));
			}

			if (node.TryGetNode("LayerElementUV", out FbxNode layerElementUV))
			{
				geometry.Layers.Add(this.mapLayerElementUV(layerElementUV));
			}

			if (node.TryGetNode("LayerElementSmoothing", out FbxNode layerElementSmoothing))
			{
				geometry.Layers.Add(this.mapLayerElementSmoothing(layerElementSmoothing));
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

			this.mapCommonLayer(layer, node);

			if (node.TryGetNode("Normals", out FbxNode normals))
			{
				layer.Normals = this.arrToXYZ(this.arrToDoubleArray(normals.Value as IEnumerable));
			}

			if (node.TryGetNode("NormalsW", out FbxNode normalsw))
			{
				layer.Weights.AddRange(this.arrToDoubleArray(normalsw.Value as IEnumerable));
			}

			return layer;
		}

		private LayerElement mapLayerElementBinormal(FbxNode node)
		{
			LayerElementBinormal layer = new LayerElementBinormal();

			this.mapCommonLayer(layer, node);

			if (node.TryGetNode("Binormals", out FbxNode normals))
			{
				layer.Normals = this.arrToXYZ(this.arrToDoubleArray(normals.Value as IEnumerable));
			}

			if (node.TryGetNode("BinormalsW", out FbxNode normalsw))
			{
				layer.Weights.AddRange(this.arrToDoubleArray(normalsw.Value as IEnumerable));
			}

			return layer;
		}

		private LayerElement mapLayerElementTangent(FbxNode node)
		{
			LayerElementTangent layer = new LayerElementTangent();

			this.mapCommonLayer(layer, node);

			if (node.TryGetNode("Tangents", out FbxNode normals))
			{
				layer.Tangents = this.arrToXYZ(this.arrToDoubleArray(normals.Value as IEnumerable));
			}

			if (node.TryGetNode("TangentsW", out FbxNode normalsw))
			{
				layer.Weights.AddRange(this.arrToDoubleArray(normalsw.Value as IEnumerable));
			}

			return layer;
		}

		private LayerElement mapLayerElementUV(FbxNode node)
		{
			LayerElementUV layer = new LayerElementUV();

			this.mapCommonLayer(layer, node);

			if (node.TryGetNode("UV", out FbxNode uv))
			{
				layer.UV = this.arrToXY(this.arrToDoubleArray(uv.Value as IEnumerable));
			}

			if (node.TryGetNode("UVIndex", out FbxNode indices))
			{
				layer.Indexes.AddRange(this.toArr<int>(indices.Value as IEnumerable));
			}

			return layer;
		}

		private LayerElement mapLayerElementSmoothing(FbxNode node)
		{
			LayerElementSmoothing layer = new LayerElementSmoothing();

			this.mapCommonLayer(layer, node);

			if (node.TryGetNode("Smoothing", out FbxNode smooth))
			{
				layer.Smoothing.AddRange(this.toArr<int>(smooth.Value as IEnumerable));
			}

			return layer;
		}

		private LayerElement BuildLayerElementMaterial(FbxNode node)
		{
			LayerElementMaterial layer = new LayerElementMaterial();

			this.mapCommonLayer(layer, node);

			if (node.TryGetNode("Materials", out FbxNode materials))
			{
				layer.Indexes.AddRange(this.toArr<int>(materials.Value as IEnumerable));
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
