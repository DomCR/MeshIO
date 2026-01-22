using System.Collections.Generic;
using MeshIO.Formats.Fbx.Readers;

namespace MeshIO.Formats.Fbx.Builders;

internal abstract class FbxObjectBuilder<T> : IFbxObjectBuilder
	where T : Element3D
{
	public string Id { get; set; }

	public string Name { get { return this._element.Name; } }

	public abstract string FbxObjectName { get; }

	public abstract string FbxTypeName { get; }

	public string Prefix { get { return $"{FbxObjectName}::"; } }

	public Dictionary<string, FbxProperty> FbxInstanceProperties { get; } = new();

	public FbxNode FbxNode { get; }

	protected readonly T _element;

	protected FbxObjectBuilder(T element)
	{
		_element = element;
		Id = element.Id.ToString();
	}

	protected FbxObjectBuilder(FbxNode node, T element) : this(element)
	{
		FbxNode = node;
		Id = node?.GetProperty<object>(0).ToString();
	}

	public Element3D GetElement()
	{
		return _element;
	}

	public virtual void Build(FbxFileBuilderBase builder)
	{
		FbxPropertyBuilder template = builder.GetProperties(FbxObjectName);

		if (builder.Version < FbxVersion.v7000)
		{
			_element.Id = IdUtils.CreateId();
			_element.Name = removePrefix(FbxNode.GetProperty<string>(0));
		}
		else
		{
			_element.Name = removePrefix(FbxNode.GetProperty<string>(1));
		}

		Dictionary<string, FbxProperty> nodeProp = builder.ReadProperties(FbxNode);
		foreach (var t in template.Properties)
		{
			if (nodeProp.ContainsKey(t.Key))
			{
				continue;
			}

			nodeProp.Add(t.Key, t.Value);
		}

		buildProperties(nodeProp);
	}

	protected string removePrefix(string fullname)
	{
		if (string.IsNullOrEmpty(fullname))
		{
			return string.Empty;
		}
		else if (fullname.StartsWith(Prefix))
		{
			return fullname.Remove(0, Prefix.Length);
		}

		return fullname;
	}

	protected virtual void buildProperties(Dictionary<string, FbxProperty> properties)
	{
		foreach (var prop in properties)
		{
			_element.Properties.Add(prop.Value.ToProperty());
		}
	}
}
