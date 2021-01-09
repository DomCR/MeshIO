using System;
using System.Collections.Generic;
using System.Text;

namespace MeshIO.FBX.Nodes.Connections
{
	public class FbxConnection<T> : FbxConnection
		where T : class
	{
		public T ContainerId { get; set; }
		public T ElementId { get; set; }
	}
}
