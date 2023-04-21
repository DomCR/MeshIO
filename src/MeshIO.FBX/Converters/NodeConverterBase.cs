using CSMath;
using MeshIO.Entities.Geometries.Layers;
using MeshIO.Entities.Geometries;
using MeshIO.FBX.Exceptions;
using MeshIO.Shaders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MeshIO.FBX.Converters.Mappers;

namespace MeshIO.FBX.Converters
{
	/// <summary>
	/// Base class to convert a node structure fbx <see cref="FbxRootNode"/> into a <see cref="Scene"/>
	/// </summary>
	public abstract class NodeConverterBase : ConverterBase, INodeConverter
	{
		public const string TokenModel = "Model";

		public const string TokenGeometry = "Geometry";

		public const string TokenMaterial = "Material";

		public string PrefixSeparator { get { return "::"; } }

		public abstract string PropertiesToken { get; }

		public FbxVersion Version { get { return this._root.Version; } }

		public FbxDocumentsMapper MapDocuments { get; private set; } = new FbxDocumentsMapper();

		public FbxDefinitionsMapper MapDefinitions { get; private set; } = new FbxDefinitionsMapper();

		public FbxObjectsMapper MapObjects { get; private set; } = new FbxObjectsMapper();

		public FbxConnectionMapper MapConnections { get; private set; } = new FbxConnectionMapper();

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

		protected NodeConverterBase(FbxRootNode root)
		{
			this._root = root;
		}

		public Scene ConvertScene()
		{
			this.createMappers();

			Scene scene = this.MapDocuments.RootScene;
			this.MapObjects.MapElements(this.MapDefinitions);

			this.assignSubNodes(scene.RootNode);

			return scene;
		}

		private void createMappers()
		{
			List<string> mappedSections = new List<string>();

			this.createMapper(this.MapDocuments, mappedSections);
			this.createMapper(this.MapDefinitions, mappedSections);
			this.createMapper(this.MapObjects, mappedSections);
			this.createMapper(this.MapConnections, mappedSections);

			foreach (FbxNode item in this._root)
			{
				if (!mappedSections.Contains(item.Name))
				{
					this.notify($"Seciton not implemented {item.Name}", Core.NotificationType.NotImplemented);
				}
			}
		}

		private void createMapper<T>(T mapper, List<string> mapped)
			where T : IFbxMapper
		{
			mapper.OnNotification += this.mapperOnNotification;

			if (this._root.TryGetNode(mapper.SectionName, out FbxNode node))
			{
				mapper.Map(node);
				mapped.Add(mapper.SectionName);
			}
			else
			{
				this.notify("", Core.NotificationType.Warning);
			}
		}

		private void assignSubNodes(Node node)
		{
			foreach (ulong id in this.MapConnections.GetChildren(node.Id))
			{
				if (this.MapObjects.ObjectMap.TryGetValue(id, out Element3D sub))
				{
					node.Nodes.Add(sub);

					if (sub is Node n)
					{
						this.assignSubNodes(n);
					}
				}
				else
				{
					this.notify($"Unable to find {id}", Core.NotificationType.Warning);
				}
			}
		}

		protected void mapperOnNotification(object sender, Core.NotificationEventArgs e)
		{
			this.notify(e.Message, e.NotificationType, e.Exception);
		}
	}
}
