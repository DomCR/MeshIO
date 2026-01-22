using MeshIO.Formats.Fbx.Builders;
using MeshIO.Formats.Fbx.Connections;
using MeshIO.Formats.Fbx.Templates;
using System;
using System.Collections.Generic;

namespace MeshIO.Formats.Fbx.Readers;

internal abstract class FbxFileBuilderBase
{
	public event NotificationEventHandler OnNotification;

	public FbxVersion Version { get { return this.Root.Version; } }

	public FbxRootNode Root { get; }

	public FbxReaderOptions Options { get; }

	protected readonly Scene _scene;

	protected readonly IFbxObjectBuilder _rootTemplate;

	protected readonly Dictionary<string, FbxPropertyBuilder> _propertyTemplates = new();

	protected readonly Dictionary<string, IFbxObjectBuilder> _objectTemplates = new();

	protected readonly Dictionary<string, List<FbxConnection>> _connections = new();

	protected FbxFileBuilderBase(FbxRootNode root, FbxReaderOptions options)
	{
		this.Root = root;
		this.Options = options;

		this._scene = new Scene();
		this._scene.RootNode.Id = 0;
		this._rootTemplate = new FbxRootNodeBuilder(this._scene.RootNode);
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
				throw new NotSupportedException($"Fbx version {root.Version} no supported for reader");
			case FbxVersion.v6000:
			case FbxVersion.v6100:
				return new FbxFileBuilder6000(root, options);
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

	public Scene Build()
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
				case FbxFileToken.Document:
					this.readDocument(n);
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

	public bool TryGetTemplate(string id, out IFbxObjectBuilder template)
	{
		return this._objectTemplates.TryGetValue(id, out template);
	}

	public FbxPropertyBuilder GetProperties(string objName)
	{
		if (this._propertyTemplates.TryGetValue(objName, out var properties))
		{
			return properties;
		}
		else
		{
			return new FbxPropertyBuilder();
		}
	}

	public List<FbxConnection> GetChildren(string id)
	{
		if (this._connections.TryGetValue(id, out List<FbxConnection> children))
		{
			return children;
		}

		if (this.Version < FbxVersion.v7000)
		{
			if (this._connections.TryGetValue($"Model::{id}", out children))
			{
				return children;
			}
		}

		return new List<FbxConnection>();
	}

	protected FbxProperty readFbxProperty(FbxNode node)
	{
		string name = node.GetProperty<string>(0);
		string type1 = node.GetProperty<string>(1);
		string label = string.Empty;
		PropertyFlags flags;
		if (this.Version < FbxVersion.v7000)
		{
			flags = FbxProperty.ParseFlags(node.GetProperty<string>(2));
		}
		else
		{
			label = node.GetProperty<string>(2);
			flags = FbxProperty.ParseFlags(node.GetProperty<string>(3));
		}

		int valueIndex = this.Version < FbxVersion.v7000 ? 3 : 4;
		object value = null;

		if (node.Properties.Count == valueIndex)
		{
			value = null;
		}
		else if (node.Properties.Count == valueIndex + 1)
		{
			value = node.Properties[valueIndex];
		}
		else
		{
			value = new List<object>();
			for (int i = valueIndex; i < node.Properties.Count; i++)
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
		FbxPropertyBuilder globalSettings = new FbxPropertyBuilder(FbxFileToken.GlobalSettings, string.Empty, properties);
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
		foreach (FbxNode n in node)
		{
			switch (n.Name)
			{
				case "Name":
					this._rootTemplate.Id = n.Value.ToString();
					break;
			}
		}
	}

	protected void readReferences(FbxNode node)
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
					this.Notify($"[{node.Name}] unknown node: {n.Name}", NotificationType.NotImplemented);
					break;
			}
		}
	}

	protected void readDefinition(FbxNode node)
	{
		if (!node.TryGetProperty(0, out string objectType))
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
			return;
		}

		if (!tempalteNode.TryGetProperty(0, out name))
		{
			return;
		}

		Dictionary<string, FbxProperty> properties = this.ReadProperties(tempalteNode);
		FbxPropertyBuilder template = new FbxPropertyBuilder(objectType, name, properties);
		this._propertyTemplates.Add(objectType, template);
	}

	protected void readObjects(FbxNode node)
	{
		foreach (FbxNode n in node)
		{
			IFbxObjectBuilder template = null;

			switch (n.Name)
			{
				case FbxFileToken.GlobalSettings:
					this.readGlobalSettings(n);
					continue; ;
				case FbxFileToken.Model:
					template = new FbxNodeBuilder(n);
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

	protected IFbxObjectBuilder readGeometryNode(FbxNode node)
	{
		string type = node.GetProperty<string>(2);

		switch (type)
		{
			case FbxFileToken.Mesh:
				return new FbxMeshBuilder(node);
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
