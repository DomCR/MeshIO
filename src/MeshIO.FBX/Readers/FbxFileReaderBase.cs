using MeshIO.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MeshIO.FBX.Readers
{
	internal abstract class FbxFileReaderBase
	{
		public event NotificationEventHandler OnNotification;

		public FbxRootNode Root { get; }

		public FbxReaderOptions Options { get; }

		protected readonly Scene _scene;

		protected readonly Dictionary<string, FbxPropertyTemplate> _propertyTemplates = new();

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

			}

			return properties;
		}

		protected void readHeader(FbxNode node)
		{
			foreach (FbxNode n in node)
			{
				switch (n.Name)
				{
					default:
						break;
				}
			}
		}

		protected void readGlobalSettings(FbxNode n)
		{
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
		}

		protected void readReferences(FbxNode n)
		{
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

			Dictionary<string, FbxProperty> properties = this.ReadProperties(tempalteNode[FbxFileToken.GetPropertiesName(this.Root.Version)]);
			FbxPropertyTemplate template = new FbxPropertyTemplate(objectType, name, properties);
			this._propertyTemplates.Add(objectType, template);
		}

		protected void readObjects(FbxNode n)
		{
			throw new NotImplementedException();
		}

		protected void readConnections(FbxNode node)
		{

		}

		protected void notify(string message, NotificationType notificationType = NotificationType.Information, Exception ex = null)
		{
			this.OnNotification?.Invoke(this, new NotificationEventArgs(message, notificationType, ex));
		}
	}
}
