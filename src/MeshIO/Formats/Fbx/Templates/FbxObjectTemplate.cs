using System;
using System.Collections.Generic;
using MeshIO.Formats.Fbx.Writers;

namespace MeshIO.Formats.Fbx.Templates;

internal abstract class FbxObjectTemplate<T> : IFbxObjectTemplate
	where T : Element3D
{
	public Dictionary<string, FbxProperty> FbxInstanceProperties { get; } = new();

	public FbxNode FbxNode { get; }

	public abstract string FbxObjectName { get; }

	public abstract string FbxTypeName { get; }

	public string Id { get; set; }

	public bool Is6000Fbx { get { return this.Version < FbxVersion.v7000; } }

	public string Name { get { return this._element.Name; } }

	public string Prefix { get { return $"{this.FbxObjectName}::"; } }

	public FbxVersion Version { get; }

	protected readonly T _element;

	protected FbxObjectTemplate(FbxVersion version, T element)
	{
		this.Version = version;
		this._element = element;
		this.Id = element.Id.ToString();
	}

	public virtual void ApplyTemplate(FbxPropertyTemplate template)
	{
		foreach (Property item in this._element.Properties)
		{
			if (template.Properties.TryGetValue(item.Name, out FbxProperty property)
				&& item.Value == property.Value)
			{
				continue;
			}

			this.FbxInstanceProperties.Add(item.Name, FbxProperty.CreateFrom(item));
		}
	}

	public Element3D GetElement()
	{
		return this._element;
	}

	public string GetIdByVersion(FbxVersion version)
	{
		if (version < FbxVersion.v7000)
		{
			if (string.IsNullOrEmpty(this._element.Name))
			{
				this._element.Name = $"id_{this._element.GetIdOrDefault().ToString()}";
			}

			return $"{this.FbxObjectName}::{this._element.Name}";
		}
		else
		{
			return this._element.GetIdOrDefault().ToString();
		}
	}

	public virtual void ProcessChildren(FbxFileWriterBase fbxFileWriterBase)
	{
	}

	public FbxNode ToFbxNode(FbxFileWriterBase writer)
	{
		FbxNode n = this.nodeHeader(writer.Version);

		this.addObjectBody(n, writer);

		return n;
	}

	protected virtual void addObjectBody(FbxNode node, FbxFileWriterBase writer)
	{
		node.Nodes.Add(writer.PropertiesToNode(this.FbxInstanceProperties.Values));
	}

	protected FbxNode nodeHeader(FbxVersion version)
	{
		if (version < FbxVersion.v7000)
		{
			return new FbxNode(this.FbxObjectName, $"{this.FbxObjectName}::{this._element.Name}", this.FbxTypeName);
		}
		else
		{
			return new FbxNode(this.FbxObjectName, this.getId(), $"{this.FbxObjectName}::{this._element.Name}", this.FbxTypeName);
		}
	}

	protected virtual void processProperties(Dictionary<string, FbxProperty> properties)
	{
		foreach (var prop in properties)
		{
			this._element.Properties.Add(prop.Value.ToProperty());
		}
	}

	private long getId()
	{
		if (!this._element.Id.HasValue)
		{
			this._element.Id = IdUtils.CreateId();
		}

		return Math.Abs((long)this._element.Id.Value);
	}
}