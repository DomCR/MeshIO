using System.Collections.Generic;

namespace MeshIO.Formats.Fbx.Builders;

public class FbxPropertyBuilder
{
	public string ObjectTypeName { get; }

	public string Name { get; }

	public Dictionary<string, FbxProperty> Properties { get; } = new();

	public FbxPropertyBuilder() : this(string.Empty, string.Empty, []) { }

	public FbxPropertyBuilder(string objectTypeName, string name, Dictionary<string, FbxProperty> properties)
	{
		this.ObjectTypeName = objectTypeName;
		this.Name = name;
		this.Properties = properties;
	}
}