using CSMath;
using MeshIO.Entities.Geometries;
using MeshIO.Entities.Geometries.Layers;
using MeshIO.FBX.Exceptions;
using MeshIO.Shaders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MeshIO.FBX.Converters.Mappers
{
	public class FbxObjectsMapper : FbxMapperBase
	{
		public const string TokenModel = "Model";

		public const string TokenGeometry = "Geometry";

		public const string TokenMaterial = "Material";

		public const string TokenProperties = "Properties70";

		public const string PrefixSeparator = "::";

		public override string SectionName { get { return "Objects"; } }

		public FbxNode ObjectsNode { get; private set; }

		public FbxDefinitionsMapper DefinitionMap { get; set; } = new FbxDefinitionsMapper();

		public Dictionary<ulong, Element3D> ObjectMap { get; } = new Dictionary<ulong, Element3D>();

		public override void Map(FbxNode node)
		{
			base.Map(node);

			this.ObjectsNode = node;
		}

		public void MapElements(FbxDefinitionsMapper definitions = null)
		{
			if (definitions != null)
			{
				this.DefinitionMap = definitions;
			}

			foreach (FbxNode n in this.ObjectsNode)
			{
				Element3D element = null;

				switch (n.Name)
				{
					case TokenModel:
						element = this.mapModel(n);
						break;
					case TokenGeometry:
						element = this.mapGeometry(n);
						break;
					case TokenMaterial:
						element = this.mapMaterial(n);
						break;
					default:
						this.notify($"Unknown fbx object node with name : {n.Name}", Core.NotificationType.NotImplemented);
						continue;
				}

				if (element == null)
					continue;

				this.ObjectMap.Add(element.Id.Value, element);
			}
		}

		private void mapCommon(Element3D element, FbxNode node, string prefix)
		{
			element.Id = Convert.ToUInt64(node.Properties[0]);

			string name = node.Properties[1].ToString();
			if (name.StartsWith(prefix))
				name = name.Remove(0, prefix.Length);

			element.Name = name;
		}

		private IEnumerable<Property> mergeProperties(string token, FbxNode node)
		{
			return this.MapProperties(node);

			Dictionary<string, Property> result = new Dictionary<string, Property>();

			var properties = this.MapProperties(node);
			var definitions = this.DefinitionMap.GetDefinitions(token);

			return result.Select(r => r.Value);
		}

		private Element3D mapModel(FbxNode node)
		{
			Node model = new Node();

			this.mapCommon(model, node, $"{TokenModel}{PrefixSeparator}");

			foreach (var p in this.mergeProperties(TokenModel, node))
			{
				switch (p.Name)
				{
					case FbxProperty.LclRotation:
						model.Transform.Rotation = (XYZ)p.Value;
						continue;
					case FbxProperty.LclScaling:
						model.Transform.Scale = (XYZ)p.Value;
						continue;
					case FbxProperty.LclTranslation:
						model.Transform.Translation = (XYZ)p.Value;
						continue;
					case FbxProperty.Show:
						model.IsVisible = (bool)p.Value;
						continue;
				}

				model.Properties.Add(p);
			}

			return model;
		}

		private Element3D mapMaterial(FbxNode node)
		{
			Material material = new Material();

			this.mapCommon(material, node, $"{TokenMaterial}{PrefixSeparator}");

			if (node.TryGetNode("ShadingModel", out FbxNode ShadingModel))
			{
				material.ShadingModel = ShadingModel.Value.ToString();
			}

			foreach (var p in this.mergeProperties(TokenModel, node))
			{
				switch (p.Name)
				{
					//TODO: finish material read
					case FbxProperty.AmbientColor:
						material.AmbientColor = (Color)p.Value;
						continue;
				}

				material.Properties.Add(p);
			}

			return material;
		}

		private Element3D mapGeometry(FbxNode node)
		{
			Geometry geometry = null;

			switch (node.Properties[2].ToString())
			{
				case "Mesh":
					geometry = this.mapMesh(node);
					break;
				default:
					this.notify($"Unknow geometry type with name {node.Properties[2]}");
					return null;
			}

			this.mapCommon(geometry, node, $"{TokenGeometry}{PrefixSeparator}");

			foreach (var p in this.mergeProperties(TokenModel, node))
			{
				switch (p.Name)
				{
					case FbxProperty.PrimaryVisibility:
						geometry.IsVisible = (bool)p.Value;
						continue;
					case FbxProperty.CastShadows:
						geometry.CastShadows = (bool)p.Value;
						continue;
					case FbxProperty.ReceiveShadows:
						geometry.ReceiveShadows = (bool)p.Value;
						continue;
				}

				geometry.Properties.Add(p);
			}

			return geometry;
		}

		private Geometry mapMesh(FbxNode node)
		{
			Mesh mesh = new Mesh();

			//General properties
			if (node.TryGetNode("Vertices", out FbxNode vertices))
			{
				mesh.Vertices = this.arrToXYZ(vertices.Value as double[]);
			}
			if (node.TryGetNode("Edges", out FbxNode edges))
			{
				mesh.Edges.AddRange(this.toArr<int>(edges.Value as IEnumerable));
			}
			if (node.TryGetNode("PolygonVertexIndex", out FbxNode polygons))
			{
				mesh.Polygons = this.mapPolygons(polygons.Value as int[]);
			}

			//Mesh layers
			this.mapGeometryLayers(mesh, node);

			return mesh;
		}

		private void mapGeometryLayers(Geometry geometry, FbxNode node)
		{
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
				if (Enum.TryParse<MappingMode>((string)mappingInformationType.Value, out MappingMode mappingMode))
				{
					layer.MappingMode = mappingMode;
				}
				else
				{
					this.notify($"Could not parse MappingMode | {mappingInformationType.Value}", Core.NotificationType.Warning);
				}
			}

			if (node.TryGetNode("ReferenceInformationType", out FbxNode referenceInformationType))
			{
				if (Enum.TryParse<ReferenceMode>((string)referenceInformationType.Value, out ReferenceMode referenceMode))
				{
					layer.ReferenceMode = referenceMode;
				}
				else
				{
					this.notify($"Could not parse MappingMode | {referenceInformationType.Value}", Core.NotificationType.Warning);
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

			if (node.TryGetNode("Normals", out FbxNode normalsw))
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
				layer.Indices.AddRange(this.toArr<int>(indices.Value as IEnumerable));
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
				layer.Indices.AddRange(this.toArr<int>(materials.Value as IEnumerable));
			}

			return layer;
		}

		protected double[] arrToDoubleArray(IEnumerable arr)
		{
			List<double> converted = new List<double>();
			foreach (var item in arr)
			{
				converted.Add(Convert.ToDouble(item));
			}

			return converted.ToArray();
		}

		protected List<Polygon> mapPolygons(int[] arr)
		{
			List<Polygon> Polygons = new List<Polygon>();

			if (arr == null)
				return Polygons;

			//Check if the arr are faces or quads
			if (arr[2] < 0)
			{
				for (int i = 2; i < arr.Length; i += 3)
				{
					Triangle tmp = new Triangle(
						arr[i - 2],
						arr[i - 1],
						//Substract a unit to the last
						(Math.Abs(arr[i])) - 1);

					Polygons.Add(tmp);
				}
			}

			//Quads
			else if (arr[3] < 0)
			{
				for (int i = 3; i < arr.Length; i += 4)
				{
					Quad tmp = new Quad(
						Math.Abs(arr[i - 3]),
						Math.Abs(arr[i - 2]),
						Math.Abs(arr[i - 1]),
						//Substract a unit to the last
						Math.Abs(arr[i]) - 1);

					Polygons.Add(tmp);
				}
			}

			return Polygons;
		}

		protected IEnumerable<T> toArr<T>(IEnumerable arr)
		{
			foreach (var item in arr)
			{
				yield return (T)Convert.ChangeType(item, typeof(T));
			}
		}

		protected List<XY> arrToXY(double[] arr)
		{
			List<XY> xy = new List<XY>();

			//Check for null value
			if (arr == null || arr.Length == 1)
				return xy;

			if (arr.Length % 2 != 0)
				throw new FbxConverterException("2D point array with odd length");

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

			//Check for null value
			if (arr == null || arr.Length == 1)
				return xyz;

			if (arr.Length % 3 != 0)
				throw new FbxConverterException("3D point array length is not multiple of 3");

			//Create the vertices
			for (int i = 2; i < arr.Length; i += 3)
			{
				XYZ v = new XYZ(arr[i - 2], arr[i - 1], arr[i]);
				xyz.Add(v);
			}

			return xyz;
		}
	}
}
