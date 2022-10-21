using System.Collections.Generic;

namespace MeshIO.Elements
{
	public class Scene : Element
	{
		public List<Node> Nodes { get; set; } = new List<Node>();

		public Scene() : base() { }

		public Scene(string name) : base(name) { }
	}
}
