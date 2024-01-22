using CSMath;
using MeshIO.Core;
using MeshIO.Entities.Geometries;
using MeshIO.FBX.Connections;
using MeshIO.FBX.Readers.Templates;
using System;
using System.Collections.Generic;

namespace MeshIO.FBX.Readers
{
	internal abstract class FbxFileReaderBase
	{
		public event NotificationEventHandler OnNotification;

		public FbxVersion Version { get { return this.Root.Version; } }

		public FbxRootNode Root { get; }

		public FbxReaderOptions Options { get; }

		protected readonly Scene _scene;

		protected readonly Dictionary<string, FbxPropertyTemplate> _propertyTemplates = new();

		protected readonly Dictionary<string, IFbxObjectTemplate> _objectTemplates = new();

		protected readonly Dictionary<string, List<FbxConnection>> _connections = new();

		protected FbxFileReaderBase(FbxRootNode root, FbxReaderOptions options)
		{
			this.Root = root;
			this.Options = options;
			this._scene = new Scene();
			this._scene.RootNode.Id = 0;
		}

		public static FbxFileReaderBase Create(FbxRootNode root, FbxReaderOptions options)
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
					return new FbxFileReader7000(root, options);
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
						this.notify($"Unknown section: {n.Name}", NotificationType.Warning);
						break;
				}
			}

			this.buildScene();

			return this._scene;
		}

		public Dictionary<string, FbxProperty> ReadProperties(FbxNode node)
		{
			Dictionary<string, FbxProperty> properties = new Dictionary<string, FbxProperty>();
			if (node == null)
			{
				return new();
			}

			foreach (FbxNode propNode in node)
			{
				FbxProperty prop = this.readFbxProperty(propNode);
				if (prop is null)
					continue;

				properties.Add(prop.Name, prop);
			}

			return properties;
		}

		protected FbxProperty readFbxProperty(FbxNode node)
		{
			string name = node.GetProperty<string>(0);
			string type1 = node.GetProperty<string>(1);
			string label = node.GetProperty<string>(2);
			PropertyFlags flags = FbxProperty.ParseFlags(node.GetProperty<string>(3));
			List<object> arr = new();

			for (int i = 4; i < node.Properties.Count; i++)
			{
				arr.Add(node.Properties[i]);
			}

			return new FbxProperty(name, type1, label, flags, arr);
		}

		protected void buildScene()
		{

		}

		protected void readHeader(FbxNode node)
		{
			this.notify("FBXHeaderExtension section not implemented", NotificationType.NotImplemented);

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
			this.notify("GlobalSettings section not implemented", NotificationType.NotImplemented);
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
						this.notify($"{node.Name} | unknown node: {n.Name}", NotificationType.NotImplemented);
						break;
				}
			}
		}

		protected void readDocument(FbxNode node)
		{
			this.notify("Document not implemented", NotificationType.NotImplemented);
		}

		protected void readReferences(FbxNode node)
		{
			this.notify("References section not implemented", NotificationType.NotImplemented);
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
						this.notify($"{node.Name} | unknown node: {n.Name}", NotificationType.NotImplemented);
						break;
				}
			}
		}

		protected void readDefinition(FbxNode node)
		{
			if (!node.TryGetProperty<string>(0, out string objectType))
			{
				this.notify($"Undefined ObjectType", NotificationType.Warning);
				return;
			}

			if (objectType == FbxFileToken.GlobalSettings)
			{
				return;
			}

			string name = string.Empty;
			if (!node.TryGetNode("PropertyTemplate", out FbxNode tempalteNode))
			{
				this.notify($"PropertyTemplate not found for {objectType}", NotificationType.Warning);
				return;
			}

			if (!tempalteNode.TryGetProperty<string>(0, out name))
			{
				this.notify($"PropertyTemplate name not found for {objectType}", NotificationType.Warning);
				return;
			}

			Dictionary<string, FbxProperty> properties = this.ReadProperties(tempalteNode[FbxFileToken.GetPropertiesName(this.Version)]);
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
					this.notify($"[{node.Name}] unknown node: {n}", NotificationType.NotImplemented);
					continue;
				}

				if (string.IsNullOrEmpty(template.TemplateId))
				{
					this.notify($"[{node.Name}] Id not found for node {n}", NotificationType.Warning);
					continue;
				}

				this._objectTemplates.Add(template.TemplateId, template);
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
				string parent = n.GetProperty<object>(1).ToString();
				string child = n.GetProperty<object>(2).ToString();

				connection = new FbxConnection(type, parent, child);

				if (!this._connections.TryGetValue(parent, out List<FbxConnection> children))
				{
					children = new List<FbxConnection>();
					this._connections.Add(parent, children);
				}

				children.Add(connection);
			}
		}

		protected void notify(string message, NotificationType notificationType = NotificationType.Information, Exception ex = null)
		{
			this.OnNotification?.Invoke(this, new NotificationEventArgs(message, notificationType, ex));
		}
	}
}
