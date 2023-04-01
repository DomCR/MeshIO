using CSMath;
using MeshIO.Core;
using MeshIO.Entities.Geometries.Layers;
using MeshIO.Entities.Geometries;
using MeshIO.FBX.Exceptions;
using MeshIO.Shaders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MeshIO.FBX.Converters
{
    /// <summary>
    /// Base class to convert a node structure fbx <see cref="FbxRootNode"/> into a <see cref="Scene"/>
    /// </summary>
    public abstract class NodeConverterBase : INodeConverter
	{
		public event NotificationEventHandler OnNotification;

		public FbxVersion Version { get { return this._root.Version; } }

		public FbxNode SectionDocuments { get; set; }

		public FbxNode SectionObjects { get; set; }

		public FbxNode SectionConnections { get; set; }

		protected System.Text.RegularExpressions.Regex _propertiesRegex = new System.Text.RegularExpressions.Regex(@"(Properties).*?[\d]+");

		protected Dictionary<ulong, FbxNode> _objects = new Dictionary<ulong, FbxNode>();

		protected readonly FbxRootNode _root;

		public static INodeConverter GetConverter(FbxRootNode root)
		{
			INodeConverter converter = null;

			switch (root.Version)
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
					throw new NotSupportedException($"Fbx version {root.Version} not supported");
				case FbxVersion.v6000:
				case FbxVersion.v6100:
					throw new NotImplementedException($"Fbx version {root.Version} not implemented");
				case FbxVersion.v7000:
				case FbxVersion.v7100:
				case FbxVersion.v7200:
				case FbxVersion.v7300:
				case FbxVersion.v7400:
				case FbxVersion.v7500:
				case FbxVersion.v7600:
				case FbxVersion.v7700:
					converter = new NodeConverter7000(root);
					break;
				default:
					throw new Exception($"Unknown fbx version : {root.Version}");
			}

			//TODO: check the versions differences to implement the missing converters

			return converter;
		}

		public NodeConverterBase(FbxRootNode root)
		{
			this._root = root;

			this.checkFileSections();
		}

		public Scene ConvertScene()
		{
			Scene scene = this.buildScene(this.SectionDocuments);

			foreach (FbxNode n in this.SectionObjects.Nodes)
			{
				this._objects.Add(Convert.ToUInt64(n.Properties[0]), n);
			}

			foreach (FbxNode n in this.getChildren(scene.Id.Value))
			{
				Element3D element = this.ToElement(n);

				switch (element)
				{
					case Node fbxNode:
						scene.RootNode.Nodes.Add(fbxNode);
						break;
					default:
						this.notify($"Element3D is not a {typeof(Node).FullName} is a {element.GetType().FullName}");
						break;
				}
			}

			return scene;
		}

		public Element3D ToElement(FbxNode node)
		{
			Element3D element = null;

			switch (node.Name)
			{
				case "Model":
					element = this.BuildModel(node);
					break;
				case "Material":
					element = this.BuildMaterial(node);
					break;
				case "Geometry":
					element = this.BuildGeometryObject(node);
					break;
				//case "NodeAttribute":
				//	element = this.BuildNodeAttribute(node);
				//	break;
				//case "Object":
				//case "ObjectMetaData":  //TODO: Link data with model
				//	break;
				default:
					this.notify($"Unknown element node with name : {node.Name}");
					break;
			}

			return element;
		}

		public void BuildElement(FbxNode node, Element3D element, string prefix)
		{
			element.Id = Convert.ToUInt64(node.Properties[0]);

			string name = node.Properties[1].ToString();
			if (name.StartsWith(prefix))
				name = name.Remove(0, prefix.Length);

			element.Name = name;
		}

		public List<Property> BuildProperties(FbxNode node)
		{
			List<Property> properties = new List<Property>();

			foreach (FbxNode n in node.Nodes)
			{
				var p = this.BuildProperty(n);

				if (properties.Select(p => p.Name).Contains(p.Name))
				{
					this.notify($"Duplicated property with name : {p.Name}");
					continue;
				}

				properties.Add(p);
			}

			return properties;
		}

		public Property BuildProperty(FbxNode node)
		{
			//P : ["PropName", "PropType", "Label(?)", "Flags", __values__, …]
			Property property = null;

			string type1 = (string)node.Properties[1];
			string type2 = (string)node.Properties[2];
			string flags = (string)node.Properties[3];

			switch (type1)
			{
				case "Color":
				case "ColorRGB":
					byte r = (byte)(Convert.ToDouble(node.Properties[4]) * 255);
					byte g = (byte)(Convert.ToDouble(node.Properties[5]) * 255);
					byte b = (byte)(Convert.ToDouble(node.Properties[6]) * 255);
					property = new Property<Color>(node.Properties[0].ToString(), new Color(r, g, b));
					break;
				case "ColorAndAlpha":
					r = (byte)(Convert.ToDouble(node.Properties[4]) * 255);
					g = (byte)(Convert.ToDouble(node.Properties[5]) * 255);
					b = (byte)(Convert.ToDouble(node.Properties[6]) * 255);
					byte a = (byte)(Convert.ToDouble(node.Properties[7]) * 255);
					property = new Property<Color>(node.Properties[0].ToString(), new Color(r, g, b, a));
					break;
				case "Visibility":
				case "Bool":
				case "bool":
					property = new Property<bool>(node.Properties[0].ToString(), Convert.ToInt32(node.Properties[4]) != 0);
					break;
				case "Vector":
				case "Vector3":
				case "Vector3D":
				case "Lcl Translation":
				case "Lcl Rotation":
				case "Lcl Scaling":
					double x = Convert.ToDouble(node.Properties[4]);
					double y = Convert.ToDouble(node.Properties[5]);
					double z = Convert.ToDouble(node.Properties[6]);
					property = new Property<XYZ>(node.Properties[0].ToString(), new XYZ(x, y, z));
					break;
				case "int":
				case "Integer":
				case "Enum":
				case "enum":
					property = new Property<int>(node.Properties[0].ToString(), Convert.ToInt32(node.Properties[4]));
					break;
				case "KString":
					property = new Property<string>(node.Properties[0].ToString(), (string)node.Properties[4]);
					break;
				case "Float":
					property = new Property<float>(node.Properties[0].ToString(), Convert.ToSingle(node.Properties[4]));
					break;
				case "FieldOfView":
				case "FieldOfViewX":
				case "FieldOfViewY":
				case "double":
				case "Number":
					property = new Property<double>(node.Properties[0].ToString(), Convert.ToDouble(node.Properties[4]));
					break;
				case "KTime":
					property = new Property<TimeSpan>(node.Properties[0].ToString(), new TimeSpan(Convert.ToInt64(node.Properties[4])));
					break;
				case "Reference":
				case "Compound":
					property = new Property(node.Properties[0].ToString(), null);
					break;
				default:
					System.Diagnostics.Debug.Fail($"{node.Properties[1]}");
					break;
			}

			return property;
		}

		public Element3D BuildModel(FbxNode node)
		{
			Node model = new Node();

			List<Property> properties = new List<Property>();

			this.BuildElement(node, model, "Model::");

			foreach (FbxNode n in node.Nodes)
			{
				switch (n.Name)
				{
					case "MultiLayer":
						model.MultiLayer = (char)n.Value == 'T';
						break;
					case "MultiTake":
						model.MultiTake = (char)n.Value == 'T';
						break;
					case "Shading":
						model.Shading = (char)n.Value == 'T';
						break;
					case "Culling":
						model.Culling = (string)n.Value;
						break;
					default:
						if (!this.isCommonElementField(model, n, properties))
							this.notify($"Unknow node while building Model:: with name {n.Name}");
						break;
				}
			}

			//Process the properties
			foreach (Property p in properties)
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
				}

				model.Properties.Add(p);
			}

			//Get the children for this Node
			foreach (FbxNode n in this.getChildren(model.Id.Value))
			{
				Element3D child = this.ToElement(n);

				if (child == null)
					continue;

				model.Nodes.Add(child);
			}

			return model;
		}

		public Element3D BuildMaterial(FbxNode node)
		{
			Material material = new Material();

			List<Property> properties = new List<Property>();

			this.BuildElement(node, material, "Material::");

			foreach (FbxNode n in node.Nodes)
			{
				switch (n.Name)
				{
					case "ShadingModel":
						material.ShadingModel = (string)n.Value;
						break;
					case "MultiLayer":
						material.MultiLayer = Convert.ToInt32(n.Value);
						break;
					default:
						if (!this.isCommonElementField(material, n, properties))
							this.notify($"Unknow node while building Material:: with name {n.Name}");
						break;
				}
			}

			//Process the properties
			foreach (Property p in properties)
			{
				switch (p.Name)
				{
					case FbxProperty.AmbientColor:
						material.AmbientColor = (Color)p.Value;
						continue;
				}

				material.Properties.Add(p);
			}

			//Get fbxProperty values
			if (material.Properties.Contains("DiffuseColor"))
			{
				material.DiffuseColor = (Color)(material.Properties["DiffuseColor"]?.Value);
				material.Properties.Remove("DiffuseColor");
			}

			if (material.Properties.Contains("SpecularColor"))
			{
				material.SpecularColor = (Color)(material.Properties["SpecularColor"]?.Value);
				material.Properties.Remove("SpecularColor");
			}

			if (material.Properties.Contains("SpecularFactor"))
			{
				material.SpecularFactor = (double)(material.Properties["SpecularFactor"]?.Value);
				material.Properties.Remove("SpecularFactor");
			}

			if (material.Properties.Contains("ShininessExponent"))
			{
				material.ShininessExponent = (double)(material.Properties["ShininessExponent"]?.Value);
				material.Properties.Remove("ShininessExponent");
			}

			if (material.Properties.Contains("TransparencyFactor"))
			{
				material.TransparencyFactor = (double)(material.Properties["TransparencyFactor"]?.Value);
				material.Properties.Remove("TransparencyFactor");
			}

			if (material.Properties.Contains("EmissiveColor"))
			{
				material.EmissiveColor = (Color)(material.Properties["EmissiveColor"]?.Value);
				material.Properties.Remove("EmissiveColor");
			}

			if (material.Properties.Contains("EmissiveFactor"))
			{
				material.EmissiveFactor = (double)(material.Properties["EmissiveFactor"]?.Value);
				material.Properties.Remove("EmissiveFactor");
			}

			return material;
		}

		public Element3D BuildGeometryObject(FbxNode node)
		{
			Element3D geometry = null;

			switch (node.Properties[2].ToString())
			{
				case "Mesh":
					geometry = this.BuildMesh(node);
					break;
				default:
					this.notify($"Unknow geometry type with name {node.Properties[2]}");
					break;
			}

			return geometry;
		}

		public Mesh BuildMesh(FbxNode node)
		{
			Mesh mesh = new Mesh();

			List<Property> properties = new List<Property>();

			this.BuildElement(node, mesh, "Geometry::");

			foreach (FbxNode n in node)
			{
				switch (n.Name)
				{
					case "Vertices":
						mesh.Vertices = this.arrToXYZ(n.Value as double[]);
						break;
					case "Edges":
						mesh.Edges.AddRange(this.toArr<int>(n.Value as IEnumerable));
						break;
					case "PolygonVertexIndex":
						mesh.Polygons = this.buildPolygons(n.Value as int[]);
						break;
					default:
						if (!this.isCommonGeometryField(mesh, n, properties))
							this.notify($"Unknow node while building Geometry:: with name {n.Name}");
						break;
				}
			}

			//Process the properties
			foreach (Property p in properties)
			{
				mesh.Properties.Add(p);
			}

			return mesh;
		}

		public Element3D BuildNodeAttribute(FbxNode node)
		{
			switch (node.Properties[2].ToString())
			{
				case "Camera":
				case "Light":
				default:
					this.notify($"Unknow node while building NodeAttribute with name {node.Properties[2].ToString()}");
					break;
			}

			return null;
		}

		public LayerElement BuildLayerElementNormal(FbxNode node)
		{
			LayerElementNormal layer = new LayerElementNormal();

			foreach (FbxNode n in node)
			{
				switch (n.Name)
				{
					case "Normals":
						layer.Normals = this.arrToXYZ(this.arrToDoubleArray(n.Value as IEnumerable));
						break;
					case "NormalsW":
						layer.Weights.AddRange(this.arrToDoubleArray(n.Value as IEnumerable));
						break;
					default:
						if (!this.isCommonLayerField(layer, n))
							this.notify($"Unknow node while building LayerElement with name {n.Name}");
						break;
				}
			}

			return layer;
		}

		public LayerElement BuildLayerElementBinormal(FbxNode node)
		{
			LayerElementBinormal layer = new LayerElementBinormal();

			foreach (FbxNode n in node)
			{
				switch (n.Name)
				{
					case "Binormals":
						layer.Normals = this.arrToXYZ(this.arrToDoubleArray(n.Value as IEnumerable));
						break;
					case "BinormalsW":
						layer.Weights.AddRange(this.arrToDoubleArray(n.Value as IEnumerable));
						break;
					default:
						if (!this.isCommonLayerField(layer, n))
							this.notify($"Unknow node while building LayerElement with name {n.Name}");
						break;
				}
			}

			return layer;
		}

		public LayerElement BuildLayerElementTangent(FbxNode node)
		{
			LayerElementTangent layer = new LayerElementTangent();

			foreach (FbxNode n in node)
			{
				switch (n.Name)
				{
					case "Tangents":
						layer.Tangents = this.arrToXYZ(this.arrToDoubleArray(n.Value as IEnumerable));
						break;
					case "TangentsW":
						layer.Weights.AddRange(this.arrToDoubleArray(n.Value as IEnumerable));
						break;
					default:
						if (!this.isCommonLayerField(layer, n))
							this.notify($"Unknow node while building LayerElement with name {n.Name}");
						break;
				}
			}

			return layer;
		}

		public LayerElement BuildLayerElementUV(FbxNode node)
		{
			LayerElementUV layer = new LayerElementUV();

			foreach (FbxNode n in node)
			{
				switch (n.Name)
				{
					case "UV":
						layer.UV.AddRange(this.arrToXY(this.arrToDoubleArray(n.Value as IEnumerable)));
						break;
					case "UVIndex":
						layer.Indices.AddRange(this.toArr<int>(n.Value as IEnumerable));
						break;
					default:
						if (!this.isCommonLayerField(layer, n))
							this.notify($"Unknow node while building LayerElement with name {n.Name}");
						break;
				}
			}

			return layer;
		}

		public LayerElement BuildLayerElementSmoothing(FbxNode node)
		{
			LayerElementSmoothing layer = new LayerElementSmoothing();

			foreach (FbxNode n in node)
			{
				switch (n.Name)
				{
					case "Smoothing":
						layer.Smoothing.AddRange(this.toArr<int>(n.Value as IEnumerable));
						break;
					default:
						if (!this.isCommonLayerField(layer, n))
							this.notify($"Unknow node while building LayerElement with name {n.Name}");
						break;
				}
			}

			return layer;
		}

		public LayerElement BuildLayerElementMaterial(FbxNode node)
		{
			LayerElementMaterial layer = new LayerElementMaterial();

			foreach (FbxNode n in node)
			{
				switch (n.Name)
				{
					case "Materials":
						layer.Indices.AddRange(this.toArr<int>(n.Value as IEnumerable));
						break;
					default:
						if (!this.isCommonLayerField(layer, n))
							this.notify($"Unknow node while building LayerElement with name {n.Name}");
						break;
				}
			}

			return layer;
		}

		protected void checkFileSections()
		{
			foreach (var item in this._root)
			{
				switch (item.Name)
				{
					case "Documents":
						this.SectionDocuments = this.setRootSection(this.SectionDocuments, item);
						break;
					case "Objects":
						this.SectionObjects = this.setRootSection(this.SectionObjects, item);
						break;
					case "Connections":
						this.SectionConnections = this.setRootSection(this.SectionConnections, item);
						break;
					default:
						this.notify($"Unknown root node with name : {item.Name}");
						break;
				}
			}

			if (this.SectionDocuments == null)
				throw new FbxConverterException($"Root section not found: Documents");
			if (this.SectionObjects == null)
				throw new FbxConverterException($"Root section not found: Objects");
			if (this.SectionConnections == null)
				throw new FbxConverterException($"Root section not found: Connections");
		}

		protected FbxNode setRootSection(FbxNode property, FbxNode node)
		{
			if (property != null)
				throw new FbxConverterException($"Duplicate root node with name: {property.Name}");

			return node;
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

		protected IEnumerable<T> toArr<T>(IEnumerable arr)
		{
			foreach (var item in arr)
			{
				yield return (T)Convert.ChangeType(item, typeof(T));
			}
		}

		protected List<Polygon> buildPolygons(int[] arr)
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

		protected IEnumerable<FbxNode> getChildren(ulong containerId)
		{
			foreach (FbxNode c in this.SectionConnections.Nodes)
			{
				if (Convert.ToUInt64(c.Properties[2]) == containerId)
				{
					string connectionType = c.Properties[0].ToString();

					switch (connectionType)
					{
						case "OO":  // Object(source) to Object(destination).
							break;
						case "OP":  // Object(source) to Property(destination).
						case "PO":  // Property(source) to Object(destination).
						case "PP":  // Property(source) to Property(destination).
						default:
							throw new NotImplementedException();
					}

					ulong elementId = Convert.ToUInt64(c.Properties[1]);

					yield return this._objects[elementId];
				}
			}
		}

		private bool isCommonElementField(Element3D element, FbxNode node, List<Property> properties)
		{
			switch (node.Name)
			{
				case "Version":
				case "GeometryVersion":
					return true;
				default:
					return this.isPropertiesField(node, properties);
			}
		}

		private bool isCommonGeometryField(Geometry geometry, FbxNode node, List<Property> properties)
		{
			switch (node.Name)
			{
				case "Layer":
					//TODO: Process the layer node in the geometry
					return true;
				case "LayerElementNormal":
					geometry.Layers.Add(this.BuildLayerElementNormal(node));
					return true;
				case "LayerElementBinormal":
					geometry.Layers.Add(this.BuildLayerElementBinormal(node));
					return true;
				case "LayerElementTangent":
					geometry.Layers.Add(this.BuildLayerElementTangent(node));
					return true;
				case "LayerElementMaterial":
					geometry.Layers.Add(this.BuildLayerElementMaterial(node));
					return true;
				case "LayerElementUV":
					geometry.Layers.Add(this.BuildLayerElementUV(node));
					return true;
				case "LayerElementSmoothing":
					geometry.Layers.Add(this.BuildLayerElementSmoothing(node));
					return true;
				default:
					return this.isCommonElementField(geometry, node, properties);
			}
		}

		private bool isCommonLayerField(LayerElement layer, FbxNode node)
		{
			switch (node.Name)
			{
				case "Version":
					return true;
				case "Name":
					layer.Name = node.Value as string;
					return true;
				case "MappingInformationType":
					if (Enum.TryParse<MappingMode>((string)node.Value, out MappingMode mappingMode))
					{
						layer.MappingMode = mappingMode;
					}
					else
					{
						this.notify("Could not parse MappingMode");
					}
					return true;
				case "ReferenceInformationType":
					if (Enum.TryParse<ReferenceMode>((string)node.Value, out ReferenceMode referenceMode))
					{
						layer.ReferenceMode = referenceMode;
					}
					else
					{
						this.notify("Could not parse MappingMode");
					}
					return true;
				default:
					return false;
			}
		}

		private bool isPropertiesField(FbxNode node, List<Property> properties)
		{
			if (this._propertiesRegex.IsMatch(node.Name))
			{
				properties.AddRange(this.BuildProperties(node));
				return true;
			}
			else
			{
				return false;
			}
		}

		private Scene buildScene(FbxNode documents)
		{
			Scene scene = new Scene();

			scene.Id = 0;

			return scene;
		}

		private void notify(string message, [CallerMemberName] string caller = null)
		{
			this.OnNotification?.Invoke(this, new NotificationEventArgs($"{caller} | {message}"));
		}
	}
}
