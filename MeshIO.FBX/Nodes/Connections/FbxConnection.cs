using MeshIO.FBX.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeshIO.FBX.Nodes.Connections
{
	//TODO: fbx versions lower than 6000, the connection is made by the name
	public class FbxConnection : FbxNodeReference
	{
		public override string ClassName { get { return "C"; } }
		public FbxConnectionType ConnectionType { get; set; }
		public ulong Container { get; set; }
		public ulong Element { get; set; }
		public FbxConnection() { }
		public FbxConnection(FbxNode node)
		{
			if (Enum.TryParse<FbxConnectionType>(node.Properties[0].ToString(), out FbxConnectionType type))
				ConnectionType = type;
			else
				throw new FbxNodeException($"The connection type {node.Properties[0]} doesn't exist.");

			Element = Convert.ToUInt64(node.Properties[1]);
			Container = Convert.ToUInt64(node.Properties[2]);
		}
		public override FbxNode ToFbxNode()
		{
			return new FbxNode(ClassName, ConnectionType.ToString(), Element, Container);
		}
	}
}
