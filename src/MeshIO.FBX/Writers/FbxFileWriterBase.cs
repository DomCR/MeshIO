using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using MeshIO.FBX.Connections;
using MeshIO.FBX.Templates;

namespace MeshIO.FBX.Writers
{
	internal abstract class FbxFileWriterBase
	{
		public FbxVersion Version { get { return this.Options.Version; } }

		public FbxWriterOptions Options { get; }

		public Scene Scene { get; }

		public Node RootNode { get { return this.Scene.RootNode; } }

		protected readonly Dictionary<string, FbxPropertyTemplate> _tempaltes = new();

		protected readonly Dictionary<string, List<IFbxObjectTemplate>> _definedObjects = new();

		protected readonly Dictionary<ulong, IFbxObjectTemplate> _objectTemplates = new();

		protected readonly List<FbxConnection> _connections = new();

		private readonly FbxRootNode fbxRoot;

		private readonly string MeshIOVersion;

		protected FbxFileWriterBase(Scene scene, FbxWriterOptions options)
		{
			this.Scene = scene;
			this.Options = options;

			this.fbxRoot = new FbxRootNode
			{
				Version = this.Options.Version
			};

			this.MeshIOVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
		}

		public static FbxFileWriterBase Create(Scene scene, FbxWriterOptions options)
		{
			FbxVersion version = options.Version;
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
					throw new NotSupportedException($"Fbx version {version} no supported for writer");
				case FbxVersion.v7000:
				case FbxVersion.v7100:
				case FbxVersion.v7200:
				case FbxVersion.v7300:
				case FbxVersion.v7400:
				case FbxVersion.v7500:
				case FbxVersion.v7600:
				case FbxVersion.v7700:
					return new FbxFileWriter7000(scene, options);
				default:
					throw new NotSupportedException($"Unknown Fbx version {version} for writer");

			}
		}

