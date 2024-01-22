using MeshIO.FBX.Writers.Objects;

namespace MeshIO.FBX.Connections
{
	internal class FbxConnection
	{
		public FbxConnectionType ConnectionType { get; }

		public string ParentId { get; }

		public string ChildId { get; }

		public IFbxObjectWriter Child { get; }

		public IFbxObjectWriter Parent { get; }

		public FbxConnection(IFbxObjectWriter child, IFbxObjectWriter parent)
		{
			Child = child;
			Parent = parent;
		}

		public FbxConnection(FbxConnectionType type, string parentId, string childId)
		{
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
	}
}
