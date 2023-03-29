using System;
using System.Collections.Generic;

namespace MeshIO.Elements
{
	public class Scene : Element
	{
		[Obsolete("Use the RootNode instead")]
		public List<Node> Nodes { get; set; } = new List<Node>();

		public Node RootNode { get; }

		public Scene() : this(string.Empty) { }

		public Scene(string name) : base(name)
		{
			this.RootNode = new Node();
		}
	}
}
