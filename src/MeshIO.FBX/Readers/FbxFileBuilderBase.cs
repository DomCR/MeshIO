using MeshIO.Core;
using MeshIO.FBX.Connections;
using MeshIO.FBX.Templates;
using System;
using System.Collections.Generic;

namespace MeshIO.FBX.Readers
{
	internal abstract class FbxFileBuilderBase
	{
		public event NotificationEventHandler OnNotification;

		public FbxVersion Version { get { return this.Root.Version; } }

		public FbxRootNode Root { get; }

		public FbxReaderOptions Options { get; }

		protected readonly Scene _scene;

		protected readonly IFbxObjectTemplate _rootTemplate;

		protected readonly Dictionary<string, FbxPropertyTemplate> _propertyTemplates = new();

		protected readonly Dictionary<string, IFbxObjectTemplate> _objectTemplates = new();

		protected readonly Dictionary<string, List<FbxConnection>> _connections = new();

		protected FbxFileBuilderBase(FbxRootNode root, FbxReaderOptions options)
		{
			this.Root = root;
			this.Options = options;

			this._scene = new Scene();
			this._scene.RootNode.Id = 0;
			this._rootTemplate = new FbxRootNodeTemplate(this._scene.RootNode);
		}

		public static FbxFileBuilderBase Create(FbxRootNode root, FbxReaderOptions options)
		{
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
				case FbxVersion.v6000:
				case FbxVersion.v6100:
					throw new NotSupportedException($"Fbx version {root.Version} no supported for reader");
				case FbxVersion.v7000:
				case FbxVersion.v7100:
				case FbxVersion.v7200:
				case FbxVersion.v7300:
				case FbxVersion.v7400:
				case FbxVersion.v7500:
				case FbxVersion.v7600:
				case FbxVersion.v7700:
					return new FbxFileBuilder7000(root, options);
				default:
					throw new NotSupportedException($"Unknown Fbx version {root.Version} for writer");

			}
		}

		public Scene Read()
		{
			foreach (FbxNode n in Root)
			{
				switch (n.Name)
				{
					case FbxFileToken.FBXHeaderExtension:
						this.readHeader(n);
						break;
					case FbxFileToken.GlobalSettings:
						this.readGlobalSettings(n);
						break;
					case FbxFileToken.Documents:
						this.readDocuments(n);
						break;
					case FbxFileToken.References:
						this.readReferences(n);
						break;
					case FbxFileToken.Definitions:
						this.readDefinitions(n);
						break;
					case FbxFileToken.Objects:
						this.readObjects(n);
						break;
					case FbxFileToken.Connections:
						this.readConnections(n);
						break;
					default:
						this.Notify($"Unknown section: {n.Name}", NotificationType.Warning);
						break;
				}
			}

			this.buildScene();

			return this._scene;
		}

		public Dictionary<string, FbxProperty> ReadProperties(FbxNode node)
		{
			Dictionary<string, FbxProperty> properties = new Dictionary<string, FbxProperty>();
			if (!node.TryGetNode(FbxFileToken.GetPropertiesName(this.Version), out FbxNode propertiesNode))
			{
				return properties;
			}

			foreach (FbxNode propNode in propertiesNode)
			{
				FbxProperty prop = this.readFbxProperty(propNode);
				if (prop is null)
					continue;

				properties.Add(prop.Name, prop);
			}

			return properties;
		}

		public bool TryGetTemplate(string id, out IFbxObjectTemplate template)
		{
			return this._objectTemplates.TryGetValue(id, out template);
		}

		public FbxPropertyTemplate GetProperties(string objName)
		{
			if (this._propertyTemplates.TryGetValue(objName, out var properties))
			{
				return properties;
			}
			else
			{
				return new FbxPropertyTemplate();
			}
		}

		public List<FbxConnection> GetChildren(string id)
		{
			if (this._connections.TryGetValue(id, out List<FbxConnection> children))
			{
				return children;
			}
			else
			{
				return new List<FbxConnection>();
			}
		}

		protected FbxProperty readFbxProperty(FbxNode node)
		{
			string name = node.GetProperty<string>(0);
			string type1 = node.GetProperty<string>(1);
			string label = node.GetProperty<string>(2);
			PropertyFlags flags = FbxProperty.ParseFlags(node.GetProperty<string>(3));

			object value = null;

			if(node.Properties.Count == 4)
			{
				value = null;
			}
			else if (node.Properties.Count == 5)
			{
				value = node.Properties[4];
			}
			else
			{
				value = new List<object>();
				for (int i = 4; i < node.Properties.Count; i++)
				{
					(value as List<object>).Add(node.Properties[i]);
				}
			}

			return new FbxProperty(name, type1, label, flags, value);
		}

