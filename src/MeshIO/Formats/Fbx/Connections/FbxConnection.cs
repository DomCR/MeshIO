using MeshIO.Formats.Fbx.Builders;

namespace MeshIO.Formats.Fbx.Connections;

internal class FbxConnection
{
	public FbxConnectionType ConnectionType { get; }

	public string ParentId { get; }

	public string ChildId { get; }

	public IFbxObjectBuilder Child { get; }

	public IFbxObjectBuilder Parent { get; }

	public FbxConnection(IFbxObjectBuilder child, IFbxObjectBuilder parent)
	{
		Child = child;
		Parent = parent;
	}

	public FbxConnection(FbxConnectionType type, string parentId, string childId)
	{
		ConnectionType = type;
		ParentId = parentId;
		ChildId = childId;
	}

	public string GetComment()
	{
		return $"{Child.FbxObjectName}::{Child.Name}, {Parent.FbxObjectName}::{Parent.Name}";
	}

	public static FbxConnectionType Parse(string type)
	{
		switch (type)
		{
			case "OO":
				return FbxConnectionType.ObjectObject;
			case "OP":
				return FbxConnectionType.ObjectProperty;
			case "PO":
				return FbxConnectionType.PropertyObject;
			case "PP":
				return FbxConnectionType.PropertyProperty;
			default:
				throw new System.ArgumentException($"Unknown Fbx connectin type: {type}", nameof(type));
		}
	}

	public override string ToString()
	{
		return $"{ConnectionType} {ChildId} | {ParentId}";
	}
}
