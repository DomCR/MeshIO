using MeshIO.FBX.Writers.Objects;

namespace MeshIO.FBX.Writers.Connections
{
	internal class FbxConnection
	{
		public FbxConnectionType ConnectionType { get; }

		public IFbxObjectWriter Child { get; }

		public IFbxObjectWriter Parent { get; }

		public FbxConnection(IFbxObjectWriter child, IFbxObjectWriter parent)
		{
			this.Child = child;
			this.Parent = parent;
		}

		public string GetComment()
		{
			return $"{Child.FbxObjectName}::{Child.Name}, {Parent.FbxObjectName}::{Parent.Name}";
		}
	}
}