		public FbxRootNode ToNodeStructure()
		{
			this.initializeRoot();

			this.fbxRoot.Nodes.Add(this.nodeFBXHeaderExtension());

			if (this.Options.IsBinaryFormat)
			{
				byte[] id = new byte[16];
				Random random = new Random();
				random.NextBytes(id);
				this.fbxRoot.Add("FileId", id);

				this.fbxRoot.Add("CreationTime", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss:fff", CultureInfo.InvariantCulture));

				this.fbxRoot.Add("Creator", $"MeshIO.FBX {this.MeshIOVersion}");
			}

			this.fbxRoot.Nodes.Add(this.nodeGlobalSettings());
			this.fbxRoot.Nodes.Add(this.nodeDocuments());
			this.fbxRoot.Nodes.Add(this.nodeReferences());
			this.fbxRoot.Nodes.Add(this.nodeDefinitions());
			this.fbxRoot.Nodes.Add(this.nodeObjects());
			this.fbxRoot.Nodes.Add(this.nodeConnections());

			return this.fbxRoot;
		}

		public bool TryGetPropertyTemplate(string fbxName, out FbxPropertyTemplate template)
		{
			return this._tempaltes.TryGetValue(fbxName, out template);
		}

		public void CreateConnection(Element3D child, IFbxObjectTemplate parent)
		{
			IFbxObjectTemplate objwriter = FbxTemplateFactory.Create(child);
			if (objwriter is null)
			{
				return;
			}

			FbxConnection conn = new FbxConnection(objwriter, parent);

			this._connections.Add(conn);

			objwriter.ProcessChildren(this);

			this._objectTemplates.Add(child.Id.Value, objwriter);
			if (!this._definedObjects.TryGetValue(objwriter.FbxObjectName, out List<IFbxObjectTemplate> lst))
			{
				this._definedObjects.Add(objwriter.FbxObjectName, lst = new List<IFbxObjectTemplate>());
			}
			lst.Add(objwriter);
		}

		protected void initializeRoot()
		{
			//Root node should be processed to create the connections but it is not writen in the file
			this.RootNode.Id = 0;

			IFbxObjectTemplate root = FbxTemplateFactory.Create(this.RootNode);

			root.ProcessChildren(this);
		}


		private FbxNode nodeFBXHeaderExtension()
		{
			FbxNode header = new FbxNode(FbxFileToken.FBXHeaderExtension);

			header.Nodes.Add(new FbxNode(FbxFileToken.FBXHeaderVersion, 1003));
			header.Nodes.Add(new FbxNode("FBXVersion", (int)this.Version));

			if (this.Options.IsBinaryFormat)
			{
				header.Add("EncryptionType", 0);
			}

			DateTime now = DateTime.Now;
			FbxNode tiemespan = new FbxNode(FbxFileToken.CreationTimeStamp);
			tiemespan.Nodes.Add(new FbxNode(FbxFileToken.Version, 1000));
			tiemespan.Nodes.Add(new FbxNode(nameof(now.Year), now.Year));
			tiemespan.Nodes.Add(new FbxNode(nameof(now.Month), now.Month));
			tiemespan.Nodes.Add(new FbxNode(nameof(now.Day), now.Day));
			tiemespan.Nodes.Add(new FbxNode(nameof(now.Hour), now.Hour));
			tiemespan.Nodes.Add(new FbxNode(nameof(now.Minute), now.Minute));
			tiemespan.Nodes.Add(new FbxNode(nameof(now.Second), now.Second));
			tiemespan.Nodes.Add(new FbxNode(nameof(now.Millisecond), now.Millisecond));
			header.Nodes.Add(tiemespan);

			header.Add(FbxFileToken.Creator, $"MeshIO.FBX {this.MeshIOVersion}");

			return header;

			throw new NotImplementedException();
		}

		private FbxNode nodeGlobalSettings()
		{
			FbxGlobalSettingsTemplate globalSettings = new FbxGlobalSettingsTemplate();

			FbxNode settings = new FbxNode(FbxFileToken.GlobalSettings);

			settings.Nodes.Add(new FbxNode(FbxFileToken.Version, 100));

			settings.Nodes.Add(this.PropertiesToNode(globalSettings.FbxProperties));

			this._definedObjects.Add(FbxFileToken.GlobalSettings, new List<IFbxObjectTemplate> { globalSettings });

			return settings;
		}

		private FbxNode nodeDocuments()
		{
			FbxNode documents = new FbxNode(FbxFileToken.Documents);

			documents.Nodes.Add(new FbxNode(FbxFileToken.Count, this.Scene.SubScenes.Count + 1));

			var doc = documents.Add(FbxFileToken.Document, this.Scene.GetIdOrDefault(), this.Scene.Name, FbxFileToken.Scene);
			doc.Add(FbxFileToken.RootNode, Convert.ToInt64(this.RootNode.Id));

			return documents;
		}

		private FbxNode nodeReferences()
		{
			FbxNode references = new FbxNode(FbxFileToken.References);

			references.Nodes.Add(null);

			return references;
		}

		private FbxNode nodeDefinitions()
		{
			FbxNode definitions = new FbxNode(FbxFileToken.Definitions);

			definitions.Nodes.Add(new FbxNode(FbxFileToken.Version, 100));
			definitions.Nodes.Add(new FbxNode(FbxFileToken.Count, this._definedObjects.Sum(o => o.Value.Count)));

			foreach (var item in this._definedObjects)
			{
				FbxNode d = new FbxNode(FbxFileToken.ObjectType, item.Key);
				d.Nodes.Add(new FbxNode(FbxFileToken.Count, item.Value.Count));

				if (item.Key == FbxFileToken.GlobalSettings)
				{
					definitions.Nodes.Add(d);
					continue;
				}

				FbxPropertyTemplate template = FbxPropertyTemplate.Create(item.Key);

				this._tempaltes.Add(item.Key, template);

				var t = new FbxNode("PropertyTemplate", template.Name);
				t.Nodes.Add(this.PropertiesToNode(template.Properties.Values));

				d.Nodes.Add(t);

				definitions.Nodes.Add(d);
			}

			return definitions;
		}

		private FbxNode nodeObjects()
		{
			FbxNode objects = new FbxNode(FbxFileToken.Objects);

			foreach (IFbxObjectTemplate obj in this._objectTemplates.Values)
			{
				if (!this._tempaltes.TryGetValue(obj.FbxObjectName, out FbxPropertyTemplate template))
				{
					template = new FbxPropertyTemplate();
				}

				obj.ApplyTemplate(template);

				objects.Nodes.Add(obj.ToFbxNode(this));
			}

			return objects;
		}

		private FbxNode nodeConnections()
		{
			FbxNode connections = new FbxNode(FbxFileToken.Connections);

			foreach (FbxConnection c in this._connections)
			{
				FbxNode con = connections.Add("C");

				switch (c.ConnectionType)
				{
					case FbxConnectionType.ObjectObject:
						con.Properties.Add("OO");
						break;
					default:
						throw new NotImplementedException();
				}

				con.Properties.Add(long.Parse(c.Child.Id));
				con.Properties.Add(long.Parse(c.Parent.Id));
			}

			return connections;
		}

		public FbxNode PropertiesToNode(IEnumerable<Property> properties)
		{
			if (!properties.Any())
			{
				return null;
			}

			FbxNode node = new FbxNode(FbxFileToken.GetPropertiesName(this.Version));

			foreach (Property p in properties)
			{
				if (p is not FbxProperty fbxProp)
				{
					fbxProp = FbxProperty.CreateFrom(p);
				}

				node.Nodes.Add(fbxProp.ToNode());
			}

			return node;
		}
	}
}
