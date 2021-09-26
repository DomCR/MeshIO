using MeshIO.Elements;
using MeshIO.Elements.Geometries;
using MeshIO.Elements.Geometries.Layers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshIO.FBX.Converters
{
	public abstract class FbxConverterBase : IFbxConverter
	{
		public static IFbxConverter GetConverter(Scene scene, FbxVersion version)
		{
			IFbxConverter converter = null;

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
				case FbxVersion.v6000:
				case FbxVersion.v6100:
					throw new NotImplementedException($"Incompatible version {version}");
				case FbxVersion.v7000:
				case FbxVersion.v7100:
				case FbxVersion.v7200:
				case FbxVersion.v7300:
				case FbxVersion.v7400:
					converter = new FbxConverter7400(scene);
					break;
				case FbxVersion.v7500:
				case FbxVersion.v7600:
				case FbxVersion.v7700:
					converter = new FbxConverter7400(scene);
					break;
				default:
					throw new NotImplementedException($"Incompatible version {version}");
			}

			//TODO: check the versions differences to implement the missing converters

			return converter;
		}

		public FbxVersion Version { get; }

		private FbxRootNode _root;
		private FbxNode _references;
		private FbxNode _definitions;
		private FbxNode _objects;
		private FbxNode _connections;
		private readonly Scene _scene;

		public FbxConverterBase(Scene scene, FbxVersion version)
		{
			this._scene = scene;
			this.Version = version;
		}

		public FbxRootNode ToRootNode()
		{
			_root = new FbxRootNode();
			_root.Version = this.Version;

			_root.Nodes.Add(new FBXHeader(this.Version).ToNode());
			_root.Nodes.Add(new GlobalSettings(this.Version).ToNode());
			_root.Nodes.Add(buildDocumentsNode());

			buildReferncesNode();
			buildDefinitionsNode();
			buildConnections();
			buildObjectsNode();

			addDefinition("GlobalSettings");

			_root.Nodes.Add(_references);
			_root.Nodes.Add(_definitions);
			_root.Nodes.Add(_objects);
			_root.Nodes.Add(_connections);

			return _root;
		}

		protected FbxNode buildDocumentsNode()
		{
			FbxNode node = new FbxNode("Documents");

			node.Nodes.Add(new FbxNode("Count", 1));

			FbxNode doc = new FbxNode("Document", Utils.CreateId(), "", "Scene");

			FbxNode properties = new FbxNode("Properties70");
			properties.Nodes.Add(new FbxNode("P", "SourceObject", "object", "", ""));
			properties.Nodes.Add(new FbxNode("P", "ActiveAnimStackName", "KString", "", "", ""));

			doc.Nodes.Add(properties);
			doc.Nodes.Add(new FbxNode("RootNode", 0));

			node.Nodes.Add(doc);

			return node;
		}

		protected void buildReferncesNode()
		{
			_references = new FbxNode("References");
		}

		protected void buildDefinitionsNode()
		{
			_definitions = new FbxNode("Definitions");

			_definitions.Nodes.Add(new FbxNode("Version", 100));
			_definitions.Nodes.Add(new FbxNode("Count", 0));
		}

		protected void addDefinition(string name)
		{
			bool found = false;
			foreach (var item in _definitions.Where(n => n.Name == "ObjectType"))
			{
				if (item.Properties[0].ToString() == name)
				{
					item["Count"].Value = Convert.ToInt32(item["Count"].Value) + 1;
					found = true;
				}
			}

			if (!found)
			{
				FbxNode def = new FbxNode("ObjectType", name);
				def.Nodes.Add(new FbxNode("Count", 1));

				_definitions.Nodes.Add(def);
			}

			_definitions["Count"].Value = Convert.ToInt32(_definitions["Count"].Value) + 1;
		}

		protected void buildConnections()
		{
			_connections = new FbxNode("Connections");
		}

		protected void addConnection(string type, ulong element, ulong container)
		{
			_connections.Nodes.Add(new FbxNode("C", type, element, container));
		}

		protected void buildObjectsNode()
		{
			_objects = new FbxNode("Objects");

			foreach (Element item in this._scene.Nodes)
			{
				FbxNode c = buildElementNode(item);

				if (c == null)
					continue;

				addConnection("OO", item._id.Value, 0);
			}
		}

		protected FbxNode buildElementNode(Element element)
		{
			FbxNode node = null;
			FbxNode properties = buildProperties(element.Properties);

			switch (element)
			{
				case Node n:
					node = buildModel(n, properties);
					break;
				case Material m:
					node = buildMaterial(m, properties);
					break;
				case Mesh mesh:
					node = buildMesh(mesh);
					break;
				case Camera camera:
					//node = buildCamera();
					break;
				default:
					System.Diagnostics.Debug.Fail($"{element.GetType().Name}");
					break;
			}

			if (node == null)
				return node;

			node.Nodes.Add(properties);

			_objects.Nodes.Add(node);
			addDefinition(node.Name);

			return node;
		}

		protected FbxNode buildModel(Node n, FbxNode properties)
		{
			FbxNode node = new FbxNode("Model", n._id, $"Model::{n.Name}", "Null");
			node.Nodes.Add(new FbxNode("Version", 232));

			if (n.MultiLayer.HasValue)
				node.Nodes.Add(new FbxNode("MultiLayer", n.MultiLayer.Value ? 'T' : 'F'));
			if (n.MultiTake.HasValue)
				node.Nodes.Add(new FbxNode("MultiTake", n.MultiTake.Value ? 'T' : 'F'));

			node.Nodes.Add(new FbxNode("Shading", n.Shading ? 'T' : 'F'));
			node.Nodes.Add(new FbxNode("Culling", n.Culling));

			foreach (Element item in n.Children)
			{
				buildElementNode(item);

				if (item == null)
					continue;

				addConnection("OO", item._id.Value, n._id.Value);
			}

			properties.Nodes.Add(buildProperty("Lcl Translation", n.Transform.Translation / n.Transform.Scale));
			properties.Nodes.Add(buildProperty("Lcl Scaling", n.Transform.Scale));

			return node;
		}

		protected FbxNode buildMaterial(Material n, FbxNode properties)
		{
			FbxNode node = new FbxNode("Material", n._id, $"Material::{n.Name}", "Null");
			node.Nodes.Add(new FbxNode("Version", 102));

			if (n.MultiLayer.HasValue)
				node.Nodes.Add(new FbxNode("MultiLayer", n.MultiLayer.Value));

			if (!string.IsNullOrEmpty(n.ShadingModel))
				node.Nodes.Add(new FbxNode("ShadingModel", n.ShadingModel));

			properties.Nodes.Add(buildProperty("AmbientColor", n.AmbientColor));
			properties.Nodes.Add(buildProperty("DiffuseColor", n.DiffuseColor));
			properties.Nodes.Add(buildProperty("SpecularColor", n.SpecularColor));
			properties.Nodes.Add(buildProperty("SpecularFactor", n.SpecularFactor));
			properties.Nodes.Add(buildProperty("ShininessExponent", n.ShininessExponent));
			properties.Nodes.Add(buildProperty("TransparencyFactor", n.TransparencyFactor));
			properties.Nodes.Add(buildProperty("EmissiveColor", n.EmissiveColor));
			properties.Nodes.Add(buildProperty("EmissiveFactor", n.EmissiveFactor));

			return node;
		}

		protected FbxNode buildMesh(Mesh mesh)
		{
			FbxNode node = new FbxNode("Geometry", mesh._id, $"Geometry::{mesh.Name}", "Mesh");
			node.Nodes.Add(new FbxNode("GeometryVersion", 124));

			node.Nodes.Add(new FbxNode("Vertices", mesh.Vertices.SelectMany(x => x.GetComponents()).ToArray()));
			node.Nodes.Add(new FbxNode("PolygonVertexIndex", polygonsArray(mesh)));

			buildMeshLayers(node, mesh.Layers);

			return node;
		}

		private double[] xyToArrayDouble(IEnumerable<XY> xy)
		{
			List<double> arr = new List<double>();

			foreach (XY v in xy)
			{
				arr.Add(v.X);
				arr.Add(v.Y);
			}

			return arr.ToArray();
		}

		private double[] xyzToArrayDouble(IEnumerable<XYZ> xyz)
		{
			List<double> arr = new List<double>();

			foreach (XYZ v in xyz)
			{
				arr.Add(v.X);
				arr.Add(v.Y);
				arr.Add(v.Z);
			}

			return arr.ToArray();
		}

		protected int[] polygonsArray(Mesh mesh)
		{
			List<int> arr = new List<int>();

			//Check if the polygons list is empty
			if (!mesh.Polygons.Any())
				return arr.ToArray();

			if (mesh.Polygons.First() is Triangle)
			{
				foreach (Triangle t in mesh.Polygons)
				{
					arr.Add((int)t.Index0);
					arr.Add((int)t.Index1);
					arr.Add(-((int)t.Index2 + 1));
				}
			}
			else
			{
				foreach (Quad t in mesh.Polygons)
				{
					arr.Add((int)t.Index0);
					arr.Add((int)t.Index1);
					arr.Add((int)t.Index2);
					arr.Add(-((int)t.Index3 + 1));
				}
			}

			return arr.ToArray();
		}

		protected void buildMeshLayers(FbxNode parent, IEnumerable<LayerElement> layers)
		{
			FbxNode layer = new FbxNode("Layer", 0);
			layer.Nodes.Add(new FbxNode("Version", 100));
			parent.Nodes.Add(layer);

			foreach (var item in layers)
			{
				FbxNode layerType = null;

				switch (item)
				{
					case LayerElementMaterial layerElement:
						layerType = buildLayerElementMaterial(layerElement);
						break;
					case LayerElementPolygonGroup layerElement:
						layerType = buildLayerElementPolygonGroup(layerElement);
						break;
					case LayerElementBinormal layerElement:
						layerType = buildLayerElementBinormal(layerElement);
						break;
					case LayerElementUV layerElement:
						layerType = buildLayerElementUV(layerElement);
						break;
					case LayerElementSmoothing layerElement:
						//layerType = buildElementSmoothing(layerElement);
						break;
					case LayerElementTangent layerElement:
						layerType = buildLayerElementTangent(layerElement);
						break;
					case LayerElementNormal layerElement:
						layerType = buildLayerElementNormal(layerElement);
						break;
					case LayerElementVertexColor _:
					case LayerElementVertexCrease _:
					case LayerElementEdgeCrease _:
					case LayerElementUserData _:
					case LayerElementVisibility _:
					case LayerElementSpecular _:
					case LayerElementWeight _:
					case LayerElementHole _:
						break;
					default:
						System.Diagnostics.Debug.Fail($"{item.GetType().Name}");
						break;
				}

				if (layerType == null)
					continue;

				FbxNode type = new FbxNode("LayerElement");
				type.Nodes.Add(new FbxNode("Type", layerType.Name));
				type.Nodes.Add(new FbxNode("TypedIndex", 0));

				layer.Nodes.Add(type);
				parent.Nodes.Add(layerType);
			}
		}

		public void buildLayerElement(FbxNode node, LayerElement layer)
		{
			node.Nodes.Add(new FbxNode("Name", layer.Name));
			node.Nodes.Add(new FbxNode("MappingInformationType", layer.MappingInformationType.ToString()));
			node.Nodes.Add(new FbxNode("ReferenceInformationType", layer.ReferenceInformationType.ToString()));
		}

		public FbxNode buildLayerElementMaterial(LayerElementMaterial layer)
		{
			FbxNode node = new FbxNode("LayerElementMaterial", 0);
			node.Nodes.Add(new FbxNode("Version", 101));
			buildLayerElement(node, layer);
			node.Nodes.Add(new FbxNode("Materials", layer.Materials.ToArray()));
			return node;
		}

		public FbxNode buildLayerElementPolygonGroup(LayerElementPolygonGroup layer)
		{
			FbxNode node = new FbxNode("LayerElementPolygonGroup", 0);
			node.Nodes.Add(new FbxNode("Version", 101));
			buildLayerElement(node, layer);
			//node.Nodes.Add(new FbxNode("Materials", layer..ToArray()));
			return node;
		}

		public FbxNode buildLayerElementBinormal(LayerElementBinormal layer)
		{
			FbxNode node = new FbxNode("LayerElementBinormal", 0);
			node.Nodes.Add(new FbxNode("Version", 101));
			buildLayerElement(node, layer);
			node.Nodes.Add(new FbxNode("BiNormals", layer.BiNormals.SelectMany(x => x.GetComponents()).ToArray()));
			return node;
		}

		public FbxNode buildLayerElementUV(LayerElementUV layer)
		{
			FbxNode node = new FbxNode("LayerElementUV", 0);
			node.Nodes.Add(new FbxNode("Version", 101));
			buildLayerElement(node, layer);
			node.Nodes.Add(new FbxNode("UV", layer.UV.SelectMany(x => x.GetComponents()).ToArray()));
			node.Nodes.Add(new FbxNode("UVIndex", layer.UVIndex.ToArray()));
			return node;
		}

		public FbxNode buildLayerElementTangent(LayerElementTangent layer)
		{
			FbxNode node = new FbxNode("LayerElementTangent", 0);
			node.Nodes.Add(new FbxNode("Version", 102));
			buildLayerElement(node, layer);
			node.Nodes.Add(new FbxNode("Tangents", layer.Tangents.SelectMany(x => x.GetComponents()).ToArray()));
			return node;
		}

		public FbxNode buildLayerElementNormal(LayerElementNormal layer)
		{
			FbxNode node = new FbxNode("LayerElementNormal", 0);
			node.Nodes.Add(new FbxNode("Version", 102));
			buildLayerElement(node, layer);
			node.Nodes.Add(new FbxNode("Normals", layer.Normals.SelectMany(x => x.GetComponents()).ToArray()));
			return node;
		}

		private FbxNode buildProperties(PropertyCollection properties)
		{
			FbxNode node = new FbxNode("Properties70");

			foreach (Property p in properties)
			{
				FbxNode pn = buildProperty(p);

				if (pn != null)
					node.Nodes.Add(pn);
			}

			return node;
		}

		private FbxNode buildProperty(Property property)
		{
			switch (property)
			{
				case FbxProperty:
				case Property:
				default:
					break;
			}

			return buildProperty(property.Name, property.Value);
		}

		private FbxNode buildProperty(string name, object propValue)
		{
			FbxNode node = new FbxNode("P");
			node.Properties.Add(name);

			switch (propValue)
			{
				case string value:
					node.Properties.Add("KString");
					node.Properties.Add("");
					node.Properties.Add("");
					node.Properties.Add(value);
					break;
				case Color value:
					if (value.A.HasValue)
					{
						node.Properties.Add("ColorRGB");
						node.Properties.Add("Color");
						node.Properties.Add("");
						node.Properties.Add(value.R / (double)255);
						node.Properties.Add(value.G / (double)255);
						node.Properties.Add(value.B / (double)255);
						node.Properties.Add(value.A / (double)255);
					}
					else
					{
						node.Properties.Add("ColorAndAlpha");
						node.Properties.Add("");
						node.Properties.Add("A");   //TODO: Fix the fbx property flags
						node.Properties.Add(value.R / (double)255);
						node.Properties.Add(value.G / (double)255);
						node.Properties.Add(value.B / (double)255);
					}
					break;
				case double value:
					node.Properties.Add("double");
					node.Properties.Add("Number");
					node.Properties.Add("");
					node.Properties.Add(value);
					break;
				case int value:
					node.Properties.Add("int");
					node.Properties.Add("Integer");
					node.Properties.Add("");
					node.Properties.Add(value);
					break;
				case float value:
					node.Properties.Add("Float");
					node.Properties.Add("");
					node.Properties.Add("A");
					node.Properties.Add(value);
					break;
				case bool value:
					node.Properties.Add("bool");
					node.Properties.Add("");
					node.Properties.Add("");
					node.Properties.Add(value ? 1 : 0);
					break;
				case XYZ value:
					node.Properties.Add("Vector3D");
					node.Properties.Add("Vector");
					node.Properties.Add("");
					node.Properties.Add(value.X);
					node.Properties.Add(value.Y);
					node.Properties.Add(value.Z);
					break;
				case null:
					node.Properties.Add("");
					node.Properties.Add("");
					node.Properties.Add("");
					break;
				default:
					System.Diagnostics.Debug.Fail($"{propValue.GetType().FullName}");
					break;
			}

			return node;
		}

		internal class FBXHeader
		{
			public FbxVersion Version { get; set; }
			public DateTime CreationTime { get; set; }
			public string Creator { get { return "MeshIO.FBX"; } }
			public int HeaderVersion { get; }

			public FBXHeader(FbxVersion version)
			{
				this.Version = version;
				CreationTime = DateTime.Now;

				switch (Version)
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
					case FbxVersion.v6000:
					case FbxVersion.v6100:
					case FbxVersion.v7000:
					case FbxVersion.v7100:
					case FbxVersion.v7200:
					case FbxVersion.v7300:
					case FbxVersion.v7400:
					case FbxVersion.v7500:
					case FbxVersion.v7600:
					case FbxVersion.v7700:
						HeaderVersion = 1003;
						break;
					default:
						break;
				}
			}

			public FbxNode ToNode()
			{
				FbxNode node = new FbxNode("FBXHeaderVersion");

				node.Nodes.Add(new FbxNode("Creator", Creator));
				node.Nodes.Add(new FbxNode("FBXVersion", (int)Version));
				node.Nodes.Add(new FbxNode("FBXHeaderVersion", HeaderVersion));

				FbxNode tiemespan = new FbxNode("CreationTimeStamp");
				tiemespan.Nodes.Add(new FbxNode("Version", 1000));
				tiemespan.Nodes.Add(new FbxNode("Year", CreationTime.Year));
				tiemespan.Nodes.Add(new FbxNode("Month", CreationTime.Month));
				tiemespan.Nodes.Add(new FbxNode("Day", CreationTime.Day));
				tiemespan.Nodes.Add(new FbxNode("Hour", CreationTime.Hour));
				tiemespan.Nodes.Add(new FbxNode("Minute", CreationTime.Minute));
				tiemespan.Nodes.Add(new FbxNode("Second", CreationTime.Second));
				tiemespan.Nodes.Add(new FbxNode("Millisecond", CreationTime.Millisecond));

				node.Nodes.Add(tiemespan);

				return node;
			}
		}

		internal class GlobalSettings
		{
			public int Version { get; set; }

			public GlobalSettings(FbxVersion version)
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
					case FbxVersion.v6000:
					case FbxVersion.v6100:
					case FbxVersion.v7000:
					case FbxVersion.v7100:
					case FbxVersion.v7200:
					case FbxVersion.v7300:
					case FbxVersion.v7400:
					case FbxVersion.v7500:
					case FbxVersion.v7600:
					case FbxVersion.v7700:
						Version = 1000;
						break;
					default:
						break;
				}
			}

			public FbxNode ToNode()
			{
				FbxNode node = new FbxNode("GlobalSettings");

				node.Nodes.Add(new FbxNode("Version", (int)Version));

				FbxNode properties = new FbxNode("Properties70");

				node.Nodes.Add(properties);

				properties.Nodes.Add(new FbxNode("P", "UpAxis", "int", "Integer", "", 1));
				properties.Nodes.Add(new FbxNode("P", "UpAxisSign", "int", "Integer", "", 1));
				properties.Nodes.Add(new FbxNode("P", "FrontAxis", "int", "Integer", "", 2));
				properties.Nodes.Add(new FbxNode("P", "FrontAxisSign", "int", "Integer", "", 1));
				properties.Nodes.Add(new FbxNode("P", "CoordAxis", "int", "Integer", "", 0));
				properties.Nodes.Add(new FbxNode("P", "CoordAxisSign", "int", "Integer", "", 1));
				properties.Nodes.Add(new FbxNode("P", "OriginalUpAxis", "int", "Integer", "", 2));
				properties.Nodes.Add(new FbxNode("P", "OriginalUpAxisSign", "int", "Integer", "", 1));
				properties.Nodes.Add(new FbxNode("P", "UnitScaleFactor", "double", "Number", "", 100000));
				properties.Nodes.Add(new FbxNode("P", "OriginalUnitScaleFactor", "double", "Number", "", 100));
				properties.Nodes.Add(new FbxNode("P", "AmbientColor", "ColorRGB", "Color", "", 0, 0, 0));
				properties.Nodes.Add(new FbxNode("P", "DefaultCamera", "KString", "", "", "Producer"));
				properties.Nodes.Add(new FbxNode("P", "TimeMode", "enum", "", "", 6));
				properties.Nodes.Add(new FbxNode("P", "TimeProtocol", "enum", "", "", 2));
				properties.Nodes.Add(new FbxNode("P", "SnapOnFrameMode", "enum", "", "", 0));
				properties.Nodes.Add(new FbxNode("P", "TimeSpanStart", "KTime", "Time", "", 0));
				properties.Nodes.Add(new FbxNode("P", "TimeSpanStop", "KTime", "Time", "", 153953860));
				properties.Nodes.Add(new FbxNode("P", "CustomFrameRate", "double", "Number", "", -1));
				properties.Nodes.Add(new FbxNode("P", "TimeMarker", "Compound", "", ""));
				properties.Nodes.Add(new FbxNode("P", "CurrentTimeMarker", "int", "Integer", "", -1));

				return node;
			}
		}
	}
}