		protected void buildScene()
		{
			this._rootTemplate.Build(this);
		}

		protected void readHeader(FbxNode node)
		{
			this.Notify("FBXHeaderExtension section not implemented", NotificationType.NotImplemented);

			foreach (FbxNode n in node)
			{
				switch (n.Name)
				{
					default:
						break;
				}
			}
		}

		protected void readGlobalSettings(FbxNode node)
		{
			Dictionary<string, FbxProperty> properties = this.ReadProperties(node);
			FbxPropertyTemplate globalSettings = new FbxPropertyTemplate(FbxFileToken.GlobalSettings, string.Empty, properties);
			this._propertyTemplates.Add(FbxFileToken.GlobalSettings, globalSettings);
		}

		protected void readDocuments(FbxNode node)
		{
			foreach (FbxNode n in node)
			{
				switch (n.Name)
				{
					case FbxFileToken.Count:
						break;
					case FbxFileToken.Document:
						this.readDocument(n);
						break;
					default:
						this.Notify($"{node.Name} | unknown node: {n.Name}", NotificationType.NotImplemented);
						break;
				}
			}
		}

		protected void readDocument(FbxNode node)
		{

		}

		protected void readReferences(FbxNode node)
		{
			if (!node.IsEmpty)
			{
				this.Notify("References section not implemented", NotificationType.NotImplemented);
			}
		}

		protected void readDefinitions(FbxNode node)
		{
			foreach (FbxNode n in node)
			{
				switch (n.Name)
				{
					case FbxFileToken.Count:
					case FbxFileToken.Version:
						break;
					case FbxFileToken.ObjectType:
						this.readDefinition(n);
						break;
					default:
						this.Notify($"{node.Name} | unknown node: {n.Name}", NotificationType.NotImplemented);
						break;
				}
			}
		}

		protected void readDefinition(FbxNode node)
		{
			if (!node.TryGetProperty<string>(0, out string objectType))
			{
				this.Notify($"Undefined ObjectType", NotificationType.Warning);
				return;
			}

			if (objectType == FbxFileToken.GlobalSettings)
			{
				return;
			}

			string name = string.Empty;
			if (!node.TryGetNode("PropertyTemplate", out FbxNode tempalteNode))
			{
				this.Notify($"PropertyTemplate not found for {objectType}", NotificationType.Warning);
				return;
			}

			if (!tempalteNode.TryGetProperty<string>(0, out name))
			{
				this.Notify($"PropertyTemplate name not found for {objectType}", NotificationType.Warning);
				return;
			}

			Dictionary<string, FbxProperty> properties = this.ReadProperties(tempalteNode);
			FbxPropertyTemplate template = new FbxPropertyTemplate(objectType, name, properties);
			this._propertyTemplates.Add(objectType, template);
		}

		protected void readObjects(FbxNode node)
		{
			foreach (FbxNode n in node)
			{
				IFbxObjectTemplate template = null;

				switch (n.Name)
				{
					case FbxFileToken.Model:
						template = new FbxNodeTemplate(n);
						break;
					case FbxFileToken.Geometry:
						template = this.readGeometryNode(n);
						break;
				}

				if (template == null)
				{
					this.Notify($"[{node.Name}] unknown node: {n}", NotificationType.NotImplemented);
					continue;
				}

				if (string.IsNullOrEmpty(template.Id))
				{
					this.Notify($"[{node.Name}] Id not found for node {n}", NotificationType.Warning);
					continue;
				}

				this._objectTemplates.Add(template.Id, template);
			}
		}

		protected IFbxObjectTemplate readGeometryNode(FbxNode node)
		{
			string type = node.GetProperty<string>(2);

			switch (type)
			{
				case FbxFileToken.Mesh:
					return new FbxMeshTemplate(node);
				default:
					return null;
			}
		}

		protected void readConnections(FbxNode node)
		{
			foreach (FbxNode n in node)
			{
				FbxConnection connection;

				FbxConnectionType type = FbxConnection.Parse(n.GetProperty<string>(0));
				string child = n.GetProperty<object>(1).ToString();
				string parent = n.GetProperty<object>(2).ToString();

				connection = new FbxConnection(type, parent, child);

				if (!this._connections.TryGetValue(parent, out List<FbxConnection> children))
				{
					children = new List<FbxConnection>();
					this._connections.Add(parent, children);
				}

				children.Add(connection);
			}
		}

		public void Notify(string message, NotificationType notificationType = NotificationType.Information, Exception ex = null)
		{
			this.OnNotification?.Invoke(this, new NotificationEventArgs(message, notificationType, ex));
		}
	}
}
